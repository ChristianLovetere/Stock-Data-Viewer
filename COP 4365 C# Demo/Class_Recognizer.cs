using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace COP_4365_C__Demo
{
    /// <summary>
    /// Abstract class for all other recognizers to pull from. Includes a few member variables, an abstract recognize function, and a universal
    /// function for telling whether or not a candlestick has the desired pattern already.
    /// </summary>
    internal abstract class Recognizer
    {
        public string patternName { get; set; } //store name of pattern
        public int patternLength { get; set; } //store num of candlesticks that make up pattern
        public abstract bool Recognize(List<smartCandlestick> lscs, int currentIndex); //abstract recognize func

        public Recognizer(string pattern, int scope) //construct with the necessary member variables
        {
            patternName = pattern;
            patternLength = scope;
        }

        public void RecognizeAll(List<smartCandlestick> lscs)
        {
            int j = 0;
            foreach (smartCandlestick scs in lscs)
            {
                if(!PatternAlreadyExists(scs))
                    scs.dictOfPatternProperties["is" + patternName] = Recognize(lscs, j); 
                j++;
            }
        }
        //given a candlestick, return whether or not it has this pattern assigned already
        public bool PatternAlreadyExists(smartCandlestick scs)
        {
            return scs.dictOfPatternProperties.TryGetValue("is" + patternName, out bool val);
        }
    }
}