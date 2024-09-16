using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365_C__Demo
{
    /// <summary>
    /// Derived class from Recognizer to recognize the peak pattern. Adds isValley pattern to the candlestick to the immediate right of the actual peak
    /// </summary>
    internal class Class_Recognizer_Peak : Recognizer //derive from recognizer
    {
        public override bool Recognize(List<smartCandlestick> lscs, int currentIndex) //override abstract method
        {
            smartCandlestick mid_scs = lscs[currentIndex]; //get middle cs
            if (PatternAlreadyExists(mid_scs))
                return mid_scs.dictOfPatternProperties["is" + patternName]; //if so, return it
            if (currentIndex == lscs.Count() - 1 || currentIndex == 0) //if at the ends of the data set, auto return false
                return false;

            smartCandlestick right_scs = lscs[currentIndex + 1]; //get rightmost cs
            smartCandlestick left_scs = lscs[currentIndex - 1]; //get leftmost cs

            //if the left scs and right scs's top prices are above the middle scs's top price
            if ((left_scs.topPrice < mid_scs.topPrice) && (mid_scs.topPrice > right_scs.topPrice))
                return true;
            else
                return false;
        }
        /// <summary>
        /// use Recognizer constructor
        /// </summary>
        public Class_Recognizer_Peak() : base("Peak", 3) { }
    }
}
