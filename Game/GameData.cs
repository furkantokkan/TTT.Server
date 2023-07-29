using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server
{
    public class GameData
    {
        public GameData(List<string> playersList, string currentUser)
        {
            ID = Guid.NewGuid();

            StartTime = DateTime.UtcNow;
            CurrentRoundStartTime = DateTime.UtcNow;

            players = new PlayerData[playersList.Count];

            for (int i = 0; i < playersList.Count; i++)
            {
                players[i].Player = playersList[i];
            }

            Round = 1;

            CurrentUser = currentUser;
        }


        public Guid ID { get; set; }

        public ushort Round { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime CurrentRoundStartTime { get; set; }

        public PlayerData[] players { get; set; }

        public string CurrentUser { get; set; }
    }
}
