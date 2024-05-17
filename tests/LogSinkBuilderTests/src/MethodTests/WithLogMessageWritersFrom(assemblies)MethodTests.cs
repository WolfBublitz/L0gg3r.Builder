using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestLogSink;

namespace LogSinkBuilderTests.MethodTests.WithLogMessageWritersFrom__assemblies__MethodTests;


[TestClass]
public class WithLogMessageWriterMethod
{
    public class TestAssembly1DataAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            yield return new object[]
            {
                new[]
                {
                    Assembly.Load("TestAssembly1")
                },
                new[]
                {
                    typeof(TestAssembly1.LogMessageWriter)
                }
            };
        }

        public string? GetDisplayName(MethodInfo methodInfo, object?[]? data) => "TestAssembly1";
    }

    public class TestAssembly2DataAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            yield return new object[]
            {
                new[]
                {
                    Assembly.Load("TestAssembly2")
                },
                new[]
                {
                    typeof(TestAssembly2.LogMessageWriter)
                }
            };
        }

        public string? GetDisplayName(MethodInfo methodInfo, object?[]? data) => "TestAssembly2";
    }

    public class TestAssembly1And2DataAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            yield return new object[]
            {
                new[]
                {
                    Assembly.Load("TestAssembly1"),
                    Assembly.Load("TestAssembly2")
                },
                new[]
                {
                    typeof(TestAssembly1.LogMessageWriter),
                    typeof(TestAssembly2.LogMessageWriter)
                }
            };
        }

        public string? GetDisplayName(MethodInfo methodInfo, object?[]? data) => "TestAssembly2";
    }

    [DataTestMethod]
    [TestAssembly1Data]
    [TestAssembly2Data]
    [TestAssembly1And2Data]
    public void ShouldCreateALogSinkWithLogMessageWriterInstance(Assembly[] assemblies, Type[] logMessageTypes)
    {
        // Arrange
        TestLogSinkBuilder logSinkBuilder = new();

        // Act
        LogSink logSink = logSinkBuilder.WithLogMessageWritersFrom(assemblies).Build();

        // Assert
        logSink.LogMessageWriters.Select(lmw => lmw.GetType()).Should().BeEquivalentTo(logMessageTypes);
    }
}
