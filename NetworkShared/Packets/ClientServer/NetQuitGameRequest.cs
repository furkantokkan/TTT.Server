using LiteNetLib.Utils;
using NetworkShared;
using System.Collections;
using System.Collections.Generic;

public class NetQuitGameRequest : INetPacket
{
    public PacketType Type => PacketType.QuitGameRequest;

    public void Deserialize(NetDataReader reader)
    {

    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put((byte)Type);
    }
}
