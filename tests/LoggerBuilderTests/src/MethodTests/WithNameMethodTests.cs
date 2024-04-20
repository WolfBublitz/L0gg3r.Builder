using System;
using L0gg3r;

namespace LoggerBuilderTests.MethodTests.WithNameMethodTests;

[TestClass]
public class TheWithNameMethod
{
    [TestMethod]
    public void ShouldCreateALoggerWithAGivenName()
    {
        // Arrange
        LoggerBuilder loggerBuilder = new();

        // Act
        Logger logger = loggerBuilder.WithName("TestLogger").Build();

        // Assert
        logger.Name.Should().Be("TestLogger");
    }

    [TestMethod]
    public void ShouldThrowArgumentNullExceptionIfNameIsNull()
    {
        // Arrange
        LoggerBuilder loggerBuilder = new();

        // Act
        Action action = () => loggerBuilder.WithName(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
