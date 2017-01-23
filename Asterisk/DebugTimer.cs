using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Asterisk
{
    public static class DebugTimer
    {
        public static DateTime Time;

        public static void Init()
        {
            Time = DateTime.Now;
            Mark("started");
        }

        public static void Mark(string name)
        {
            var since = DateTime.Now - Time;

            Debug.WriteLine("{0} at {1}", name, since);
        }
    }
}