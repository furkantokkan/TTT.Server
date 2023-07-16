using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Data;

namespace TTT.Server.Game
{
    public class ServerConnection
    {
        public int ConnectionID { get; set; }

        public User User { get; set; }

        public NetPeer Peer { get; set; }

        public Guid? GameID { get; set; }
    }
}
