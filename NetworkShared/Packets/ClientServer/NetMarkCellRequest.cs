using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.NetworkShared.Packets.ClientServer
{
    public struct NetMarkCellRequest : INetPacket
    {
        public PacketType Type => PacketType.MarkCellRequest;

        public byte Index { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Index = reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Index);
        }
    }
}
