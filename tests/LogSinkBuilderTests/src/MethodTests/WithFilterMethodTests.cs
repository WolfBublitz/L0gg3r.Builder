using System;
using System.Linq;
using L0gg3r.Base;
using TestLogSink;

namespace LogSinkBuilderTests.MethodTests.WithFilterMethodTests;

[TestClass]
public class TheWithFilterMethod
{
    [TestMethod]
    public void ShouldAddAFilterToTheLogSink()
    {
        // Arrange
        TestLogSinkBuilder logSinkBuilder = new();
        Predicate<LogMessage> filter = _ => true;

        // Act
        LogSink logSink = logSinkBuilder.WithFilter(filter).Build();

        // Assert
        logSink.Filters.First().Should().BeSameAs(filter);
    }
}
