using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeystrokeAnalysis.Parser
{
    public class DataParser
    {
        private string[] data;

        public DataParser(string s)
        {
            char[] delimiters = new char[] { ',' };
            data = s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        public string TimeStamp()
        {
            return data[0].Substring(2, data[0].Length - 3); 
        }

        public string UserID()
        {
            return data[1].Substring(1);
        }

        public string KeystrokesData()
        {
            return data[2].Substring(2, data[2].Length - 4);
        }

        public string IP()
        {
            return data[3].Substring(2, data[3].Length - 3);
        }

        public string Browser()
        {
            int i = 1;
            string ret = data[4];
            while (4 + i < data.Length)
            {
                ret = ret + data[4 + i];
                i++;
            }
            return ret.Substring(2, ret.Length - 4);
        }
         

    }
}
