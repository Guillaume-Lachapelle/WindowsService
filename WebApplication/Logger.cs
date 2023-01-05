﻿using log4net;

namespace WebApplication
{
    public static class Logger
    {
        public static ILog MonitoringLogger
        {
            get { return LogManager.GetLogger("MonitoringLogger"); }
        }

        public static ILog ExceptionLogger
        {
            get { return LogManager.GetLogger("ExceptionLogger"); }
        }
    }
}
