using System;
using L0gg3r;

namespace LoggerBuilderTests.MethodTests.WithChildLogger__name__MethodTests;

[TestClass]
public class TheWithChildLoggerMethod
{
    [TestMethod]
    public void ShouldCreateALoggerWithWithAChildLogger()
    {
        // Arrange
        LoggerBuilder loggerBuilder = new();

        // Act
        Logger logger = loggerBuilder.WithChildLogger("ChildLogger").Build();

        // Assert
        logger.ChildLoggers.Should().ContainKey("ChildLogger");
    }

    [TestMethod]
    public void ShouldThrowArgumentNullExceptionIfNameIsNull()
    {
        // Arrange
        LoggerBuilder loggerBuilder = new();

        // Act
        Action action = () => loggerBuilder.WithChildLogger(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
