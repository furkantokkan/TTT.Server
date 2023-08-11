using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class NetOnSurrender : INetPacket
{
    public PacketType Type => PacketType.OnSurrender;

    public string Winner { get; set; }

    public void Deserialize(NetDataReader reader)
    {
        Winner = reader.GetString();
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)Type);
        writer.Put(Winner);
    }
}