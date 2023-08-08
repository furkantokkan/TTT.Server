using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.NetworkShared.Packets.ServerClient
{
    public struct NetOnStartGame : INetPacket
    {
        public PacketType Type => PacketType.OnStartGame;

        public PlayerData[] PlayerData { get; set; }

        public ushort PlayersCount { get; set; }

        public Guid GameId { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            PlayersCount = reader.GetUShort();
            PlayerData = new PlayerData[PlayersCount];
            for (int i = 0; i < PlayersCount; i++)
            {
                PlayerData[i] = reader.Get<PlayerData>();
            }
            GameId = Guid.Parse(reader.GetString());
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(PlayersCount);
            for (int i = 0; i < PlayerData.Length; i++)
            {
                writer.Put(PlayerData[i]);
            }
            writer.Put(GameId.ToString());
        }
    }
}
