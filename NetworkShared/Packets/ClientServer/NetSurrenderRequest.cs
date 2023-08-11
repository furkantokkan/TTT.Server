using LiteNetLib.Utils;
using NetworkShared;
using System.Collections;
using System.Collections.Generic;

public class NetSurrenderRequest : INetPacket
{
    public PacketType Type => PacketType.SurrenderRequest;

    public void Deserialize(NetDataReader reader)
    {

    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)Type);
    }

}
