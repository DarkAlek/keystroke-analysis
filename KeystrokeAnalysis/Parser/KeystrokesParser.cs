using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeystrokeAnalysis.Parser
{
    public class KeystrokesParser
    {
        private string[] keystrokes;

        public KeystrokesParser(string userInput)
        {
            char[] delimiters = new char[] { '\'', ' '};
            keystrokes = userInput.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        public List<long> inputToVector()
        {
            //List<KeystrokeData> keystrokesList = new List<KeystrokeData>();
            //foreach (var el in keystrokes)
            //{
            //    char[] delimiters = new char[] { '_' };
            //    string[] keystrokeData = el.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            //    keystrokesList.Add(new KeystrokeData(keystrokeData[0] == "d" ? true : false,
            //                                         int.Parse(keystrokeData[1]),
            //                                         long.Parse(keystrokeData[2]),
            //                                         keystrokeData[0] == "d" ? int.Parse(keystrokeData[3]) : (int?)null));
            //}

            List<long> keystrokesList = new List<long>();
            for(int i = 0; i < keystrokes.Length - 1; ++i)
            {
                char[] delimiters = new char[] { '_' };
                string[] keystrokeData1 = keystrokes[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                string[] keystrokeData2 = keystrokes[i+1].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (keystrokeData1[1].Equals("16") || keystrokeData2[1].Equals("16")) continue;

                keystrokesList.Add(Math.Abs(int.Parse(keystrokeData1[2]) - int.Parse(keystrokeData2[2])));
            }

            return keystrokesList;
        }
    }
}
