using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L0gg3r.Base;
using L0gg3r.LogSinks.Base;
using TestLogSink;

namespace TestAssembly1;

[LogMessageWriter<LogSink>]
public class LogMessageWriter : ILogMessageWriter<object>
{
    public ValueTask WriteAsync(in DateTimeOffset timestamp, LogLevel logLevel, IReadOnlyCollection<string> senders, object payload)
        => ValueTask.CompletedTask;
}
