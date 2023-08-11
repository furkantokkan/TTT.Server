using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.NetworkShared.Packets.ClientServer
{
    public struct NetAceptPlayAgainRequest : INetPacket
    {
        public PacketType Type => PacketType.AcceptPlayAgainRequest;

        public void Deserialize(NetDataReader reader)
        {
           
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}
