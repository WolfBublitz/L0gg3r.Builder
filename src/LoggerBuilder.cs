// ----------------------------------------------------------------------------
// <copyright file="LoggerBuilder.cs" company="L0gg3r">
// Copyright (c) L0gg3r Project
// </copyright>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

using L0gg3r.Base;

namespace L0gg3r.Builder;

/// <summary>
/// A builder for creating <see cref="Logger"/> instances.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="LoggerBuilder"/> class.
/// </remarks>
public sealed class LoggerBuilder : BuilderBase<Logger, LoggerBuilder>
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Constructors                                                            │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggerBuilder"/> class.
    /// </summary>
    public LoggerBuilder()
    {
        string loggerName = Assembly.GetCallingAssembly().GetName().Name ?? "Logger";

        WithModification(logger => logger.Name = loggerName);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggerBuilder"/> class.
    /// </summary>
    /// <param name="name">The name of the <see cref="Logger"/>.</param>
    public LoggerBuilder(string name)
    {
        WithModification(logger => logger.Name = name);
    }

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                              │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Gets a <see cref="LogTo"/>.
    /// </summary>
    public LogTo LogTo => new(this);

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Internal Properties                                                            │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Gets the collection of <see cref="ILogSink"/>s.
    /// </summary>
    internal ICollection<ILogSink> LogSinks { get; } = [];

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Creates a <see cref="Logger"/> with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="Logger"/>.</param>
    /// <returns>The <see cref="LoggerBuilder"/> instance.</returns>
    public LoggerBuilder WithName(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        return WithModification(logger => logger.Name = name);
    }

    /// <summary>
    /// Creates a <see cref="Logger"/> with the specified <paramref name="minimumLogLevel"/>.
    /// </summary>
    /// <param name="minimumLogLevel">The minium <see cref="LogLevel"/>.</param>
    /// <returns>This <see cref="LoggerBuilder"/> instance.</returns>
    public LoggerBuilder WithMinimumLogLevel(LogLevel minimumLogLevel)
    {
        return WithModification(logger => logger.MinimumLogLevel = minimumLogLevel);
    }

    /// <summary>
    /// Creates a <see cref="Logger"/> with the specified <paramref name="filter"/>.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <returns>This <see cref="LoggerBuilder"/> instance.</returns>
    public LoggerBuilder WithFilter(Predicate<LogMessage> filter)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));

        return WithModification(logger => logger.AddFilter(filter));
    }

    /// <summary>
    /// Creates a <see cref="Logger"/> with a child logger named <paramref name="childLoggerName"/>.
    /// </summary>
    /// <param name="childLoggerName">The name of the child logger.</param>
    /// <returns>This <see cref="LoggerBuilder"/> instance.</returns>
    public LoggerBuilder WithChildLogger(string childLoggerName)
    {
        ArgumentNullException.ThrowIfNull(childLoggerName, nameof(childLoggerName));

        return WithModification(logger =>
        {
            Logger childLogger = new LoggerBuilder().WithName(childLoggerName).Build();

            logger.AddChildLogger(childLogger);
        });
    }

    /// <summary>
    /// Creates a <see cref="Logger"/> with a child logger named <paramref name="childLoggerName"/>.
    /// </summary>
    /// <param name="childLoggerName">The name of the child logger.</param>
    /// <param name="childLoggerBuilderAction">An action providing access to the <see cref="LoggerBuilder"/> of the child logger.</param>
    /// <returns>This <see cref="LoggerBuilder"/> instance.</returns>
    public LoggerBuilder WithChildLogger(string childLoggerName, Action<LoggerBuilder> childLoggerBuilderAction)
    {
        ArgumentNullException.ThrowIfNull(childLoggerName, nameof(childLoggerName));
        ArgumentNullException.ThrowIfNull(childLoggerBuilderAction, nameof(childLoggerBuilderAction));

        LoggerBuilder childloggerBuilder = new LoggerBuilder().WithName(childLoggerName);

        childLoggerBuilderAction(childloggerBuilder);

        return WithModification(logger =>
        {
            Logger childLogger = childloggerBuilder.Build();

            logger.AddChildLogger(childLogger);
        });
    }
}
