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

namespace TTT.Server.ServerPacketHandlers
{
    [HandlerRegister(PacketType.PlayAgainRequest)]
    public class PlayAgainRequestHandler : IPacketHandler
    {
        private readonly UsersManager usersManager;
        private readonly GameManager gameManager;
        private readonly NetworkServer server;
        public PlayAgainRequestHandler(UsersManager usersManager,
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
            var userID = connection.User.Id;
            var game = gameManager.FindGame(userID);
            game.SetRematchState(userID);


            string opponentID = game.GetOpponent(userID);
            ServerConnection opponentConnection = usersManager.GetConnection(opponentID);

            var response = new NetOnPlayAgain();
            server.SendClient(opponentConnection.ConnectionID, response);
        }
    }
}
