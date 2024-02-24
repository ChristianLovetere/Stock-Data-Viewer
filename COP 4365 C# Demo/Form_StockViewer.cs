using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COP_4365_C__Demo
{
    public partial class Form_StockViewer : Form
    {
        //List that will be populated with candlesticks from the csv file (starting capacity 1024)
        List<Candlestick> listOfCandlesticks = new List<Candlestick>(1024);
        BindingList<Candlestick> boundCandlesticks; //init a binding list to update the candlesticks live. This variable must
                                                    //be global so it can be written to by the filter method and read out of
                                                    //for the normalize and display functions

        public Form_StockViewer() //required function for designer support
        {
            InitializeComponent();
        }

        private void button_pickStock_Click(object sender, EventArgs e) 
        {
            openFileDialog_TickerChooser.ShowDialog(); //brings up the menu for selecting a file for the ticker chooser
                                                       //The file dialog has a filter that lets the user pick whether they want
                                                       //daily, weekly, or monthly data
        }
        /// <summary>
        /// when an applicable .csv file is given to the openFileDialog, create a bound list of candlesticks from the
        /// stock data and bind this bound list to both the dataGridView and the Chart, so that it can be updated based on 
        /// specified date range when requested by the user.
        /// </summary>
        private void openFileDialog_TickerChooser_FileOk(object sender, CancelEventArgs e) 
        {
            readCandlesticksFromFile(); //parse through the .csv file with stock data and create a candlestick for each row of data

            filterCandlesticks(); //apply date time pickers as the date time range and make a sublist of only the candlstikcs
                                  //in that range
            NormalizeCandlesticks(); //Change min y and max y of candlestick chart so as to show all candlesticks without
                                     //extra white space
            DisplayCandlesticks(); //bind the sublist to the dataGridView and the Chart
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
        /// <param name="filename"></param>
        /// <returns></returns>
        private List<Candlestick> readCandlesticksFromFile_internal(string filename) 
        {
            //string filename = openFileDialog_TickerChooser.FileName; //hold the filename / path / location in this string
            this.Text = filename; //set the text in the task header to the file's location

            const string referenceString = "Date,Open,High,Low,Close,Adj Close,Volume"; //This string is a model for the
                                                                                        //expected header for the .csv file

            //Pass the file path and file name to the new StreamReader
            using (StreamReader sr = new StreamReader(filename)) //automatically close the file accessed by the streamreader
                                                                 //when the using block is finished
            {
                if ( listOfCandlesticks != null ) { listOfCandlesticks.Clear(); } //if the listOfCandlesticks is not empty,
                                                                                  //clear it. If it is empty, continue

                string line = sr.ReadLine(); //store the first line of the .csv to the "line" string

                if (line == referenceString) //if the line matches the expected header, continue scanning through the entire
                                             //.csv to parse out stock data
                {
                    while ((line = sr.ReadLine()) != ",,,,,," && line != null) //while the line is not empty
                    {
                        Candlestick cs = new Candlestick(line); //create a new candlestick with the line of data
                        listOfCandlesticks.Add(cs); //add that candlestick to the listOfCandlesticks
                    }
                    listOfCandlesticks.Reverse(); //reverse the order of the list so the candlesticks display oldest on the
                                                  //left and newest on the right
                }
                else
                { Text = "Bad File"; } //if the initial line does not match the expected stock data header, print "Bad File"
                                       //to the task header
            }
            return listOfCandlesticks;
        }
        /// <summary>
        /// create a binding list of candlesticks from the filtered list returned by the internal filter function. This binding
        /// list is saved to the global binding list accessible everywhere
        /// </summary>
        private void filterCandlesticks()
        {
            //create the binding list with the filtered candlestick list
            boundCandlesticks = new BindingList<Candlestick>(filterCandlesticks_internal(listOfCandlesticks, 
                                                                                         DateTimePicker_StartDate.Value,
                                                                                         DateTimePicker_EndDate.Value)
                                                                                        );
        }
        /// <summary>
        /// create and return a filtered sublist of candlesticks that only contains candlesticks between start date and end date.
        /// </summary>
        /// <param name="originalList"> the list you want to pass in to filter
        /// <param name="startDate"> the start date with type DateTime
        /// <param name="endDate"> the end date with type DateTime
        /// <returns> returns the filtered sublist
        private List<Candlestick> filterCandlesticks_internal(List<Candlestick> originalList, DateTime startDate, DateTime endDate)
        {
            List<Candlestick> selectedCandlesticks = originalList //create a copy list of candlesticks with only the desired
                                                                  //date range using LINQ

                //'c' is an instance of candlestick in the list. Where the value of c.date is between the start date and end date,
                //send it to the list. This LINQ implementation is somewhat inefficient because it does not break out after passing
                //the end date, meaning it will check the entire list even when we know we have passed the range of values
                //that we want to keep.
                .Where(c => c.date >= startDate && c.date <= endDate)
                .ToList();

            return selectedCandlesticks;
        }
        /// <summary>
        /// call the internal normalize method to change the chart's y-axis so it minimizes white space above and below the candlesticks
        /// </summary>
        private void NormalizeCandlesticks()
        {
            NormalizeCandlesticks_internal(boundCandlesticks); //call internal func with the boundCandlesticks list
        }
        /// <summary>
        /// change the y-axis of the Chart so that it clearly shows all candlesticks while minimizing white space above and below.
        /// This method takes in the bindinglist whose max high and min high should be found. The boundCandlesticks bindinglist
        /// is global in this context, but the internal method is reusable in applications where the bindinglist is not global.
        /// However, this internal method does not return anything because it makes changes to the format of a forms object,
        /// meaning there is no need to return a value.
        /// </summary>
        private void NormalizeCandlesticks_internal(BindingList<Candlestick> listOfCandlesticksToNormalize) {
            
            decimal lowest_low; //the lowest 'low' in the data set
            decimal highest_high; //the highest 'high' in the data set

            if (listOfCandlesticksToNormalize.Count > 0) //if the boundCandlesticks list is not empty,
            {
                lowest_low = listOfCandlesticksToNormalize[0].low; //set the lowest_low to the low of the first entry
                highest_high = listOfCandlesticksToNormalize[0].high; //set the highest_high to the high of the first entry
            }
            else //if the selectedCandlesticks list IS empty
            {
                lowest_low = 0; //set the lowest_low and highest_high to default values (nothing is displayed, so the numbers don't
                                //really matter)
                highest_high = 100; 
            }

            foreach (Candlestick cs in listOfCandlesticksToNormalize) //look at all the candlesticks and:
            {
                if (cs.low < lowest_low) //keep updating the lowest_low as the lowest 'low' in the data set
                    lowest_low = cs.low;
                if (cs.high > highest_high) //keep updating the highest_high as the highest 'high' in the data set
                    highest_high = cs.high;
            }
            double lowest_low_display = Math.Round((double)lowest_low * 0.98, 0); //display variable that serves as a rounded
                                                                                  //version of the lowest_low. for the chart's Y-axis
            double highest_high_display = Math.Round((double)highest_high * 1.02, 0); //display variable that serves as a rounded
                                                                                      //version of the highest_high. for the chart's Y-axis

            chart_Candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Minimum = lowest_low_display;
            //adjust the lowest point on the y-axis to be 2% lower than the lowest low. 

            chart_Candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Maximum = highest_high_display;
            //adjust the highest point on the y-axis to be 2% higher than the highest high.

            //The ChartAreas need 'double' type, which is why the display variables are of type double, and why their values
            //require the lowest_low and highest_high respectively to be typecasted from decimal to double.

        }
        /// <summary>
        /// call the internal function to display the boundCandlesticks list by setting the data source for the dataGridView
        /// and the chart as well as binding the chart.
        /// </summary>
        private void DisplayCandlesticks()
        {
            DisplayCandlesticks_internal(boundCandlesticks); //call internal func with boundCandlesticks
        }
        /// <summary>
        /// Set the data source for the dataGridView and chart to the supplied binding list of candlesticks and bind the chart.
        /// Since this method just messes with MS Forms design objects, there is no need to return anything directly.
        /// The boundCandlesticks bindinglist is a global variable and does not necessarily need to be passed into the internal
        /// version, but this version is reusable in contexts where the desired bindinglist to display is NOT a global variable.
        /// </summary>
        private void DisplayCandlesticks_internal(BindingList<Candlestick> listOfCandlesticksToDisplay)
        {
            dataGridView_Candlesticks.DataSource = listOfCandlesticksToDisplay; //bind the data grid to this new bound list

            chart_Candlesticks.DataSource = listOfCandlesticksToDisplay; //bind the chart to this new bound list
            chart_Candlesticks.DataBind();
        }
        /// <summary>
        /// update the dataGridView and the Chart by changing the displayed candlesticks to only include candlesticks which
        /// occur between the user-specified start and end date
        /// </summary>
        private void button_update_Click(object sender, EventArgs e)
        {
            filterCandlesticks(); //create sublist based on specified date range and bindinglist based on the sublist
            NormalizeCandlesticks(); //normalize with respect to the new bindinglist
            DisplayCandlesticks(); //set the data source for the visuals to the new binding list derived from the sublist
        }
    }
}