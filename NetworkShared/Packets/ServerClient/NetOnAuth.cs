using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.NetworkShared.Packets.ServerClient
{
    public class NetOnAuth : INetPacket
    {
        public string username;

        public PacketType Type => PacketType.OnAuth;

        public void Deserialize(NetDataReader reader)
        {
            username = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(username);
        }
    }
}
