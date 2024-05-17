using System.Linq;
using TestLogSink;

namespace LogSinkBuilderTests.MethodTests.WithLogMessageWritersFromAllAssembliesMethodTests;

[TestClass]
public class WithLogMessageWriterFromAllAssembliesMethod
{
    [TestMethod]
    [Ignore]
    public void ShouldCreateALogSinkWithLogMessageWritersFromAllAssembliesInstance()
    {
        // Arrange
        TestLogSinkBuilder logSinkBuilder = new();

        // Act
        LogSink logSink = logSinkBuilder.WithLogMessageWritersFromAllAssemblies().Build();

        // Assert
        logSink.LogMessageWriters.Should().Contain(lmw => lmw.GetType() == typeof(TestAssembly1.LogMessageWriter));
        logSink.LogMessageWriters.Should().Contain(lmw => lmw.GetType() == typeof(TestAssembly2.LogMessageWriter));

    }
}
