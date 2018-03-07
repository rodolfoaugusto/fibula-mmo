﻿using System;
using System.Linq;
using OpenTibia.Communications.Packets.Incoming;
using OpenTibia.Data.Contracts;
using OpenTibia.Server.Data.Interfaces;
using OpenTibia.Server.Data.Models.Structs;
using OpenTibia.Server.Events;

namespace OpenTibia.Server.Actions
{
    internal class UseItemOnAction : PlayerAction
    {

        public UseItemOnAction(IPlayer player, ItemUseOnPacket useOnPacket, Location retryLocation)
            : base(player, useOnPacket, retryLocation)
        {

        }

        protected override void InternalPerform()
        {
            var useOnPacket = Packet as ItemUseOnPacket;

            if (useOnPacket == null)
            {
                return;
            }

            IThing thingToUse = null;
            switch (useOnPacket.FromLocation.Type)
            {
                case LocationType.Ground:
                    thingToUse = Game.Instance.GetTileAt(useOnPacket.FromLocation)?.GetThingAtStackPosition(useOnPacket.FromStackPosition);
                    break;
                case LocationType.Container:
                    var fromContainer = Player.GetContainer(useOnPacket.FromLocation.Container);
                    try
                    {
                        thingToUse = fromContainer.Content[fromContainer.Content.Count - useOnPacket.FromStackPosition - 1];
                    }
                    catch (ArgumentOutOfRangeException) { } // Happens when the content list does not contain the thing.
                    break;
                case LocationType.Slot:
                    try
                    {
                        thingToUse = Player.Inventory[Convert.ToByte(useOnPacket.FromLocation.Slot)];
                    }
                    catch
                    {
                        // ignored
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IThing thingToUseOn = null;
            switch (useOnPacket.ToLocation.Type)
            {
                case LocationType.Ground:
                    thingToUseOn = Game.Instance.GetTileAt(useOnPacket.ToLocation)?.GetThingAtStackPosition(useOnPacket.ToStackPosition);
                    break;
                case LocationType.Container:
                    var fromContainer = Player.GetContainer(useOnPacket.ToLocation.Container);
                    try
                    {
                        thingToUseOn = fromContainer.Content[fromContainer.Content.Count - useOnPacket.ToStackPosition - 1];
                    }
                    catch (ArgumentOutOfRangeException) { } // Happens when the content list does not contain the thing.
                    break;
                case LocationType.Slot:
                    try
                    {
                        thingToUseOn = Player.Inventory[Convert.ToByte(useOnPacket.ToLocation.Slot)];
                    }
                    catch
                    {
                        // ignored
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (thingToUse == null || thingToUseOn == null)
            {
                return;
            }

            var useEvents = Game.Instance.EventsCatalog[EventType.MultiUse].Cast<MultiUseEvent>();

            var candidate = useEvents.FirstOrDefault(e => e.ItemToUseId == useOnPacket.FromSpriteId && e.ItemToUseOnId == useOnPacket.ToSpriteId && e.Setup(thingToUse, thingToUseOn, Player) && e.CanBeExecuted);

            // Execute all actions.
            candidate?.Execute();
        }
    }
}