# SimpleLogTracer

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Features](#features)
  * [Log levels](#log-levels)
  * [Date formatting](#date-formatting)
  * [Log entry level information](#log-entry-level-information)
  * [Log entry numbering](#log-entry-numbering)
  * [Logging in csv format](#logging-in-csv-format)
  * [Custom methods for events](#custom-methods-for-events)
  * [Deletion of log file at disposal](#deletion-of-log-file-at-disposal)
* [Constructors](#constructors)
* [Properties](#properties)
* [Writing to the log](#writing-to-the-log)
* [Examples](#examples)
  * [Basic instantiation](#basic-instantiation)
  * [Instantiation with settings](#instantiation-with-settings)
  * [Setting properties](#setting-properties)
  * [Writing into the log](#writing-into-the-log)
  * [Free resources](#free-resources)
* [License](#license)

## General info
This is a .NET Core 3.0 project that implements a simple logger class to write log entries into a file. List of features:
* Log level entries: [None], Info, Debug, Warning, Error, [All]
* Log level combinations
* Encoding settings
* Different date formats
* Log entry level information
* Log entry numbering
* Output in .csv format
* Possibility to inherit and create custom methods to subscribe to events and logging desired information
* Option to delete log file when disposing object

## Technologies
.NET Core 3.0

Easily adaptable code to other Core/Framework versions

## Features

### Log levels
Log levels are described in the following `enum`

```c#
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
```

The `enum` is flagged so different levels can be combined together at the same time with a bitwise operator. The only exception is the `None` flag that cannot be combined with other levels (an `ArgumentException` will be thrown if an attempt is done).

Having a flagged enumeration allows to enable or disable diferent log levels at the same time, for example, you may have debug, warning or error logging in your code, you can enable just debug logging or combine debug and error levels, etc.

This property is set to `None` by default if not informed at the time of instantiating a new object but can be set later by accessing the class property.

### Date formatting
Different date formats can be set when logging and entry. Date formats are gathered in the following `enum`

```c#
public enum ELoggerDateFormat
{
    Day2Month2Year2,
    Day2Month2Year4,
    Month2Day2Year2,
    Month2Day2Year4,
    Year2Month2Day2,
    Year4Month2Day2
}
```

* DDMMYY
* DDMMYYYY
* MMDDYY
* MMDDYYYY
* YYMMDD
* YYYYMMDD

Date settings can be passed to the logger within the class `LoggerDateTimeFormat`. This class offers the possibility of customizing date and time separators (for example '-', '/'). It is not mandatory to pass this information at the time of logger instantiation, default settings will be used instead.

### Log entry level information
Log level information is logged by default in every log entry but it can be disabled if there is no need for this information to appear in the log entries. Use property `LogLevelInEntry` to enable or disable the functionality.

### Log entry numbering
Numbering can be added to log entries information. By setting this property, every record in the log will show a sequential numbering. This feature must be set at instantiation time and cannot be changed.

### Logging in csv format
Log can be written in csv format for later processing by other apps. This feature must be set at instantiation time and cannot be changed.

### Custom methods for events
Two overridable methods are provided for the user to implement in derived classes if subscribing to events is needed. This way, when the even is fired, logging will be done automatically. Find these methods as

```c#
public virtual void OnEvent(object sender, EventArgs e)
{

}

public virtual void OnEvent<TEventArgs>(object sender, TEventArgs e)
{
            
}
```

Both methods follow .NET standard patterns for events.

### Deletion of log file at disposal
An option to delete the log file can be enabled when disposing the object. Maybe the app sends the file anywhere before finishing and it is not necessary to keep it in the system. Enable flag in the public method
```c#
public void Dispose(bool deleteLogFile)
```

## Constructors
Four constructors are implemented to create a new object of the logger class

```c#
public SimpleLogTracer(string logFilePath, bool append, Encoding encoding, bool logEntryNumbering, bool logInCSVFormat, ELoggerLevel loggerLevel = ELoggerLevel.None, LoggerDateTimeFormat loggerDateTimeFormat = null)
{
            
}
```

This first one needs a valid log file path, whether or not to create or append to an xisting file, the encoding type, whether or not to number log entries and if the output has to be set in csv format. Optional parameters regarding log level and date-time format can be passed or use defaults instead.

```c#
public SimpleLogTracer(StreamWriter streamWriter, bool logEntryNumbering, bool logInCSVFormat, ELoggerLevel loggerLevel = ELoggerLevel.None, LoggerDateTimeFormat loggerDateTimeFormat = null)
{
            
}
```

A second constructor accepts a `StreamWriter` object passed in and same log numbering, csv formatting, log level and log date-time format as the first constructor.

```c#
public SimpleLogTracer(string logFilePath)
{
            
}
```

The third constructor just needs a valid log file path and will set other options to their defaults
* No appending
* `Encoding.Default`
* No log entry numbering
* No csv format
* Log level set to `None`
* Default date-time formats

```c#
public SimpleLogTracer(string logFilePath, bool append)
{
            
}
```

A fourth constructor is provided in case appending to the log file is needed, other options are the same as previous constructor.

## Properties
Get or set the logger level.

```c#
public ELoggerLevel LoggerLevel { get; set; }
```

Get `LoggerDateTimeFormat` and access class members to set date-time formatting.

```c#
public LoggerDateTimeFormat LoggerDateTimeFormat { get; }
```

Enable or disable automatic buffer clearing and writing to the log file.

```c#
public bool AutoFlush { get; set; }
```

Get enconding settings.

```c#
public Encoding Encoding { get; }
```

Enable or disable level information in log record.

```c#
public bool LogLevelInEntry { get; set; }
```

Check if log numbering is enabled.

```c#
public bool LogEntryNumbering { get; }
```

Get current log entry number (if enabled).

```c#
public uint LogEntryNumber { get; }
```

Check if csv formatting is enabled.

```c#
public bool LogInCSVFormat { get; }
```

## Writing to the log
Two principal methods are to be considered when writing into the log. The first one is the `WriteLine` method in which the level and the message to log are passed in. If the level of the logger object is the same or `ELoggerLevel.All` as the one passed to the method the message will be logged, otherwise no action will be taken.

```c#
public void WriteLine(ELoggerLevel logLevel, string logMessage)
{
            
}
```

The second method `Flush` is to be called if the `AutoFlush` property is set to `false` where this property enables writing automatically to the log. If set to `false` a call to this method is needed in order to get the data cleared off the buffer data and written into the log.

```c#
public void Flush()
{

}
```

## Examples
### Basic instantiation
Just pass in the log file to write into. Other settings get default values.

```c#
SimpleLogTracer logger = new SimpleLogTracer("log.txt");
```

### Instantiation with settings
Passing in the log file, appending, enconding, numbering and csv settings. Logger level and date-time format with default values.

```c#
SimpleLogTracer logger = new SimpleLogTracer("log.txt", true, Encoding.Default, false, false);
```

Passing in the log file, appending, enconding, numbering, csv settings and logger level. Date-time format with default values.

```c#
SimpleLogTracer logger = new SimpleLogTracer("log.txt", true, Encoding.Default, true, false, ELoggerLevel.Debug);
```

Creating a `StreamWriter` and `LoggerDateTimeFormat` objects to pass them in as constructor parameters.

```c#
StreamWriter sw = new StreamWriter("log.txt", false, Encoding.UTF8);

LoggerDateTimeFormat logDTFormat = new LoggerDateTimeFormat(ELoggerDateFormat.Month2Day2Year4, '/');

SimpleLogTracer logger = new SimpleLogTracer(sw, true, false, ELoggerLevel.Warning, logDTFormat);
```

### Setting properties
```c#
logger.LoggerLevel = ELoggerLevel.All // Change logger level

logger.AutoFlush = true; // AutoFlush property

logger.LogLevelInEntry = false // Disable logging level information

logger.LoggerDateTimeFormat.LoggerDateFormat = ELoggerDateFormat.Day2Month2Year4; // Change logger date format
```

### Writing into the log
Use the `WriteLine` method to write into the log.

```c#
logger.WriteLine(ELoggerLevel.Info, "Writing line into the log!");
```

If the corresponding level passed to the method is enabled, an entry will be recorded on the log.

### Free resources
Don't forget to free resources once you are done with the logger ;)

```c#
logger.Dispose();
```

## License
MIT License
