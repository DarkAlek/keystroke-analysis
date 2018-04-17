using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeystrokeAnalysis.Parser
{
    public class UserData
    {
        public string timeStamp;
        public string userID;
        public string ip;
        public string browser;

        public List<long> userVector;

        public UserData() { }

        public UserData(string timeStamp, string userID, string userInput, string ip, string browser)
        {
            KeystrokesParser keystrokeParser = new KeystrokesParser(userInput);
            this.timeStamp = timeStamp;
            this.userID = userID;
            this.userVector = keystrokeParser.inputToVector();
            this.ip = ip;
            this.browser = browser;

        }
    }
}
