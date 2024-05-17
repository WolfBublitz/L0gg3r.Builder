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
    /// <seealso cref="WithLogMessageWriter(Type, bool)"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter<TLogMessageWriter>()
        where TLogMessageWriter : new()
        => WithLogMessageWriter(typeof(TLogMessageWriter), false);

    /// <summary>
    /// Registers a log message writer of type <typeparamref name="TLogMessageWriter"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful to register a single log message writer.
    /// </remarks>
    /// <typeparam name="TLogMessageWriter">The <see cref="Type"/> of the log message writer.</typeparam>
    /// <param name="replace">A value indicating whether to replace the existing log message writer (<c>true</c>) or not (<c>false</c>).</param>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="WithLogMessageWriter(Type)"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter<TLogMessageWriter>(bool replace)
        where TLogMessageWriter : new()
        => WithLogMessageWriter(typeof(TLogMessageWriter), replace);

    /// <summary>
    /// Registers a log message writer of type <paramref name="logMessageWriterType"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful to register a single log message writer.
    /// </remarks>
    /// <param name="logMessageWriterType">The <see cref="Type"/> of the log message writer.</param>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="WithLogMessageWriter{TLogMessageWriter}(bool)"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter(Type logMessageWriterType)
        => WithLogMessageWriter(logMessageWriterType, false);

    /// <summary>
    /// Registers a log message writer of type <paramref name="logMessageWriterType"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful to register a single log message writer.
    /// </remarks>
    /// <param name="logMessageWriterType">The <see cref="Type"/> of the log message writer.</param>
    /// <param name="replace">A value indicating whether to replace the existing log message writer (<c>true</c>) or not (<c>false</c>).</param>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="WithLogMessageWriter{TLogMessageWriter}()"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter(Type logMessageWriterType, bool replace)
    {
        ArgumentNullException.ThrowIfNull(logMessageWriterType);

        return WithLogMessageWriter(() => Activator.CreateInstance(logMessageWriterType)!, replace);
    }

    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter(Func<object> factory)
        => WithLogMessageWriter(factory, false);

    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter(Func<object> factory, bool replace)
    {
        ArgumentNullException.ThrowIfNull(factory);

        object instance = factory();

        return WithLogMessageWriter(instance, replace);
    }

    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter(object instance)
        => WithLogMessageWriter(instance, false);

    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWriter(object instance, bool replace)
    {
        ArgumentNullException.ThrowIfNull(instance);

        WithModification(logSink =>
        {
            if (logSink is IHasLogMessageWriters hasLogMessageWriters)
            {
                hasLogMessageWriters.RegisterLogMessageWriter(instance, replace);
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
    /// <seealso cref="WithLogMessageWritersFrom(Assembly[], bool)"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWritersFrom(Assembly[] assemblies)
        => WithLogMessageWritersFrom(assemblies, false);

    /// <summary>
    /// Registers all log message writers in the specified <paramref name="assemblies"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful when all log message writers from different <paramref name="assemblies"/>.
    /// </remarks>
    /// <param name="assemblies">The list of <see cref="Assembly"/>.</param>
    /// <param name="replace">A value indicating whether to replace the existing log message writer (<c>true</c>) or not (<c>false</c>).</param>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="WithLogMessageWritersFrom(Assembly[])"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWritersFrom(Assembly[] assemblies, bool replace)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        foreach (Assembly assembly in assemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttribute<LogMessageWriterAttribute<TLogSink>>() is not null)
                {
                    WithLogMessageWriter(type, replace);
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
    /// <seealso cref="WithLogMessageWritersFromAllAssemblies(bool)"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWritersFromAllAssemblies()
        => WithLogMessageWritersFromAllAssemblies(false);

    /// <summary>
    /// Registers all log message writers in the current <see cref="AppDomain"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful when all log message writers in the whole <see cref="AppDomain"/> should
    /// be registered.
    /// </remarks>
    /// <param name="replace">A value indicating whether to replace the existing log message writer (<c>true</c>) or not (<c>false</c>).</param>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="AppDomain.CurrentDomain"/>
    /// <seealso cref="WithLogMessageWritersFromAllAssemblies()"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWritersFromAllAssemblies(bool replace)
        => WithLogMessageWritersFrom(AppDomain.CurrentDomain.GetAssemblies(), replace);

    /// <summary>
    /// Registers all log message writers from the calling <see cref="Assembly"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful when the log message writers are in the same assembly as the
    /// calling code.
    /// </remarks>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="Assembly.GetCallingAssembly"/>
    /// <seealso cref="WithLogMessageWritersFromCallingAssembly(bool)"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWritersFromCallingAssembly()
        => WithLogMessageWritersFrom([Assembly.GetCallingAssembly()], false);

    /// <summary>
    /// Registers all log message writers from the calling <see cref="Assembly"/>.
    /// </summary>
    /// <remarks>
    /// This method is useful when the log message writers are in the same assembly as the
    /// calling code.
    /// </remarks>
    /// <param name="replace">A value indicating whether to replace the existing log message writer (<c>true</c>) or not (<c>false</c>).</param>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <seealso cref="Assembly.GetCallingAssembly"/>
    /// <seealso cref="WithLogMessageWritersFromCallingAssembly()"/>
    public LogSinkBuilder<TLogSink, TBuilder> WithLogMessageWritersFromCallingAssembly(bool replace)
        => WithLogMessageWritersFrom([Assembly.GetCallingAssembly()], replace);

    /// <summary>
    /// Adds the <paramref name="filter"/> to the log sink.
    /// </summary>
    /// <param name="filter">The filter to add.</param>
    /// <returns>This <see cref="LogSinkBuilder{TLogSink, TBuilder}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="filter"/> is <c>null</c>.</exception>
    public LogSinkBuilder<TLogSink, TBuilder> WithFilter(Predicate<LogMessage> filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        return WithModification(logSink =>
        {
            if (logSink is IHasLogMessageFilters hasLogMessageFilters)
            {
                hasLogMessageFilters.AddFilter(filter);
            }
            else
            {
                throw new InvalidOperationException("The log sink does not implement IHasLogMessageFilters.");
            }
        });
    }
}
