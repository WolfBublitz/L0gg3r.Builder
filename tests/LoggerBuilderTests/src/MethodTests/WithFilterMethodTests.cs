using System;
using L0gg3r;
using L0gg3r.Base;

namespace LoggerBuilderTests.MethodTests.WithFilterMethodTests;

[TestClass]
public class TheWithFilterMethod
{
    [TestMethod]
    public void ShouldCreateALoggerWithAGivenFilter()
    {
        // Arrange
        LoggerBuilder loggerBuilder = new();
        static bool filter(LogMessage _) => true;

        // Act
        Logger logger = loggerBuilder.WithFilter(filter).Build();

        // Assert
        logger.Filters.Should().Contain(filter);
    }

    [TestMethod]
    public void ShouldThrowArgumentNullExceptionIfNameIsNull()
    {
        // Arrange
        LoggerBuilder loggerBuilder = new();

        // Act
        Action action = () => loggerBuilder.WithFilter(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
