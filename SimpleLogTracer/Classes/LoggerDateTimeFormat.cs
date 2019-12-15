using SimpleLogTracer.Enums;
using System;

namespace SimpleLogTracer
{
    /// <summary>
    ///     Class to set logger date and time formatting
    /// </summary>
    public sealed class LoggerDateTimeFormat
    {
        private const ELoggerDateFormat DEFAULT_DATE_FORMAT = ELoggerDateFormat.Year4Month2Day2;
        private const char DEFAULT_DATE_SEPARATOR = '-';
        private const char DEFAULT_TIME_SEPARATOR = ':';
        private const string DEFAULT_DATE_TIME_SEPARATOR = " ";

        /// <summary>
        ///     Logger date format 
        /// </summary>
        public ELoggerDateFormat LoggerDateFormat { get; set; }

        /// <summary>
        ///     Date separator (character between year, month and day)
        /// </summary>
        public char DateSeparator { get; set; }

        /// <summary>
        ///     Time separator (character between hour, minute, second)
        /// </summary>
        public char TimeSeparator { get; set; }

        /// <summary>
        ///     String between date and time
        /// </summary>
        public string DateTimeSeparator { get; set; }

        /// <summary>
        ///     Generates a new struct having date and time formatting for the logger
        /// </summary>
        /// <param name="loggerDateFormat">The logger date format</param>
        /// <param name="dateSeparator">Character between date components (year, month, day)</param>
        /// <param name="timeSeparator">Character between time components (hour, minute, second)</param>
        /// <param name="datetimeSeparator">String between date and time</param>
        /// <exception cref="ArgumentNullException">Thrown when date time separator is null</exception>
        public LoggerDateTimeFormat(ELoggerDateFormat loggerDateFormat = DEFAULT_DATE_FORMAT, char dateSeparator = DEFAULT_DATE_SEPARATOR, char timeSeparator = DEFAULT_TIME_SEPARATOR, string datetimeSeparator = DEFAULT_DATE_TIME_SEPARATOR)
        {
            LoggerDateFormat = loggerDateFormat;
            DateSeparator = dateSeparator;
            TimeSeparator = timeSeparator;
            DateTimeSeparator = datetimeSeparator ?? throw new ArgumentNullException("datetimeSeparator", "Date time separator cannot be null!");
        }

        /// <summary>
        ///     Formats date and time
        /// </summary>
        /// <returns>A formatted date time string according to the class settings</returns>
        public string FormattedDateTime()
        {
            string l_formatted = string.Empty;

            switch (LoggerDateFormat)
            {
                case ELoggerDateFormat.Day2Month2Year2:
                    l_formatted = DateTime.Now.ToString($"dd'{DateSeparator}'MM'{DateSeparator}'yy'{DateTimeSeparator}'HH'{TimeSeparator}'mm'{TimeSeparator}'ss'.'fffff");
                    break;
                case ELoggerDateFormat.Day2Month2Year4:
                    l_formatted = DateTime.Now.ToString($"dd'{DateSeparator}'MM'{DateSeparator}'yyyy'{DateTimeSeparator}'HH'{TimeSeparator}'mm'{TimeSeparator}'ss'.'fffff");
                    break;
                case ELoggerDateFormat.Month2Day2Year2:
                    l_formatted = DateTime.Now.ToString($"MM'{DateSeparator}'dd'{DateSeparator}'yy'{DateTimeSeparator}'HH'{TimeSeparator}'mm'{TimeSeparator}'ss'.'fffff");
                    break;
                case ELoggerDateFormat.Month2Day2Year4:
                    l_formatted = DateTime.Now.ToString($"MM'{DateSeparator}'dd'{DateSeparator}'yyyy'{DateTimeSeparator}'HH'{TimeSeparator}'mm'{TimeSeparator}'ss'.'fffff");
                    break;
                case ELoggerDateFormat.Year2Month2Day2:
                    l_formatted = DateTime.Now.ToString($"yy'{DateSeparator}'MM'{DateSeparator}'dd'{DateTimeSeparator}'HH'{TimeSeparator}'mm'{TimeSeparator}'ss'.'fffff");
                    break;
                case ELoggerDateFormat.Year4Month2Day2:
                    l_formatted = DateTime.Now.ToString($"yyyy'{DateSeparator}'MM'{DateSeparator}'dd'{DateTimeSeparator}'HH'{TimeSeparator}'mm'{TimeSeparator}'ss'.'fffff");
                    break;
                default:
                    break;
            }

            return l_formatted;
        }
    }
}
