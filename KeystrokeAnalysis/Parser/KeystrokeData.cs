using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeystrokeAnalysis.Parser
{
    public class KeystrokeData
    {
        public bool isKeyDown;
        public int keyCode;
        public long eventTime;
        public int? caretPosition;

        public KeystrokeData() { }

        public KeystrokeData(bool isKeyDown, int keyCode, long eventTime, int? caretPosition)
        {
            this.isKeyDown = isKeyDown;
            this.keyCode = keyCode;
            this.eventTime = eventTime;
            this.caretPosition = caretPosition;
        }
    }
}
