﻿// -----------------------------------------------------------------
// <copyright file="Condition.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Mechanics.Conditions
{
    using System;
    using Fibula.Common.Contracts.Abstractions;
    using Fibula.Common.Contracts.Enumerations;
    using Fibula.Common.Utilities;
    using Fibula.Mechanics.Contracts.Abstractions;
    using Fibula.Mechanics.Notifications;
    using Fibula.Scheduling;
    using Fibula.Scheduling.Contracts.Abstractions;

    /// <summary>
    /// Abstract class that represents a base for all conditions.
    /// </summary>
    public abstract class Condition : BaseEvent, ICondition
    {
        /// <summary>
        /// A threshold within which to accept the end of the condition.
        /// </summary>
        private const int EndTimeMillisecondsThreshold = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="conditionType">The type of exhaustion.</param>
        /// <param name="endTime">The date and time at which the condition is set to end.</param>
        public Condition(ConditionType conditionType, DateTimeOffset endTime)
        {
            this.Type = conditionType;
            this.EndTime = endTime;
        }

        /// <summary>
        /// Gets the type of this condition.
        /// </summary>
        public ConditionType Type { get; }

        /// <summary>
        /// Gets or sets the end time for this condition.
        /// </summary>
        public DateTimeOffset EndTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this condition can be cured.
        /// </summary>
        public override bool CanBeCancelled { get; protected set; }

        /// <summary>
        /// Executes the event logic.
        /// </summary>
        /// <param name="context">The execution context.</param>
        public override void Execute(IEventContext context)
        {
            context.ThrowIfNull(nameof(context));

            var conditionContext = context as IConditionContext;

            var currentTime = context.CurrentTime;

            // Reset the condition's Repeat property, to avoid implementations running perpetually.
            this.RepeatAfter = TimeSpan.MinValue;

            // Check if we're ready to remove the exhaustion from the afflicted thing.
            if ((this.EndTime - currentTime).TotalMilliseconds > EndTimeMillisecondsThreshold)
            {
                var timeLeft = this.EndTime - currentTime;

                // Setup repeat to 'snooze' the removal.
                this.RepeatAfter = timeLeft;

                context.Logger.Debug($"{this.GetType().Name} not ready to end yet, snoozing for {timeLeft.TotalMilliseconds}");

                return;
            }

            // Pulse the condition so that any effects execute.
            this.Pulse(conditionContext);
        }

        /// <summary>
        /// Executes the operation's logic.
        /// </summary>
        /// <param name="context">The execution context for this condition.</param>
        protected abstract void Pulse(IConditionContext context);

        /// <summary>
        /// Sends a notification synchronously.
        /// </summary>
        /// <param name="context">A reference to the condition context.</param>
        /// <param name="notification">The notification to send.</param>
        protected void SendNotification(IConditionContext context, INotification notification)
        {
            notification.ThrowIfNull(nameof(notification));

            notification.Send(new NotificationContext(context.Logger, context.MapDescriptor, context.CreatureFinder));
        }
    }
}
