using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCNAHelper
{
    public class SerializedSettings
    {
        public bool onlineMode;
        public string apiKey;

        public PointF anchorA;
        public PointF anchorB;

        public bool showMode;

        public Keys showKey;
        public Keys hideKey;
        public Keys toggleShowKey;
    }
}
