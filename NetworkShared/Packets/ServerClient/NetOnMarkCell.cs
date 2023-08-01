using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.NetworkShared.Models;

namespace TTT.Server.NetworkShared.Packets.ServerClient
{
    public struct NetOnMarkCell : INetPacket
    {
        public PacketType Type => PacketType.OnMarkCell;

        public string Actor { get; set; }

        public byte Index { get; set; }

        public MarkOutcome Outcome { get; set; }

        public WinLineType WinLineType { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            Actor = reader.GetString();
            Index = reader.GetByte();
            Outcome = (MarkOutcome)reader.GetByte();
            WinLineType = (WinLineType)reader.GetByte();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((byte)Type);
            writer.Put(Actor);
            writer.Put(Index);
            writer.Put((byte)Outcome);
            writer.Put((byte)WinLineType);
        }
    }
}
