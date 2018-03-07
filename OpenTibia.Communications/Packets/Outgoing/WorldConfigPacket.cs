﻿using OpenTibia.Server.Data;
using OpenTibia.Server.Data.Interfaces;

namespace OpenTibia.Communications.Packets.Outgoing
{
    public class WorldConfigPacket : PacketOutgoing
    {
        public override byte PacketType => (byte)ManagementOutgoingPacketType.NoType;

        public byte DailyResetHour { get; set; }
        public byte[] IpAddressBytes { get; set; }
        public ushort MaximumRookgardians { get; set; }
        public ushort MaximumTotalPlayers { get; set; }
        
        public ushort Port { get; set; }
        public ushort PremiumMainlandBuffer { get; set; }
        public ushort PremiumRookgardiansBuffer { get; set; }
        public byte WorldType { get; set; }

        public override void Add(NetworkMessage message)
        {
            message.AddByte(0x00);

            message.AddByte(WorldType);
            message.AddByte(DailyResetHour);

            message.AddBytes(IpAddressBytes);
            message.AddUInt16(Port);

            message.AddUInt16(MaximumTotalPlayers);
            message.AddUInt16(PremiumMainlandBuffer);
            message.AddUInt16(MaximumRookgardians);
            message.AddUInt16(PremiumRookgardiansBuffer);
        }

        public override void CleanUp()
        {
            // No references to clear.
        }
    }
}