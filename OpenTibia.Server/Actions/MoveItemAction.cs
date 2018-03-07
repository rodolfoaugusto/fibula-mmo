﻿using System;
using System.Threading.Tasks;
using OpenTibia.Communications.Packets.Incoming;
using OpenTibia.Data.Contracts;
using OpenTibia.Server.Data.Interfaces;
using OpenTibia.Server.Data.Models.Structs;
using OpenTibia.Server.Movement;

namespace OpenTibia.Server.Actions
{
    internal class MoveItemAction : PlayerAction
    {
        public MoveItemAction(IPlayer player, ItemMovePacket itemMovePacket, Location retryAtLocation)
            : base(player, itemMovePacket, retryAtLocation)
        {

        }

        protected override void InternalPerform()
        {
            var itemMovePacket = Packet as ItemMovePacket;

            if (itemMovePacket == null)
            {
                return;
            }

            switch(itemMovePacket.FromLocation.Type)
            {
                case LocationType.Ground:
                    MoveFromGround(itemMovePacket);
                    break;
                case LocationType.Container:
                    MoveFromContainer(itemMovePacket);
                    break;
                case LocationType.Slot:
                    MoveFromSlot(itemMovePacket);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Console.WriteLine($"Move {itemMovePacket.Count} {itemMovePacket.ClientId} from {itemMovePacket.FromLocation}:{itemMovePacket.FromStackPos} to {itemMovePacket.ToLocation}.");
        }

        private void MoveFromSlot(ItemMovePacket itemMovePacket)
        {
            var thing = Player.Inventory[(byte)itemMovePacket.FromLocation.Slot];

            var delayTime = TimeSpan.FromMilliseconds(200);
            IMovement movement = null;

            switch (itemMovePacket.ToLocation.Type)
            {
                case LocationType.Ground:
                    movement = new ThingMovementSlotToGround(Player.CreatureId, thing, itemMovePacket.FromLocation, itemMovePacket.ToLocation, itemMovePacket.Count);
                    break;
                case LocationType.Container:
                    movement = new ThingMovementSlotToContainer(Player.CreatureId, thing, itemMovePacket.FromLocation, itemMovePacket.ToLocation, itemMovePacket.Count);
                    break;
                case LocationType.Slot:
                    movement = new ThingMovementSlotToSlot(Player.CreatureId, thing, itemMovePacket.FromLocation, itemMovePacket.ToLocation, itemMovePacket.Count);
                    break;
            }

            // submit the movement.
            if (movement != null)
            {
                Task.Delay(delayTime).ContinueWith(previous => { Game.Instance.RequestMovement(movement); });
            }
        }

        private void MoveFromContainer(ItemMovePacket itemMovePacket)
        {
            var container = Player.GetContainer(itemMovePacket.FromLocation.Container);
            var thing = container.Content[container.Content.Count - itemMovePacket.FromLocation.Z - 1];

            var delayTime = TimeSpan.FromMilliseconds(200);
            IMovement movement = null;

            switch (itemMovePacket.ToLocation.Type)
            {
                case LocationType.Ground:
                    movement = new ThingMovementContainerToGround(Player.CreatureId, thing, itemMovePacket.FromLocation, itemMovePacket.ToLocation, itemMovePacket.Count);
                    break;
                case LocationType.Container:
                    movement = new ThingMovementContainerToContainer(Player.CreatureId, thing, itemMovePacket.FromLocation, itemMovePacket.ToLocation, itemMovePacket.Count);
                    break;
                case LocationType.Slot:
                    movement = new ThingMovementContainerToSlot(Player.CreatureId, thing, itemMovePacket.FromLocation, itemMovePacket.ToLocation, itemMovePacket.Count);
                    break;
            }

            // submit the movement.
            if (movement != null)
            {
                Task.Delay(delayTime).ContinueWith(previous => { Game.Instance.RequestMovement(movement); });
            }
        }

        private void MoveFromGround(ItemMovePacket itemMovePacket)
        {
            var fromTile = Game.Instance.GetTileAt(itemMovePacket.FromLocation);
            var thing = fromTile?.GetThingAtStackPosition(itemMovePacket.FromStackPos);

            var delayTime = TimeSpan.FromMilliseconds(200);
            IMovement movement = null;

            switch (itemMovePacket.ToLocation.Type)
            {
                case LocationType.Ground:
                    if (thing is ICreature)
                    {                        
                        delayTime = TimeSpan.FromSeconds(1);
                        movement = new CreatureMovementOnMap(Player.CreatureId, thing as ICreature, itemMovePacket.FromLocation, itemMovePacket.ToLocation);
                    }
                    else
                    {
                        movement = new ThingMovementOnMap(Player.CreatureId, thing, itemMovePacket.FromLocation, itemMovePacket.FromStackPos, itemMovePacket.ToLocation, itemMovePacket.Count);
                    }
                    break;
                case LocationType.Container:
                    movement = new ThingMovementGroundToContainer(Player.CreatureId, thing, itemMovePacket.FromLocation, itemMovePacket.FromStackPos, itemMovePacket.ToLocation, itemMovePacket.Count);
                    break;
                case LocationType.Slot:
                    movement = new ThingMovementGroundToSlot(Player.CreatureId, thing, itemMovePacket.FromLocation, itemMovePacket.FromStackPos, itemMovePacket.ToLocation, itemMovePacket.Count);
                    break;
            }

            // submit the movement.
            if (movement != null)
            {
                Task.Delay(delayTime).ContinueWith(previous => { Game.Instance.RequestMovement(movement); });
            }
        }
    }
}