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
    [HandlerRegister(PacketType.SurrenderRequest)]
    public class SurrenderRequestHandler : IPacketHandler
    {
        private readonly UsersManager usersManager;
        private readonly GameManager gameManager;
        private readonly NetworkServer server;

        public SurrenderRequestHandler(UsersManager usersManager,
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
            var game = gameManager.FindGame(connection.User.Id);
            var opponentID = game.GetOpponent(connection.User.Id);
            game.AddWin(opponentID);
            usersManager.IncreaseScore(opponentID);

            var response = new NetOnSurrender
            {
                Winner = opponentID
            };

            var opponnentConnection = usersManager.GetConnection(opponentID);
            server.SendClient(opponnentConnection.ConnectionID, response);
            server.SendClient(connectionID, response);
        }
    }
}
