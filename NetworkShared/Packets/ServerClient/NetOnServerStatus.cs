using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.NetworkShared.Packets.ServerClient
{
    public class NetOnServerStatus : INetPacket
    {
        public PacketType Type => PacketType.OnServerStatus;

        public ushort PlayersCount { get; set; }

        public PlayerNetDto[] TopPlayers { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            PlayersCount = reader.GetUShort();

            ushort topPlayersCount = reader.GetUShort();

            TopPlayers = new PlayerNetDto[topPlayersCount];
            for (int i = 0; i < topPlayersCount; i++)
            {
                TopPlayers[i] = reader.Get<PlayerNetDto>();
            }
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(PlayersCount);
            writer.Put((ushort)TopPlayers.Length);
            for (int i = 0; i < TopPlayers.Length; i++)
            {
                writer.Put(TopPlayers[i]);
            }
        }
    }

    public struct PlayerNetDto : INetSerializable
    {
        public string Username { get; set; }

        public ushort Score { get; set; }

        public bool IsOnlie { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Username = reader.GetString();
            Score = reader.GetUShort();
            IsOnlie = reader.GetBool();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Username);
            writer.Put(Score);
            writer.Put(IsOnlie);
        }
    }
}
