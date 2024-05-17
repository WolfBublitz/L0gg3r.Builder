using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L0gg3r.Base;
using L0gg3r.LogSinks.Base;
using TestLogSink;

namespace TestAssembly2;

[LogMessageWriter<LogSink>]
public class LogMessageWriter : ILogMessageWriter<int>
{
    public ValueTask WriteAsync(in DateTimeOffset timestamp, LogLevel logLevel, IReadOnlyCollection<string> senders, int payload)
        => ValueTask.CompletedTask;
}
