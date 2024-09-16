using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365_C__Demo
{
    /// <summary>
    /// smartCandlestick is derived from the Candlestick class. Aside from the member variables present in a normal candlestick.
    /// the smartCandlestick has range, bodyRange, topPrice, bottomPrice, lowerTail, and upperTail, which are all arithmetically
    /// derived from the candlestick member variables. Additionally, each scs contains a dictionary of bool with string keys
    /// that represent which patterns the candlestick falls under. For example, an scs with an open > close will return true if
    /// you check isBearish, and false for isBullish.
    /// </summary>
    internal class smartCandlestick : Candlestick 
    {
        public decimal range { get; set; }
        public decimal bodyRange { get; set; }
        public decimal topPrice { get; set; }
        public decimal bottomPrice { get; set; }
        public decimal lowerTail { get; set; }
        public decimal upperTail { get; set; }

        public Dictionary<string, bool> dictOfPatternProperties { get; set; }
        /// <summary>
        /// Make a smartCandlestick from a candlestick. First assign the candlestick member variables, then initialize the
        /// dictionary, compute the smartCandlestick member variables, and finally populate/compute the dictionary entries.
        /// </summary>
        /// <param name="cs"></param> the candlestick that the smartCandlestick be based off of.
        public smartCandlestick(Candlestick cs)
        {
            this.open = cs.open;        //copy open from normal candlestick
            this.high = cs.high;        //copy high from normal candlestick
            this.low = cs.low;          //copy low from normal candlestick
            this.close = cs.close;      //copy close from normal candlestick
            this.volume = cs.volume;    //copy volume from normal candlestick
            this.date = cs.date;        //copy date from normal candlestick

            dictOfPatternProperties = new Dictionary<string, bool>(); //init dictionary 
            computeProperties();        //compute scs member vars
            computePatternProperties(); //populate and compute dict entries
        }
        /// <summary>
        /// construct a smart candlestick from a row of data directly. currently only used to populate the combobox on form 
        /// launch. Allows scs to be created from .csv file directly.
        /// </summary>
        /// <param name="rowOfData"></param>
        public smartCandlestick(string rowOfData) : base(rowOfData)
        {
            dictOfPatternProperties = new Dictionary<string, bool>(); //init dict
            computeProperties(); //compute scs member vars
            computePatternProperties(); //populate and compute dict entries
        }
        /// <summary>
        /// This function assigns decimal values to the member variables for the smartCandlestick using arithmetic between the
        /// member variables from the normal candlestick.
        /// </summary>
        void computeProperties()
        {
            range = high - low;                     //range of the entire cs
            bodyRange = Math.Abs(open - close);     //range of the body
            topPrice = Math.Max(open, close);       //equal to either the open or close, whichever is higher
            bottomPrice = Math.Min(open, close);    //equal to either the open or close, whichever is lower
            lowerTail = bottomPrice - low;          //the range from the bottom price to the low
            upperTail = high - topPrice;            //the range from the top price to the high

        }
        /// <summary>
        /// populates the existing dictionary with 8 candlestick patterns. The key to the dictionary is the word 'is' followed
        /// by the pattern in question. The value is a simple boolean based on whether or not the candlestick matches that
        /// property. Some patterns are mutually exclusive because of how they are computed.
        /// </summary>
        void computePatternProperties()
        {
            dictOfPatternProperties.Add("isBearish", open > close);     //mutually exclusive with isBullish and isNeutral.
            dictOfPatternProperties.Add("isBullish", open < close);     //mutually exclusive with isBearish and isNeutral.
            dictOfPatternProperties.Add("isNeutral", open == close);    //mutually exclusive with isBearish and isBullish.
            //candlestick is marubozu when the bodyrange is the vast majority of the range.
            dictOfPatternProperties.Add("isMarubozu", (bodyRange >= (decimal)0.85 * range));
            //candlestick is hammer when it has a very long upper or lower tail
            dictOfPatternProperties.Add("isHammer", (lowerTail >= (decimal)0.65 * range) || (upperTail >= (decimal)0.65 * range));
            //candlestick is doji when the body range is very small
            dictOfPatternProperties.Add("isDoji", (bodyRange <= (decimal)0.15 * range));
            //candlestick is dragonfly doji when it is a hammer with a high hammerhead AND a doji.
            dictOfPatternProperties.Add("isDragonflyDoji", (bodyRange <= (decimal)0.15 * range && lowerTail >= (decimal)0.65 * range));
            //candlestick is gravestone doji when it is a hammer with a low hammerhead AND a doji.
            dictOfPatternProperties.Add("isGravestoneDoji", (bodyRange <= (decimal)0.15 * range && upperTail >= (decimal)0.65 * range));
        }
    }
}
