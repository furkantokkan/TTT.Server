using LiteNetLib.Utils;

public struct PlayerData : INetSerializable
{
    public string Player { get; set; }

    public ushort PlayerWins { get; set; }

    public bool PlayerWantsRematch { get; set; }

    public void Deserialize(NetDataReader reader)
    {
        Player = reader.GetString();
        PlayerWins = reader.GetUShort();
        PlayerWantsRematch = reader.GetBool();
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Player);
        writer.Put(PlayerWins);
        writer.Put(PlayerWantsRematch);
    }
}