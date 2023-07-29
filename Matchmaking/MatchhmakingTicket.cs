using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Game;

namespace TTT.Server.Matchmaking
{
    public class MatchhmakingTicket
    {
        public ServerConnection Connection { get; set; }

        public DateTime SearchStartDate { get; set; }

        public bool MatchFound { get; set; }
    }
}
