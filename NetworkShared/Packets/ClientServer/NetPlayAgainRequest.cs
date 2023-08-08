using LiteNetLib.Utils;
using NetworkShared;
using System.Collections;
using System.Collections.Generic;
using TTT.Server.NetworkShared.Packets.ServerClient;

namespace TTT.Server.NetworkShared.Packets.ClientServer
{
    public class NetPlayAgainRequest : INetPacket
    {
        public PacketType Type => PacketType.PlayAgainRequest;

        public void Deserialize(NetDataReader reader)
        {
           
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}