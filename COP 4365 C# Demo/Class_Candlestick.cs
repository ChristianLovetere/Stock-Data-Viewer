using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COP_4365_C__Demo
{
    /// <summary>
    /// Creates a candlestick; there are multiple constructors for doing so. Obtains information about the
    /// open, high, low, close, volume, and date to later display on the candlestick chart to the end user.
    /// </summary>
    internal class Candlestick 
    {
        // open, high, low, close, volume, and date variables are initialized 
        public decimal open { get; set; }

        public decimal high { get; set; }

        public decimal low { get; set; }

        public decimal close { get; set; }

        public ulong volume { get; set; }

        public DateTime date { get; set; }
        /// <summary>
        /// an addition operator for candlesticks. This is not used in project 2, but was demonstrated in class, so I have it here
        /// for potential future use. This operator creates a new candlestick whose member variables are logically created from two candlesticks
        /// For example, the open of the first candlestick is used, whichever's high is higher is used, whichever's low is lower is used etc.
        /// This should create a composite candlestick of the two candlesticks that makes sense.
        /// </summary>
        /// <param name="a"></param> the first candlestick>
        /// <param name="b"></param> the second candlestick>
        /// <returns></returns>
        public static Candlestick operator +(Candlestick a, Candlestick b) => new Candlestick(
                a.open,                     //use earlier open
                Math.Max(a.high, b.high),   //use highest high
                Math.Min(a.low, b.low),     //use lowest low
                b.close,                    //use later close
                a.volume + b.volume,        //use combined volume
                b.date);                    //use later date

        /// <summary>
        /// Default constructor for candlesticks. Makes no assignment for any of the member variables.
        /// </summary>
        public Candlestick() { }
        /// <summary>
        /// Manual constructor for candlesticks. Given all of its member variables, make a candlestick.
        /// </summary>
        /// <param name="open"></param> the provided open
        /// <param name="high"></param> the provided high
        /// <param name="low"></param> the provided low
        /// <param name="close"></param> the provided close
        /// <param name="volume"></param> the provided volume
        /// <param name="date"></param> the provided date
        public Candlestick(decimal open, decimal high, decimal low, decimal close, ulong volume, DateTime date)
        {
            this.open = open;
            this.high = high;
            this.low = low;
            this.close = close;
            this.volume = volume;
            this.date = date;
        }
        /// <summary>
        /// Candlestick copy contructor. Given a candlestick, make another one with the same member variables
        /// </summary>
        /// <param name="copy"></param> the provided candlestick that will be copied
        public Candlestick(Candlestick copy)
        {
            this.open = copy.open;
            this.high = copy.high;
            this.low = copy.low;
            this.close = copy.close;
            this.volume = copy.volume;
            this.date = copy.date;
        }
        /// <summary>
        /// The csv constructor for candlesticks. Given a line of data from a yahoo finance .csv file, make a candlestick.
        /// </summary>
        /// <param name="rowOfData"></param> the string of data from a single row in the .csv file
        public Candlestick(string rowOfData)
        {
            //specify separators to split the string of values
            char[] separators = new char[] { ',', ' ', '"' };

            //split the row of data based on the separators and store each value in an array of strings
            string[] subs = rowOfData.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //date string ends up in subs[0]
            //open string ends up in subs[1] 
            //high string ends up in subs[2] 
            //low string ends up in subs[3] 
            //close string ends up in subs[4] 
            //adjusted close string ends up in subs[5] which is not used in this program 
            //volume string ends up in subs[6] 

            string dateString = subs[0]; 
            date = DateTime.Parse(dateString); //parse the date from the string held in subs[0]

            decimal temp; // init decimal temp for returning results from TryParse
            bool success = decimal.TryParse(subs[1], out temp); //bool success tracks whether we successfully parse the open
                                                                //from the array of strings
            if (success) open = temp; //if open was parsed successfully, assign the value from the parse to the 'open' decimal

            success = decimal.TryParse(subs[2], out temp); //bool success tracks whether we successfully parse the high from the
                                                           //array of strings
            if (success) high = temp; //if high was parsed successfully, assign the value from the parse to the 'high' decimal

            success = decimal.TryParse(subs[3], out temp); //bool success tracks whether we successfully parse the low from the
                                                           //array of strings
            if (success) low = temp; //if low was parsed successfully, assign the value from the parse to the 'low' decimal

            success = decimal.TryParse(subs[4], out temp); //bool success tracks whether we successfully parse the close from the
                                                           //array of strings
            if (success) close = temp; //if close was parsed successfully, assign the value from the parse to the 'close' decimal

            ulong tempVolume; //init ulong temp for returning results from TryParse for the DATE
            success = ulong.TryParse(subs[6], out tempVolume); //bool success tracks whether we successfully parse the volume
                                                               //from the array of strings
            if (success) volume = tempVolume; //if volume was parsed successfully, assign the value from the parse to the 'volume'
                                              //ulong
        }
    }
}
