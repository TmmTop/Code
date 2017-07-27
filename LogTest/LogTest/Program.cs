using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogTest
{
    class Program
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            
            try {
                logger.Info("sssss");
            }
            catch (Exception ex) {
                logger.Error(ex.Message);
            }
            
        }
    }
}
