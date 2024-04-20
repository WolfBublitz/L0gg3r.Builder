namespace LoggerBuilderTests.TheConstructorTests;

[TestClass]
public class TheConstructor
{
    [TestMethod]
    public void ShouldCreateABuilderWithoutLogSinks()
    {
        // Arrange / Act
        LoggerBuilder loggerBuilder = new();

        // Assert
        loggerBuilder.LogSinks.Should().BeEmpty();
    }
}
