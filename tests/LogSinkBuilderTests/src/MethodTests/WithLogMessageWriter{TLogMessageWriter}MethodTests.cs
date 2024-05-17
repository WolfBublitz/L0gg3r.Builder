using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L0gg3r.Base;
using L0gg3r.LogSinks.Base;
using TestLogSink;

namespace LogSinkBuilderTests.MethodTests.WithLogMessageWriter__TLogMessageWriter__MethodTests;

[LogMessageWriter<LogSink>]
internal class LogMessageWriter : ILogMessageWriter<object>
{
    public ValueTask WriteAsync(in DateTimeOffset timestamp, LogLevel logLevel, IReadOnlyCollection<string> senders, object payload)
        => ValueTask.CompletedTask;
}

[TestClass]
public class WithLogMessageWriterMethod
{
    [TestMethod]
    public void ShouldCreateALogSinkWithLogMessageWriterInstance()
    {
        // Arrange
        TestLogSinkBuilder logSinkBuilder = new();

        // Act
        LogSink logSink = logSinkBuilder.WithLogMessageWriter<LogMessageWriter>().Build();

        // Assert
        logSink.LogMessageWriters.First().Should().BeOfType<LogMessageWriter>();
    }
}
