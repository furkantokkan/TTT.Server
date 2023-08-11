using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Game;
using TTT.Server.NetworkShared;
using TTT.Server.NetworkShared.Attributes;

namespace TTT.Server.ServerPacketHandlers
{
    [HandlerRegister(PacketType.QuitGameRequest)]
    public class QuitGameRequestHandler : IPacketHandler
    {
        private readonly UsersManager usersManager;
        private readonly GameManager gameManager;
        private readonly NetworkServer server;

        public QuitGameRequestHandler(UsersManager usersManager,
            GameManager gameManager,
            NetworkServer server)
        {
            this.usersManager = usersManager;
            this.gameManager = gameManager;
            this.server = server;
        }
        public void Handle(INetPacket packet, int connectionID)
        {
            var connection = usersManager.GetConnection(connectionID);

            var response = new NetOnQuitGame()
            {
                Player = connection.User.Id
            };

            if (gameManager.GameExists(connection.User.Id))
            {
                var closedGame = gameManager.CloseGame(connection.User.Id);
                var opponent = closedGame.GetOpponent(connection.User.Id);
                var opponentConnection = usersManager.GetConnection(opponent);
                server.SendClient(opponentConnection.ConnectionID, response);
            }

            server.SendClient(connection.ConnectionID, response);
        }
    }
}
