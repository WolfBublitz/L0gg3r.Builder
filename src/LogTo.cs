// ----------------------------------------------------------------------------
// <copyright file="LogTo.cs" company="L0gg3r">
// Copyright (c) L0gg3r Project
// </copyright>
// ----------------------------------------------------------------------------

using System;
using L0gg3r.Base;

namespace L0gg3r.Builder;

/// <summary>
/// Provides methods to configure log sinks.
/// </summary>
/// <param name="loggerBuilder">The <see cref="LoggerBuilder"/> instance.</param>
public sealed class LogTo(LoggerBuilder loggerBuilder)
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘
    private readonly LoggerBuilder loggerBuilder = loggerBuilder;

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Adds the <typeparamref name="TLogSink"/> to the logger.
    /// </summary>
    /// <typeparam name="TLogSink">The <see cref="Type"/> of the log sink.</typeparam>
    /// <param name="logSink">The log sink instance to add.</param>
    /// <returns>The <see cref="LoggerBuilder"/> instance.</returns>
    public LoggerBuilder LogSink<TLogSink>(TLogSink logSink)
        where TLogSink : ILogSink
    {
        return loggerBuilder.WithModification(logger => logger.AddLogSink(logSink));
    }

    /// <summary>
    /// Adds a new logink built by the <paramref name="logSinkBuilder"/> to the logger.
    /// </summary>
    /// <typeparam name="TLogSink">The <see cref="Type"/> of the log sink.</typeparam>
    /// <typeparam name="TBuilder">The <see cref="Type"/> of the builder.</typeparam>
    /// <param name="logSinkBuilder">The log sink builder.</param>
    /// <returns>The <see cref="LoggerBuilder"/>.</returns>
    public LoggerBuilder LogSink<TLogSink, TBuilder>(LogSinkBuilder<TLogSink, TBuilder> logSinkBuilder)
        where TLogSink : ILogSink, new()
        where TBuilder : LogSinkBuilder<TLogSink, TBuilder>
    {
        return loggerBuilder.WithModification(logger => logger.AddLogSink(logSinkBuilder.Build()));
    }
}
