using System.Threading.Tasks;
using L0gg3r;
using L0gg3r.Base;

namespace LoggerBuilderTests.PropertyTests.LogSinksProperyTests;

internal class LogSink : ILogSink
{
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public ValueTask<bool> SubmitAsync(in LogMessage logMessage) => ValueTask.FromResult(true);
}

[TestClass]
public class TheLogSinksProperty
{
    [TestMethod]
    public void ShouldBeEmptyAtDefault()
    {
        // Arrange / Act
        LoggerBuilder loggerBuilder = new();

        // Assert
        loggerBuilder.LogSinks.Should().BeEmpty();
    }

    [TestMethod]
    public void ShouldListAllLogSinks()
    {
        // Arrange
        LogSink logSink1 = new();
        LogSink logSink2 = new();
        LoggerBuilder loggerBuilder = new();
        loggerBuilder.LogTo.LogSink(logSink1).LogTo.LogSink(logSink2);

        // Act
        Logger logger = loggerBuilder.Build();

        // Assert
        logger.LogSinks.Should().Contain(logSink1).And.Contain(logSink2);
    }
}
