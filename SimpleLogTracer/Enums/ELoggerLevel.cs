namespace SimpleLogTracer
{
    /// <summary>
    ///     Logger level enum
    /// </summary>
    [System.Flags]
    public enum ELoggerLevel
    {
        None = 1,
        Info = 2,
        Debug = 4,
        Warning = 8,
        Error = 16,

        All = Info | Debug | Warning | Error
    }
}
