using System.Threading.Tasks;
using L0gg3r.Base;
using L0gg3r.LogSinks.Base;

namespace TestLogSink;

public class LogSink : LogSinkBase<LogSink>
{
    protected override ValueTask WriteAsync(in LogMessage logMessage)
        => ValueTask.CompletedTask;
}
