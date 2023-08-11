using Microsoft.Extensions.Logging;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Game;
using TTT.Server.NetworkShared;
using TTT.Server.NetworkShared.Attributes;
using TTT.Server.NetworkShared.Packets.ClientServer;
using TTT.Server.NetworkShared.Packets.ServerClient;

namespace TTT.Server.ServerPacketHandlers
{
    [HandlerRegister(PacketType.AcceptPlayAgainRequest)]
    public class AcceptPlayAgainRequestHandler : IPacketHandler
    {
        private readonly UsersManager usersManager;
        private readonly GameManager gameManager;
        private readonly ILogger<AcceptPlayAgainRequestHandler> logger;
        private readonly NetworkServer server;

        public AcceptPlayAgainRequestHandler(
            UsersManager usersManager,
            GameManager gameManager,
            ILogger<AcceptPlayAgainRequestHandler> logger,
            NetworkServer server)
        {
            this.usersManager = usersManager;
            this.gameManager = gameManager;
            this.logger = logger;
            this.server = server;
        }
        public void Handle(INetPacket packet, int connectionID)
        {
            var connection = usersManager.GetConnection(connectionID);
            var userID = connection.User.Id;
            var game = gameManager.FindGame(userID);
            game.SetRematchState(userID);

            if (!game.BothPlayersAreReady())
            {
                logger.LogWarning("Bad State! Players are not ready!");
            }

            game.NewRound();

            string opponentID = game.GetOpponent(userID);
            ServerConnection opponentConnection = usersManager.GetConnection(opponentID);

            var response = new NetOnNewRound();
            server.SendClient(connection.ConnectionID, response);
            server.SendClient(opponentConnection.ConnectionID, response);
        }
    }
}
