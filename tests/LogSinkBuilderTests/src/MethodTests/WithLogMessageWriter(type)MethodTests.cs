using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L0gg3r.Base;
using L0gg3r.LogSinks.Base;
using TestLogSink;

namespace LogSinkBuilderTests.MethodTests.WithLogMessageWriter__type__MethodTests;

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
        LogSink logSink = logSinkBuilder.WithLogMessageWriter(typeof(LogMessageWriter)).Build();

        // Assert
        logSink.LogMessageWriters.First().Should().BeOfType<LogMessageWriter>();
    }

    [TestMethod]
    public void ShouldThrowArgumentNullExceptionIfInstanceIsNull()
    {
        // Arrange
        TestLogSinkBuilder logSinkBuilder = new();

        // Act
        Action action = () => logSinkBuilder.WithLogMessageWriter((Type)null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
