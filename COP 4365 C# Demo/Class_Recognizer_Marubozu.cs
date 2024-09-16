using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365_C__Demo
{
    /// <summary>
    /// Derived class from Recognizer to recognize the marubozu pattern
    /// </summary>
    internal class Class_Recognizer_Marubozu : Recognizer //derive from recognizer
    {
        public override bool Recognize(List<smartCandlestick> lscs, int currentIndex) //override abstract function
        {
            smartCandlestick scs = lscs[currentIndex]; //take desired candlestick
            if (PatternAlreadyExists(scs)) //check if we already evaluated this
                return scs.dictOfPatternProperties["is" + patternName]; //return what we already have

            //if no data yet,
            bool r = scs.bodyRange >= (decimal)0.85 * scs.range; //Marubozu pattern
            return r;
        }
        /// <summary>
        /// use Recognizer constructor
        /// </summary>
        public Class_Recognizer_Marubozu() : base("Marubozu", 1) { }
    }
}
