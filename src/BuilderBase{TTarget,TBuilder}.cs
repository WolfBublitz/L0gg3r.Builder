// ----------------------------------------------------------------------------
// <copyright file="BuilderBase{TTarget,TBuilder}.cs" company="L0gg3r">
// Copyright (c) L0gg3r Project
// </copyright>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace L0gg3r.Builder;

/// <summary>
/// An abstract base class for a builder.
/// </summary>
/// <typeparam name="TTarget">The <see cref="Type"/> of the target that this builder shall build.</typeparam>
/// <typeparam name="TBuilder">The <see cref="Type"/> of the builder itself.</typeparam>
public abstract class BuilderBase<TTarget, TBuilder>
    where TTarget : new()
    where TBuilder : BuilderBase<TTarget, TBuilder>
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘
    private readonly List<Action<TTarget>> modifications = [];

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Adds a modification to the builder.
    /// </summary>
    /// <remarks>
    /// A modification is an action that modifies the target object.
    /// </remarks>
    /// <param name="modification">The modification to add.</param>
    /// <returns>This <see cref="BuilderBase{TTarget, TBuilder}"/> instance.</returns>
    public TBuilder WithModification(Action<TTarget> modification)
    {
        modifications.Add(modification);

        return (TBuilder)this;
    }

    /// <summary>
    /// Creates and returns a new instance of <typeparamref name="TTarget"/>.
    /// </summary>
    /// <returns>The newly created <typeparamref name="TTarget"/>.</returns>
    public TTarget Build()
    {
        TTarget target = new();

        foreach (var modification in modifications)
        {
            modification(target);
        }

        return target;
    }
}
