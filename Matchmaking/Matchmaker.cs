using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Game;
using TTT.Server.NetworkShared.Packets.ServerClient;

namespace TTT.Server.Matchmaking
{
    public class Matchmaker
    {
        private readonly ILogger logger;

        private readonly GameManager gameManager;

        private List<MatchhmakingTicket> pool = new List<MatchhmakingTicket>();

        private readonly NetworkServer server;

        public Matchmaker(ILogger<Matchmaker> logger,
             GameManager gameManager,
             NetworkServer server) 
        { 
            this.logger = logger;
            this.gameManager = gameManager;
            this.server = server;
        }

        public void RegisterPlayer(ServerConnection connection)
        {
            if (pool.Any(x => x.Connection.User.Id == connection.User.Id))
            {
                logger.LogWarning($"{connection.User.Id} is already registered!");
                return;
            }

            var request = new MatchhmakingTicket()
            {
                Connection = connection,
                SearchStartDate = DateTime.Now,
            };

            pool.Add(request);

            logger.LogInformation($"{request.Connection.User.Id} registered in matchmaking pool");

            StartHandshake();
        }

        public void TryUnregisterPlayer(string username)
        {
            var request = pool.FirstOrDefault(x => x.Connection.User.Id == username);

            if (request != null)
            {
                logger.LogInformation($"Removeing {request.Connection.User.Id} from the matchmaking pool");
                pool.Remove(request);
            }
        }

        private void StartHandshake()
        {
            List<MatchhmakingTicket> matchedRequest = new List<MatchhmakingTicket>();

            foreach (MatchhmakingTicket request in pool)
            {
                var match = pool.FirstOrDefault(x => !x.MatchFound &&
                x.Connection.ConnectionID != request.Connection.ConnectionID);

                if (match == null)
                {
                    continue;
                }

                request.MatchFound = true;
                match.MatchFound = true;

                matchedRequest.Add(request);
                matchedRequest.Add(match);

                string player1 = request.Connection.User.Id;
                string player2 = match.Connection.User.Id;

                var gameID = gameManager.RegisterGame(matchedRequest.Select(x => x.Connection.User.Id).ToList(), player1);

                PlayerData[] playerData = new PlayerData[matchedRequest.Count];

                for (int i = 0; i < matchedRequest.Count; i++)
                {
                    playerData[i] = new PlayerData()
                    {
                        Player = matchedRequest[i].Connection.User.Id,
                    };
                }

                request.Connection.GameID = gameID;
                match.Connection.GameID = gameID;

                var msg = new NetOnStartGame
                {
                    PlayersCount = (ushort)playerData.Length,
                    PlayerData = playerData,
                    GameId = gameID,
                };

                server.SendClient(request.Connection.ConnectionID, msg);
                server.SendClient(match.Connection.ConnectionID, msg);


                logger.LogInformation($"Player1: {player1} has matched with Player2: {player2}");
            }

            foreach (MatchhmakingTicket request in matchedRequest)
            {
                pool.Remove(request);
            }
        }
    }
}
