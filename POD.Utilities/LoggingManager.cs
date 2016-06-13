using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POD.Utilities
{
    public class LoggingManager
    {
        public Logger Log { get; private set; }
        public LoggingManager()
        {
            LogManager.Flush();
            Log = LogManager.GetCurrentClassLogger();
        }
    }
}
