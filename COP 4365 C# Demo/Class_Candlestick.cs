using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COP_4365_C__Demo
{
    /// <summary>
    /// Creates a candlestick from a line of data from the read .csv file. Splits the string of data from the file and parses
    /// open, high, low, close, volume, and date out of it to display on the candlestick chart to the end user.
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
