using System;

namespace SimpleLogTracer.Enums
{
    /// <summary>
    ///     Logger level enum
    /// </summary>
    [Flags]
    public enum ELoggerLevel
    {
        None = 1,
        Info = 2,
        Debug = 4,
        Warning = 8,
        Error = 16,

        All = Info | Debug | Warning | Error
    }

    /// <summary>
    ///     Logger date format
    /// </summary>
    public enum ELoggerDateFormat
    {
        Day2Month2Year2,
        Day2Month2Year4,
        Month2Day2Year2,
        Month2Day2Year4,
        Year4Month2Day2,
        Year2Month2Day2
    }
}
