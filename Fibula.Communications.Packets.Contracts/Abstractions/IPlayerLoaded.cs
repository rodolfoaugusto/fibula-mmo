﻿// -----------------------------------------------------------------
// <copyright file="IPlayerLoaded.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Author: Jose L. Nunez de Caceres
// jlnunez89@gmail.com
// http://linkedin.com/in/jlnunez89
//
// Licensed under the MIT license.
// See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------

namespace OpenTibia.Communications.Packets.Contracts.Abstractions
{
    public interface IPlayerLoaded
    {
        uint AccountId { get; }

        string CharacterName { get; }
    }
}