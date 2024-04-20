// ----------------------------------------------------------------------------
// <copyright file="LogSinkBuilder{TLogSink,TBuilder}.cs" company="L0gg3r">
// Copyright (c) L0gg3r Project
// </copyright>
// ----------------------------------------------------------------------------

using System;
using System.Reflection;

using L0gg3r.Base;
using L0gg3r.LogSinks.Base;

namespace L0gg3r.Builder;

/// <summary>
/// A builder for creating <see cref="ILogSink"/> instances.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="LogSinkBuilder{TLogSink, TBuilder}"/> class.
/// </remarks>
/// <typeparam name="TLogSink">The <see cref="Type"/> of the log sink.</typeparam>
/// <typeparam name="TBuilder">The <see cref="Type"/> of the builder.</typeparam>
public class LogSinkBuilder<TLogSink, TBuilder> : BuilderBase<TLogSink, TBuilder>
    where TLogSink : ILogSink, new()
    where TBuilder : LogSinkBuilder<TLogSink, TBuilder>
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Registers a log message writer of type <typeparamref name="TLogMessageWriter"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful to register a single log message writer.
    /// </remarks>
    /// <typeparam name="TLogMessageWriter">The <see cref="Type"/> of the log message writer.</typeparam>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="WithLogMessageWriter(Type)"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter<TLogMessageWriter>()
        => WithLogMessageWriter(typeof(TLogMessageWriter));

    /// <summary>
    /// Registers a log message writer of type <paramref name="logMessageWriterType"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful to register a single log message writer.
    /// </remarks>
    /// <param name="logMessageWriterType">The <see cref="Type"/> of the log message writer.</param>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="WithLogMessageWriter{TLogMessageWriter}()"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter(Type logMessageWriterType)
        => WithLogMessageWriter(() => Activator.CreateInstance(logMessageWriterType)!);

    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter(Func<object> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        object instance = factory();

        return WithLogMessageWriter(instance);
    }

    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter(object instance)
    {
        WithModification(logSink =>
        {
            if (logSink is IHasLogMessageWriters hasLogMessageWriters)
            {
                hasLogMessageWriters.RegisterLogMessageWriter(instance, replace: false);
            }
        });

        return this;
    }

    /// <summary>
    /// Registers all log message writers in the specified <paramref name="assemblies"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful when all log message writers from different <paramref name="assemblies"/>.
    /// </remarks>
    /// <param name="assemblies">The list of <see cref="Assembly"/>.</param>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWritersFrom(params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        foreach (Assembly assembly in assemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttribute<LogMessageWriterAttribute<TLogSink>>() is not null)
                {
                    WithLogMessageWriter(type);
                }
            }
        }

        return this;
    }

    /// <summary>
    /// Registers all log message writers in the current <see cref="AppDomain"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful when all log message writers in the whole <see cref="AppDomain"/> should
    /// be registered.
    /// </remarks>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="AppDomain.CurrentDomain"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWritersFromAllAssemblies()
        => WithLogMessageWritersFrom(AppDomain.CurrentDomain.GetAssemblies());

    /// <summary>
    /// Registers all log message writers from the calling <see cref="Assembly"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful when the log message writers are in the same assembly as the
    /// calling code.
    /// </remarks>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="Assembly.GetCallingAssembly"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWritersFromCallingAssembly()
        => WithLogMessageWritersFrom(Assembly.GetCallingAssembly());
}
