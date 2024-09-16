using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365_C__Demo
{
    /// <summary>
    /// Derived class from Recognizer to recognize the bearish harami pattern
    /// </summary>
    internal class Class_Recognizer_BearishHarami : Recognizer //derive from recognize
    {
        public override bool Recognize(List<smartCandlestick> lscs, int currentIndex) //ovveride abstract function
        {
            smartCandlestick scs = lscs[currentIndex]; //take desired candlestick
            if (PatternAlreadyExists(scs)) //check if we already evaluated this
                return scs.dictOfPatternProperties["is" + patternName]; //return what we already have
            if (currentIndex == 0) //if at the first candlestick,
                return false;
            //if no data yet,

            smartCandlestick prev_scs = lscs[currentIndex - 1]; //get prev cs

            //if the right scs starts higher and ends lower than the left, and the left is bullish, and the right is bearish
            if (prev_scs.topPrice > scs.topPrice && prev_scs.bottomPrice < scs.bottomPrice && prev_scs.open < prev_scs.close && scs.open > scs.close)
                return true;
            else
                return false;

        }
        /// <summary>
        /// use Recognizer constructor
        /// </summary>
        public Class_Recognizer_BearishHarami() : base("BearishHarami", 2) { }
    }
}
