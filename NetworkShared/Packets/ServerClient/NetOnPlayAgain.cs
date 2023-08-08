using LiteNetLib.Utils;
using NetworkShared;
using System.Collections;
using System.Collections.Generic;

namespace TTT.Server.NetworkShared.Packets.ClientServer
{
    public class NetOnPlayAgain : INetPacket
    {
        public PacketType Type => PacketType.OnPlayAgain;

        public void Deserialize(NetDataReader reader)
        {

        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
        }
    }
}