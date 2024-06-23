using log4net;

namespace WindowsService
{
    /// <summary>
    /// Provides access to logging services.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Gets the logger for monitoring purposes.
        /// </summary>
        /// <value>
        /// The monitoring logger.
        /// </value>
        public static ILog MonitoringLogger
        {
            get { return LogManager.GetLogger("MonitoringLogger"); }
        }

        /// <summary>
        /// Gets the logger for exception logging.
        /// </summary>
        /// <value>
        /// The exception logger.
        /// </value>
        public static ILog ExceptionLogger
        {
            get { return LogManager.GetLogger("ExceptionLogger"); }
        }
    }
}
