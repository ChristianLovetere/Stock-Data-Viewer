using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP_4365_C__Demo
{
    /// <summary>
    /// Derived class from Recognizer to recognize the neutral pattern
    /// </summary>
    internal class Class_Recognizer_Neutral : Recognizer //derive from recognizer
    {
        public override bool Recognize(List<smartCandlestick> lscs, int currentIndex) //override abstract function
        {
            smartCandlestick scs = lscs[currentIndex]; //take desired candlestick
            if (PatternAlreadyExists(scs)) //check if we already evaluated this
                return scs.dictOfPatternProperties["is" + patternName]; //return what we already have

            //if no data yet,
            bool r = scs.open == scs.close; //neutral test
            return r;
        }
        /// <summary>
        /// use Recognizer constructor
        /// </summary>
        public Class_Recognizer_Neutral() : base("Neutral", 1) { }
    }
}
