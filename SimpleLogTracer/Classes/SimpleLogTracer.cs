using SimpleLogTracer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleLogTracer
{
    /// <summary>
    ///     Logger class
    /// </summary>
    public class SimpleLogTracer
    {
        private const string DEFAULT_EMPTY_MSG_LOG = "(Empty log message)";
        private const int DEFAULT_PAD_RIGHT = 10;
        private readonly StreamWriter m_streamWriter;
        private readonly string m_logFilePath;

        private ELoggerLevel m_loggerLevel;
        private uint m_logEntryNumber;

        #region Class properties
        /// <summary>
        ///     Logger level property
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when <see cref="ELoggerLevel.None"/> flag and other flags are set at the same time</exception>
        public ELoggerLevel LoggerLevel
        {
            get
            {
                return m_loggerLevel;
            }

            set
            {
                if (HasNoneAndOtherFlags(value))
                {
                    throw new ArgumentException("Incorrect logger levels: 'None' flag has been set along with other flags, this combination is not possible", "loggerLevel");
                }
                else
                {
                    m_loggerLevel = value;
                }
            }
        }

        /// <summary>
        ///     Logger date time format settings
        /// </summary>
        public LoggerDateTimeFormat LoggerDateTimeFormat
        {
            get;
        }

        /// <summary>
        ///     Gets or sets AutoFlush property
        /// </summary>
        public bool AutoFlush
        {
            get
            {
                return m_streamWriter.AutoFlush;
            }

            set
            {
                m_streamWriter.AutoFlush = value;
            }
        }

        /// <summary>
        ///     Gets the <see cref="System.Text.Encoding"/> of the logger
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return m_streamWriter.Encoding;
            }
        }

        /// <summary>
        ///     Logs level information into the log
        /// </summary>
        public bool LogLevelInEntry
        {
            get;

            set;
        }

        /// <summary>
        ///     Log entry numbering on
        /// </summary>
        public bool LogEntryNumbering
        {
            get;
        }

        /// <summary>
        ///     Log entry number
        /// </summary>
        public uint LogEntryNumber
        {
            get
            {
                return m_logEntryNumber;
            }
        }

        /// <summary>
        ///     Write log in CSV format
        /// </summary>
        public bool LogInCSVFormat
        {
            get;
        }
        #endregion

        #region Class constructors
        // Constructors
        /// <summary>
        ///     Creates a new instance using the path for the log file
        /// </summary>
        /// <param name="logFilePath">The log file path</param>
        /// <param name="append">Should append if the log file exists?</param>
        /// <param name="encoding">The <see cref="Encoding"/> used in the logger</param>
        /// <param name="logEntryNumbering">Numbers log entries</param>
        /// <param name="logInCSVFormat">Write log in CSV format</param>
        /// <param name="loggerLevel">Looger level</param>
        /// <param name="loggerDateTimeFormat">Logger date time formatting</param>
        /// <exception cref="ArgumentException">Thrown when <see cref="ELoggerLevel.None"/> flag and other flags are set at the same time</exception>
        /// <exception cref="ArgumentNullException">Thrown when log path file is incorrect</exception>
        public SimpleLogTracer(string logFilePath, bool append, Encoding encoding, bool logEntryNumbering, bool logInCSVFormat, ELoggerLevel loggerLevel = ELoggerLevel.None, LoggerDateTimeFormat loggerDateTimeFormat = null)
        {
            // Check if logger level has 'None' flag and more flags at the same time
            if (HasNoneAndOtherFlags(loggerLevel))
            {
                throw new ArgumentException("Incorrect logger levels: 'None' flag has been set along with other flags, this combination is not possible", "loggerLevel");
            }
            // Check if log file path is valid
            CheckLogFilePath(logFilePath);
            // Set log file path
            m_logFilePath = logFilePath;
            // Instantiate StreamWriter object
            m_streamWriter = new StreamWriter(logFilePath, append, encoding);
            // Save log entry numbering setting
            LogEntryNumbering = logEntryNumbering;
            // Get log entry number ready
            m_logEntryNumber = 0;
            // Set csv format setting
            LogInCSVFormat = logInCSVFormat;
            // Set logger level
            m_loggerLevel = loggerLevel;
            // Whether or not logger date time format is null, instantiate new object
            LoggerDateTimeFormat = loggerDateTimeFormat ?? new LoggerDateTimeFormat();
            
            // Set log level in entry setting as true by default
            LogLevelInEntry = true;
        }

        /// <summary>
        ///     Creates a new instance using an existing StreamWriter
        /// </summary>
        /// <param name="streamWriter">An instance of a <see cref="StreamWriter"/></param>
        /// <param name="logEntryNumbering">Numbers log entries</param>
        /// <param name="logInCSVFormat">Write log in CSV format</param>
        /// <param name="loggerLevel">Logger level</param>
        /// <param name="loggerDateTimeFormat">Logger date time formatting</param>
        /// <exception cref="ArgumentException">Thrown when <see cref="ELoggerLevel.None"/> flag and other flags are set at the same time</exception>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="StreamWriter"/> parameter is null</exception>
        public SimpleLogTracer(StreamWriter streamWriter, bool logEntryNumbering, bool logInCSVFormat, ELoggerLevel loggerLevel = ELoggerLevel.None, LoggerDateTimeFormat loggerDateTimeFormat = null)
        {
            // Check if logger level has 'None' flag and more flags at the same time
            if (HasNoneAndOtherFlags(loggerLevel))
            {
                throw new ArgumentException("Incorrect logger levels: 'None' flag has been set along with other flags, this combination is not possible", "loggerLevel");
            }
            // Set StreamWriter object
            m_streamWriter = streamWriter ?? throw new ArgumentNullException("streamWriter", "StreamWriter object cannot be null!");
            // Save log entry numbering setting
            LogEntryNumbering = logEntryNumbering;
            // Get log entry number ready
            m_logEntryNumber = 0;
            // Set csv format setting
            LogInCSVFormat = logInCSVFormat;
            // Set logger level
            m_loggerLevel = loggerLevel;
            // Whether or not logger date time format is null, instantiate new object
            LoggerDateTimeFormat = loggerDateTimeFormat ?? new LoggerDateTimeFormat();

            // Set log file path as null
            m_logFilePath = null;
            // Set log level in entry setting as true by default
            LogLevelInEntry = true;
        }

        /// <summary>
        ///     Creates a new instance with:
        ///     <list type="bullet">No appending</list>
        ///     <list type="bullet">Using <see cref="Encoding.Default"/></list>
        ///     <list type="bullet">No log entry numbering</list>
        ///     <list type="bullet">No csv format</list>
        ///     <list type="bullet">Logger level set to <see cref="ELoggerLevel.None"/></list>
        ///     <list type="bullet">Default <see cref="LoggerDateTimeFormat"/> options</list>
        /// </summary>
        /// <param name="logFilePath">The log file path</param>
        /// <exception cref="ArgumentNullException">Thrown when log path file is incorrect</exception>
        public SimpleLogTracer(string logFilePath) : this(logFilePath, false, Encoding.Default, false, false)
        {

        }

        /// <summary>
        ///     Creates a new instance with:
        ///     <list type="bullet">Using <see cref="Encoding.Default"/></list>
        ///     <list type="bullet">No log entry numbering</list>
        ///     <list type="bullet">No csv format</list>
        ///     <list type="bullet">Logger level set to <see cref="ELoggerLevel.None"/></list>
        ///     <list type="bullet">Default <see cref="LoggerDateTimeFormat"/> options</list>
        /// </summary>
        /// <param name="logFilePath">The log file path</param>
        /// <param name="append">Should append if the log file exists?</param>
        /// <exception cref="ArgumentNullException">Thrown when log path file is incorrect</exception>
        public SimpleLogTracer(string logFilePath, bool append) : this(logFilePath, append, Encoding.Default, false, false)
        {

        }
        #endregion

        #region Class methods
        /// <summary>
        ///     Write line into the log
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="logMessage">The message to log</param>
        /// <exception cref="ArgumentException">Thrown when <see cref="ELoggerLevel.All"/> flag is set as <paramref name="logLevel"/></exception>
        /// <exception cref="ArgumentException">Thrown when more than one flag is set within <paramref name="logLevel"/></exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="logMessage"/> is null</exception>
        public void WriteLine(ELoggerLevel logLevel, string logMessage)
        {
            // If logger level is set to 'None', no writing is done
            if (m_loggerLevel.HasFlag(ELoggerLevel.None) || logLevel.HasFlag(ELoggerLevel.None))
                return;

            // 'All' level cannot be set to write into the log
            if (logLevel.HasFlag(ELoggerLevel.All))
                throw new ArgumentException("'All' flag is set within the parameter method and this level cannot be set to write into the log", "logLevel");

            // Check if one and only one level is fed into the method
            if (LevelsList(logLevel).Count > 1)
                throw new ArgumentException("More than one flag has been set within the method parameter and this is not possible when writing to the log", "logLevel");

            // Check if log message is null
            if (logMessage == null)
                throw new ArgumentNullException("logMessage", "Log message is null");

            // If log comes in empty or white space set default message
            if (logMessage == string.Empty || string.IsNullOrWhiteSpace(logMessage))
                logMessage = DEFAULT_EMPTY_MSG_LOG;

            // If logger level is set to 'All', no more checking is needed to write into the log
            // Finally, if logger level has the in parameter flag then write into the log
            if (m_loggerLevel.HasFlag(ELoggerLevel.All) || m_loggerLevel.HasFlag(logLevel))
            {
                // Write log
                m_streamWriter.WriteLine(FormatLogEntry(logLevel, logMessage));
            }
        }

        /// <summary>
        ///     Flush the logger buffer into the log file
        /// </summary>
        public void Flush()
        {
            m_streamWriter.Flush();
        }

        /// <summary>
        ///     Overridable method for derived classes so they can get basic event notifications. Is up to the user to implement this method on derived classes
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">See <see cref="EventArgs"/></param>
        public virtual void OnEvent(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     Overridable method for derived classes with generic type so they can get event notifications. Is up to the user to implement this method on derived classes
        /// </summary>
        /// <typeparam name="TEventArgs">Type of event arguments</typeparam>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">Event arguments</param>
        public virtual void OnEvent<TEventArgs>(object sender, TEventArgs e)
        {
            
        }

        /// <summary>
        ///     Releases all resources used by this instance
        /// </summary>
        public void Dispose()
        {
            m_streamWriter.Dispose();
        }

        /// <summary>
        ///     Releases all resources used by this instance and deletes log file (only if used constructor had the log file path option)
        /// </summary>
        /// <param name="deleteLogFile">Should delete log file?</param>
        public void Dispose(bool deleteLogFile)
        {
            Dispose();

            if (deleteLogFile && m_logFilePath != null)
            {
                File.Delete(m_logFilePath);
            }
        }

        /// <summary>
        ///     Formats log message depending on settings
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="logMessage">The message to log</param>
        /// <returns></returns>
        private string FormatLogEntry(ELoggerLevel logLevel, string logMessage)
        {
            StringBuilder l_logEntry = new StringBuilder();

            string log_date = LoggerDateTimeFormat.FormattedDateTime();
            string log_lvl = "[" + Enum.GetName(logLevel.GetType(), logLevel).ToUpper() + "]";
            string log_lvl_struct = LogLevelInEntry ? (log_lvl + " ").PadRight(DEFAULT_PAD_RIGHT, '=') + "===> " : string.Empty;
            string log_entry_num = LogEntryNumbering ? "<" + (++m_logEntryNumber).ToString() + "> " : string.Empty;

            if (LogInCSVFormat)
            {
                // Date
                l_logEntry.Append("\"");
                l_logEntry.Append(log_date);
                l_logEntry.Append("\"");
                l_logEntry.Append(",");

                // Log level
                if (LogLevelInEntry)
                {
                    l_logEntry.Append("\"");
                    l_logEntry.Append(log_lvl);
                    l_logEntry.Append("\"");
                    l_logEntry.Append(",");
                }

                // Log entry number
                if (LogEntryNumbering)
                {
                    l_logEntry.Append("\"");
                    l_logEntry.Append(m_logEntryNumber);
                    l_logEntry.Append("\"");
                    l_logEntry.Append(",");
                }

                // Log message
                l_logEntry.Append("\"");
                l_logEntry.Append(logMessage);
                l_logEntry.Append("\"");
            }
            else
            {
                //l_logEntry = log_date + " ===> " + log_lvl_struct + log_entry_num + logMessage;

                l_logEntry.Append(log_date);
                l_logEntry.Append(" ===> ");
                l_logEntry.Append(log_lvl_struct);
                l_logEntry.Append(log_entry_num);
                l_logEntry.Append(logMessage);
            }

            return l_logEntry.ToString();
        }

        /// <summary>
        ///     Checks wether the enum value has the <see cref="ELoggerLevel.None"/> flag along with other levels active at the same time
        /// </summary>
        /// <param name="loggerLevel">Enum value</param>
        /// <returns><c>true</c> if <see cref="ELoggerLevel.None"/> and more flags are active at the same time, <c>false</c> otherwise</returns>
        private bool HasNoneAndOtherFlags(ELoggerLevel loggerLevel)
        {
            if (loggerLevel.HasFlag(ELoggerLevel.None))
            {
                foreach (ELoggerLevel logLvl in Enum.GetValues(loggerLevel.GetType()))
                {
                    if (logLvl != ELoggerLevel.None && loggerLevel.HasFlag(logLvl))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Creates array with the flagged levels within the in parameter
        /// </summary>
        /// <param name="loggerLevel">Variable</param>
        /// <returns>Returns levels array flagged in <see cref="ELoggerLevel"/> <paramref name="loggerLevel"/></returns>
        private List<ELoggerLevel> LevelsList(ELoggerLevel loggerLevel)
        {
            List<ELoggerLevel> l_levels = new List<ELoggerLevel>(Enum.GetValues(loggerLevel.GetType()).Length);

            foreach (ELoggerLevel logLvl in Enum.GetValues(loggerLevel.GetType()))
            {
                if (loggerLevel.HasFlag(logLvl))
                    l_levels.Add(logLvl);
            }

            return l_levels;
        }

        /// <summary>
        ///     Checks if log file path is correct
        /// </summary>
        /// <param name="logFilePath">The log file path</param>
        /// <returns><c>true</c> if log file path is correct, <c>false</c> otherwise</returns>
        private void CheckLogFilePath(string logFilePath)
        {
            string l_path = string.Empty;
            string l_file = string.Empty;

            if (logFilePath == null)
                throw new ArgumentNullException();

            l_path = Path.GetDirectoryName(logFilePath);
            l_file = Path.GetFileName(logFilePath);

            if (l_path != string.Empty && !Directory.Exists(l_path))
                throw new DirectoryNotFoundException("Log file directory does not exist!");

            if (l_file == string.Empty)
                throw new ArgumentException("File name invalid!");
        }
        #endregion
    }
}
