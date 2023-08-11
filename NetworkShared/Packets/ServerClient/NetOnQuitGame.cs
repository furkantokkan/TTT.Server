using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NetOnQuitGame : INetPacket
{
    public PacketType Type => PacketType.OnQuitGame;

    public string Player { get; set; }

    public void Deserialize(NetDataReader reader)
    {
        Player = reader.GetString();
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)Type);
        writer.Put(Player);
    }
}