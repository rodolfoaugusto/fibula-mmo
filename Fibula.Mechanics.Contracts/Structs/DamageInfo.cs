﻿// -----------------------------------------------------------------
// <copyright file="DamageInfo.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Author: Jose L. Nunez de Caceres
// jlnunez89@gmail.com
// http://linkedin.com/in/jlnunez89
//
// Licensed under the MIT license.
// See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Mechanics.Contracts.Structs
{
    using Fibula.Common.Contracts.Enumerations;
    using Fibula.Creatures.Contracts.Enumerations;
    using Fibula.Mechanics.Contracts.Abstractions;

    /// <summary>
    /// Struct that represents damage information.
    /// </summary>
    public struct DamageInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DamageInfo"/> struct.
        /// </summary>
        /// <param name="damageValue">The value of the damage.</param>
        /// <param name="damageDealer">Optional. The dealer of the damage, if any.</param>
        public DamageInfo(int damageValue, ICombatant damageDealer = null)
        {
            this.Damage = damageValue;
            this.Dealer = damageDealer;

            this.Effect = AnimatedEffect.XBlood;
            this.Blood = BloodType.Blood;
        }

        /// <summary>
        /// Gets the original damage dealer.
        /// </summary>
        public ICombatant Dealer { get; }

        /// <summary>
        /// Gets or sets the damage value.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets the effect of the damage.
        /// </summary>
        public AnimatedEffect Effect { get; set; }

        /// <summary>
        /// Gets or sets the blood of the damage.
        /// </summary>
        public BloodType Blood { get; set; }

        /// <summary>
        /// Gets a value indicating whether to apply blood to the environment.
        /// </summary>
        public bool ApplyBloodToEnvironment => this.Damage > 0 && this.Blood == BloodType.Blood;
    }
}
