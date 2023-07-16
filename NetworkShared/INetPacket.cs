using LiteNetLib.Utils;

namespace NetworkShared
{
    public interface INetPacket : INetSerializable
    {
        PacketType Type { get; }
    }
}