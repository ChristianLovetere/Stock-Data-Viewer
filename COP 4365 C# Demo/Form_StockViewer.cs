using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace COP_4365_C__Demo //Project 3 Christian Lovetere U46489387
{
    public partial class Form_StockViewer : Form
    {
        //List that will be populated with candlesticks from the csv file (starting capacity 1024)
        List<Candlestick> listOfCandlesticks = new List<Candlestick>(1024);

        /// <summary>
        /// init a binding list to update the candlesticks live. This variable must be global so it can be written to by the filter 
        /// method and accessed for the normalize and display functions. The bindinglist contains smartCandlesticks, because this
        /// data type is required to display the annotations. As such, only the binding list needs to contain this type, because
        /// it is presented to the end user.
        /// </summary>
        BindingList<smartCandlestick> boundSmartCandlesticks;

        /// <summary>
        /// global dictionary with one of each recognizer, where the key is the pattern it recognizes. This will be referenced by the
        /// recognizeAll function, in which the dictionary will be iterated through and each recognizer will be run on each candlestick.
        /// Global so that its easy to access and add to.
        /// </summary>
        Dictionary<string, Recognizer> dictOfRecog = new Dictionary<string, Recognizer>
        {
            {"isBearish", new Class_Recognizer_Bearish() }, //recognizers in order that they were made in
            {"isBullish", new Class_Recognizer_Bullish() },
            {"isNeutral", new Class_Recognizer_Neutral() },
            {"isMarubozu", new Class_Recognizer_Marubozu() },
            {"isHammer", new Class_Recognizer_Hammer() },
            {"isDoji", new Class_Recognizer_Doji() },
            {"isDragonflyDoji", new Class_Recognizer_DragonflyDoji() },
            {"isGravestoneDoji", new Class_Recognizer_GravestoneDoji() },
            {"isBearishEngulfing", new Class_Recognizer_BearishEngulfing() },
            {"isBullishEngulfing", new Class_Recognizer_BullishEngulfing() },
            {"isBearishHarami", new Class_Recognizer_BearishHarami() },
            {"isBullishHarami", new Class_Recognizer_BullishHarami() },
            {"isPeak", new Class_Recognizer_Peak() },
            {"isValley", new Class_Recognizer_Valley() }
        };
                                                              
        /// <summary>
        /// This constructor is for all forms that are not the parent. The parent is opened when the program is launched.
        /// This constructor is used for the child forms. It takes in the date for the start and end date picker and sets them
        /// initially. It also takes in the stock to be displayed in the form of its file path. So, when the user picks multiple
        /// stocks in the parent form, the second and onward stocks picked will be fed to this constructor for each stock, and
        /// display immediately upon the child form opening.
        /// </summary>
        /// <param name="stockPath"></param> a string containing the filepath for the stock to be displayed.
        /// <param name="start"></param> the start date in the parent's datetime pickers. Simply copies this range to the child.
        /// <param name="end"></param> the end date in the parent's datetime pickers. Simply copies this range to the child.
        public Form_StockViewer(string stockPath, DateTime start, DateTime end)
        {
            InitializeComponent();

            //set the start and end dates
            DateTimePicker_StartDate.Value = start;
            DateTimePicker_EndDate.Value = end;

            //Go read the file and place the returned list of candlesticks in listOfCandlesticks
            listOfCandlesticks = readCandlesticksFromFile_internal(stockPath);
            filterCandlesticks(); //place candlesticks in a bindinglist of smart candlesticks
            NormalizeCandlesticks(); //maximize readability of chart based on bindinglist entries
            DisplayCandlesticks(); //update chart to display to the end user
        }

        public Form_StockViewer() //required function for designer support
        {
            InitializeComponent();
        }

        private void button_pickStock_Click(object sender, EventArgs e) 
        {
            //brings up the menu for selecting a file for the ticker chooser. The file dialog has a filter that lets the user pick
            //between daily, weekly, or monthly data. It is configured to allow the picking of multiple stocks.
            openFileDialog_TickerChooser.ShowDialog(); 
        }
        /// <summary>
        /// when one or more applicable .csv files are given to the openFileDialog, create a bound list of smartCandlesticks from 
        /// the stock data and bind this bound list to both the Chart, so that it can be updated based on specified date range 
        /// when requested by the user, and show different annotations representing the requested candlestick pattern.
        /// </summary>
        private void openFileDialog_TickerChooser_FileOk(object sender, CancelEventArgs e) 
        {
            int numOfFiles = openFileDialog_TickerChooser.FileNames.Count(); //numOfFiles is the amount of files requested to be analyzed

            //loop that opens one form for each file requested to be analyzed. The first file does not open a new form, but just
            //displays in the parent form.
            for (int i = 0; i < numOfFiles; i++) 
            {
                string pathName = openFileDialog_TickerChooser.FileNames[i]; //pick out the pathname from the array of pathnames
                string ticker = Path.GetFileNameWithoutExtension(pathName); //string that holds the stock ticker for later displaying

                //construct new stockViewer form with the start date, end date, and .csv file path.
                Form_StockViewer stockViewer = new Form_StockViewer(pathName, DateTimePicker_StartDate.Value, DateTimePicker_EndDate.Value);
                if (i == 0) //if this is the first stock,
                {
                    stockViewer = this; //we are working on the parent form

                    readCandlesticksFromFile_internal(pathName); //read candlesticks and create list of candlesticks for all data
                    filterCandlesticks(); //create binding list of smartCandlesticks that only fall between start and end date, evaluate patterns for all candlesticks
                    NormalizeCandlesticks(); //edit chart y-axis to maximize readability, 
                    DisplayCandlesticks(); //update the chart with the new bound smartcandlesticks
                    clearAnnotations(); //if there were any annotations on the parent form previously, clear them.

                    stockViewer.Text = "Parent: " + ticker; //the name of the parent form should be "Parent: (ticker)"
                }
                else //if this is NOT the first stock,
                {
                    //instantiate a new form with the .csv file's path name, and the start and end date.
                    stockViewer = new Form_StockViewer(pathName, DateTimePicker_StartDate.Value, DateTimePicker_EndDate.Value);
                    stockViewer.Text = "Child: " + ticker; //the name of the child form should be "Child: (ticker)"
                }
                stockViewer.Show(); //display the form
                stockViewer.BringToFront(); //bring all forms to the front, above your other tasks
            }
        }
        /// <summary>
        /// call the readCandlesticksFromFile_internal method to make candlesticks from the selected .csv
        /// </summary>
        private void readCandlesticksFromFile()
        {
            readCandlesticksFromFile_internal(openFileDialog_TickerChooser.FileName); //generate the list of candlesticks from the stock data
        }
        /// <summary>
        /// take in comma separated values of candlestick info and build a list of candlesticks by iterating through the
        /// lines of the .csv file and parsing out the date, open, high, low, close, and volume for each line / entry,
        /// then constructing the candlestick from it and adding all candlesticks to a comprehensive list
        /// </summary>
        /// <param name="filename"></param> the file name of the file to evaluate
        /// <returns></returns>
        private List<Candlestick> readCandlesticksFromFile_internal(string filename) 
        {
            //string filename = openFileDialog_TickerChooser.FileName; //hold the filename / path / location in this string

            const string referenceString = "Date,Open,High,Low,Close,Adj Close,Volume"; //This string is a model for the
                                                                                        //expected header for the .csv file

            //Pass the file path and file name to the new StreamReader
            using (StreamReader sr = new StreamReader(filename)) //automatically close the file accessed by the streamreader
                                                                 //when the using block is finished
            {
                //if the listOfCandlesticks is not empty, clear it. If it is empty, continue
                if ( listOfCandlesticks != null ) { listOfCandlesticks.Clear(); } 

                string line = sr.ReadLine(); //store the first line of the .csv to the "line" string

                if (line == referenceString) //if the line matches the expected header, continue scanning through the entire
                                             //.csv to parse out stock data
                {
                    while ((line = sr.ReadLine()) != ",,,,,," && line != null) //while the line is not empty
                    {
                        Candlestick cs = new Candlestick(line); //create a new candlestick with the line of data
                        listOfCandlesticks.Add(cs); //add that candlestick to the listOfCandlesticks
                    }
                }
                else
                { Text = "Bad File"; } //if the initial line does not match the expected stock data header, print "Bad File"
                                       //to the task header
            }
            return listOfCandlesticks; //return the comprehensive list
        }
        /// <summary>
        /// create a binding list of smartCandlesticks from the filtered list returned by the internal filter function. This binding
        /// list is saved to the global binding list accessible everywhere. We also call recognizeAll here to ensure the binding
        /// list is fully set with a bool for every pattern we're looking to identify.
        /// </summary>
        private void filterCandlesticks()
        {
            List <smartCandlestick> filteredListOfSmartCandlesticks = filterCandlesticks_internal(listOfCandlesticks,
                                                                                                  DateTimePicker_StartDate.Value,
                                                                                                  DateTimePicker_EndDate.Value);
            recognizeAll(filteredListOfSmartCandlesticks);
            //create the binding list of scs with the filtered candlestick list and the requested date range
            boundSmartCandlesticks = new BindingList<smartCandlestick>(filteredListOfSmartCandlesticks);
        }
        /// <summary>
        /// create and return a filtered sublist of smartCandlesticks that only contains smartCandlesticks between start date 
        /// and end date.
        /// </summary>
        /// <param name="originalList"> the list you want to pass in to filter
        /// <param name="startDate"> the start date with type DateTime
        /// <param name="endDate"> the end date with type DateTime
        /// <returns> returns the filtered sublist of scs
        private List<smartCandlestick> filterCandlesticks_internal(List<Candlestick> originalList, DateTime startDate, DateTime endDate)
        {
            List<Candlestick> selectedCandlesticks = originalList //create a copy list of candlesticks with only the desired
                                                                  //date range using LINQ

                //'c' is an instance of candlestick in the list. Where the value of c.date is between the start date and end date,
                //send it to the list. This LINQ implementation is somewhat inefficient because it does not break out after passing
                //the end date, meaning it will check the entire list even when we know we have passed the range of values
                //that we want to keep.
                .Where(c => c.date >= startDate && c.date <= endDate)
                .ToList();

            //a list of smart candlesticks with enough space to store all of the candlesticks selected above.
            List<smartCandlestick> selectedSmartCandlesticks = new List<smartCandlestick>(selectedCandlesticks.Count);

            foreach (Candlestick cs in selectedCandlesticks) //convert the filtered list of cs into a filtered list of scs
            {
                smartCandlestick scs = new smartCandlestick(cs); //construct an scs out of each cs in the list selected above
                selectedSmartCandlesticks.Add(scs); //then add that scs to the scs list
            }
            return selectedSmartCandlesticks; //return the filtered list of scs
        }
        /// <summary>
        /// takes in the comprehensive list of candlesticks, and uses the sliding window technique to acquire n = patternlength candlesticks
        /// at a time, checks for the pattern across the candlesticks, then moves to the next n candlesticks. For example, for a pattern that
        /// needs 3 candlesticks, add the first 3 to the temp list, check the pattern for number 3, then remove number 1 and add number 4,
        /// check for number 4, and so on. For each key in the dictionary of recognizers, process each candlestick in the comprehensive list.
        /// </summary>
        /// <param name="lscs"></param>
        /// <returns></returns>
        private void recognizeAll(List<smartCandlestick> lscs)
        {
            if (lscs.Count() == 0) //do nothing if theres no list
                return;

            foreach (string key in dictOfRecog.Keys) //evaluate every pattern in the dictionary of recognizers
            {
                dictOfRecog[key].RecognizeAll(lscs);
            }
            return;
        }
        /// <summary>
        /// call the internal normalize method to change the chart's y-axis so it minimizes white space above and below the candlesticks
        /// </summary>
        private void NormalizeCandlesticks()
        {
            NormalizeCandlesticks_internal(boundSmartCandlesticks); //call internal func with the boundSmartCandlesticks list
        }
        /// <summary>
        /// change the y-axis of the Chart so that it clearly shows all candlesticks while minimizing white space above and below.
        /// This method takes in the bindinglist whose max high and min high should be found. The boundSmartCandlesticks bindinglist
        /// is global in this context, but the internal method is reusable in applications where the bindinglist is not global.
        /// However, this internal method does not return anything because it makes changes to the format of a forms object,
        /// meaning there is no need to return a value.
        /// </summary>
        private void NormalizeCandlesticks_internal(BindingList<smartCandlestick> listOfSmartCandlesticksToNormalize) {
            
            decimal lowest_low; //the lowest 'low' in the data set
            decimal highest_high; //the highest 'high' in the data set

            if (listOfSmartCandlesticksToNormalize.Count > 0) //if the boundSmartCandlesticks list is not empty,
            {
                lowest_low = listOfSmartCandlesticksToNormalize[0].low; //set the lowest_low to the low of the first entry
                highest_high = listOfSmartCandlesticksToNormalize[0].high; //set the highest_high to the high of the first entry
            }
            else //if the selectedCandlesticks list IS empty
            {
                //set the lowest_low and highest_high to default values (nothing is displayed, so the numbers don't really matter)
                lowest_low = 0; 
                highest_high = 100; 
            }

            foreach (smartCandlestick scs in listOfSmartCandlesticksToNormalize) //look at all the candlesticks and:
            {
                if (scs.low < lowest_low) //keep updating the lowest_low as the lowest 'low' in the data set
                    lowest_low = scs.low;
                if (scs.high > highest_high) //keep updating the highest_high as the highest 'high' in the data set
                    highest_high = scs.high;
            }
            //display variable that serves as a rounded version of the lowest_low. for the chart's Y-axis
            double lowest_low_display = Math.Round((double)lowest_low * 0.98, 0);
            //display variable that serves as a rounded version of the highest_high. for the chart's Y-axis
            double highest_high_display = Math.Round((double)highest_high * 1.02, 0);

            //adjust the lowest point on the y-axis to be 2% lower than the lowest low. 
            chart_Candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Minimum = lowest_low_display;

            //adjust the highest point on the y-axis to be 2% higher than the highest high.
            chart_Candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Maximum = highest_high_display;

            //The ChartAreas need 'double' type, which is why the display variables are of type double, and why their values
            //require the lowest_low and highest_high respectively to be typecasted from decimal to double.
        }
        /// <summary>
        /// call the internal function to display the boundSmartCandlesticks list by setting the data source for the chart and bind
        /// the chart.
        /// </summary>
        private void DisplayCandlesticks()
        {
            DisplayCandlesticks_internal(boundSmartCandlesticks); //call internal func with boundSmartCandlesticks
        }
        /// <summary>
        /// Set the data source for the chart to the supplied binding list of smartCandlesticks and bind the chart.
        /// Since this method just messes with MS Forms design objects, there is no need to return anything directly.
        /// The boundSmartCandlesticks bindinglist is a global variable and does not necessarily need to be passed into the internal
        /// version, but this version is reusable in contexts where the desired bindinglist to display is NOT a global variable.
        /// </summary>
        private void DisplayCandlesticks_internal(BindingList<smartCandlestick> bindingListOfSmartCandlesticksToDisplay)
        {
            chart_Candlesticks.DataSource = bindingListOfSmartCandlesticksToDisplay; //bind the chart to this new bound list
            chart_Candlesticks.DataBind();
        }
        /// <summary>
        /// update the chart by changing the displayed candlesticks to only include candlesticks which occur between the 
        /// user-specified start and end date
        /// </summary>
        private void button_update_Click(object sender, EventArgs e)
        {
            filterCandlesticks(); //create sublist based on specified date range and bindinglist of smartCandlestick based on the sublist
            NormalizeCandlesticks(); //normalize with respect to the new bindinglist
            DisplayCandlesticks(); //set the data source for the visual to the new binding list of smartCandlestick
            ShowAnnotations(); //display annotations if a pattern is selected
        }
        /// <summary>
        /// Displays new annotations if any are requested. Will always clear the previous annotations before determining the new ones.
        /// </summary>
        private void ShowAnnotations()
        {
            clearAnnotations(); //clear old annots first

            //if no pattern has been selected yet, exit this function.
            if (comboBox_CandlestickPatterns.SelectedItem == null)
                return;

            //this string stores the comboBox selection. Manually add the 'is' back in, so that it can reference the dictionary
            //without each comboBox element having to say 'is' at the start.
            string pattern = comboBox_CandlestickPatterns.SelectedItem.ToString();

            int i = 0; //iterator for the foreach loop to keep track of which smartCandlestick we are referencing.
            foreach (smartCandlestick scs in boundSmartCandlesticks)
            {
                if (scs.dictOfPatternProperties[pattern] == true) //if the current scs matches the requested pattern,
                {
                    int patternSize = dictOfRecog[pattern].patternLength; //separate var for the pattern length for readability

                    ArrowAnnotation arw = new ArrowAnnotation(); //make a new arrow annotation

                    arw.Width = 0; arw.Height = -5; //set the arrow to point straight down with some length.
                    arw.BackColor = Color.Black; //set arrow color to black, more readable for large data sets

                    //equation to make the size of the arrows inversely proportional with the number of candlesticks shown.
                    if (75 / boundSmartCandlesticks.Count < 1) //if theres over 75 candlesticks, force arrow size to the minimum.
                        arw.ArrowSize = 1;
                    else //if theres less than 75 candlesticks, arrow size is calculated as below:
                        arw.ArrowSize = 1 + (75 / boundSmartCandlesticks.Count);

                    arw.AnchorDataPoint = chart_Candlesticks.Series["Series_OHLC"].Points[i]; //anchor arrow to the main candlestick in the pattern.                                                                        
                    chart_Candlesticks.Annotations.Add(arw); //add the annotation to the chart

                    if (patternSize > 1) //if the pattern size is greater than 1
                    {
                        ArrowAnnotation arw2 = new ArrowAnnotation(); //make a new arrow annotation                                       
                        arw2.Width = 0; arw2.Height = -2.5; //set the arrow to point straight down with some length.
                        arw2.BackColor = Color.Yellow; //set arrow color to yellow. This arrow denotes supporting candlesticks in the pattern.

                        //equation to make the size of the arrows inversely proportional with the number of candlesticks shown.
                        if (75 / boundSmartCandlesticks.Count < 1) //if theres over 75 candlesticks, force arrow size to the minimum.
                            arw2.ArrowSize = 1;
                        else //if theres less than 75 candlesticks, arrow size is calculated as below:
                            arw2.ArrowSize = 1 + (75 / boundSmartCandlesticks.Count);

                        arw2.AnchorDataPoint = chart_Candlesticks.Series["Series_OHLC"].Points[i-1]; //anchor arrow to the previous candlestick to our black arrowed one
                        
                        chart_Candlesticks.Annotations.Add(arw2); //add the annotation to the chart

                        if (patternSize > 2) //if the pattern size is greater than 2
                        {
                            ArrowAnnotation arw3 = new ArrowAnnotation(); //make a new arrow annotation                                       
                            arw3.Width = 0; arw3.Height = -2.5; //set the arrow to point straight down with some length.
                            arw3.BackColor = Color.Yellow; //set arrow color to yellow. This arrow denotes supporting candlesticks in the pattern.

                            //equation to make the size of the arrows inversely proportional with the number of candlesticks shown.
                            if (75 / boundSmartCandlesticks.Count < 1) //if theres over 75 candlesticks, force arrow size to the minimum.
                                arw3.ArrowSize = 1;
                            else //if theres less than 75 candlesticks, arrow size is calculated as below:
                                arw3.ArrowSize = 1 + (75 / boundSmartCandlesticks.Count);

                            arw3.AnchorDataPoint = chart_Candlesticks.Series["Series_OHLC"].Points[i+1]; //anchor arrow to the candlestick next to our black arrowed one

                            chart_Candlesticks.Annotations.Add(arw3); //add the annotation to the chart
                        }
                    }
                }
                i++; //increment iterator.
            }
        }
        /// <summary>
        /// This function simply gets rid of all the annotations in the chart if there are any.
        /// </summary>
        private void clearAnnotations()
        {
            if (chart_Candlesticks.Annotations.Count > 0) //if theres annotations,
                chart_Candlesticks.Annotations.Clear(); //clear them.
        }
        /// <summary>
        /// When the stock viewer is loaded, fill the comboBox with every pattern type in the dictOfRecog dictionary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_StockViewer_Load(object sender, EventArgs e)
        {
            foreach (string key in dictOfRecog.Keys) 
            {
                comboBox_CandlestickPatterns.Items.Add(key); //add to comboBox
            }
        }
    }
}