﻿// -----------------------------------------------------------------
// <copyright file="IMapLoader.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Map.Contracts.Abstractions
{
    using System.Collections.Generic;
    using Fibula.Common.Contracts.Structs;

    /// <summary>
    /// Common interface for map loaders.
    /// </summary>
    public interface IMapLoader
    {
        /// <summary>
        /// Gets the percentage completed loading the map [0, 100].
        /// </summary>
        byte PercentageComplete { get; }

        /// <summary>
        /// Gets a value indicating whether this loader has previously loaded the given coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <returns>True if the loader has previously loaded the given coordinates, false otherwise.</returns>
        bool HasLoaded(int x, int y, sbyte z);

        /// <summary>
        /// Attempts to load all tiles within a 3 dimensional coordinates window.
        /// </summary>
        /// <param name="fromX">The start X coordinate for the load window.</param>
        /// <param name="toX">The end X coordinate for the load window.</param>
        /// <param name="fromY">The start Y coordinate for the load window.</param>
        /// <param name="toY">The end Y coordinate for the load window.</param>
        /// <param name="fromZ">The start Z coordinate for the load window.</param>
        /// <param name="toZ">The end Z coordinate for the load window.</param>
        /// <returns>A collection of ordered pairs containing the <see cref="Location"/> and its corresponding <see cref="ITile"/>.</returns>
        IEnumerable<(Location Location, ITile Tile)> Load(int fromX, int toX, int fromY, int toY, sbyte fromZ, sbyte toZ);
    }
}
