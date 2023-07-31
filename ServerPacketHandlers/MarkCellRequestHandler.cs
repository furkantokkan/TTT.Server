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
using TTT.Server.Utilities;

namespace TTT.Server.ServerPacketHandlers
{
    [HandlerRegister(PacketType.MarkCellRequest)]
    public class MarkCellRequestHandler : IPacketHandler
    {
        private readonly UsersManager usersManager;
        private readonly GameManager gameManager;
        private readonly NetworkServer server;

        public MarkCellRequestHandler(UsersManager usersManager, 
            GameManager gameManager,
            NetworkServer server)
        {
            this.usersManager = usersManager;
            this.gameManager = gameManager;
            this.server = server;
        }
        public void Handle(INetPacket packet, int connectionID)
        {
            var msg = (NetMarkCellRequest)packet;
            var connection = usersManager.GetConnection(connectionID);
            var userID = connection.User.Id;
            GameData game = gameManager.FindGame(userID);

            Validate(msg.Index, userID, game);

            var result = game.MarkCell(msg.Index);

            var response = new NetOnMarkCell()
            {
                Actor = userID,
                Index = msg.Index,
                Outcome = result.Outcome,
                WinLineType = result.WinLineType
            };

            string opponentID = game.GetOpponent(userID);
            ServerConnection opponentConnection = usersManager.GetConnection(opponentID);

            server.SendClient(connection.ConnectionID, response);
            server.SendClient(opponentConnection.ConnectionID, response);
        }
        private void Validate(byte index, string userID, GameData game)
        {
            if (game.CurrentUser != userID)
            {
                throw new ArgumentException($"[Bad Request] userID {userID} is not the current user!");
            }

            var (row, col) = BasicExtensions.GetRowCol(index);

            if (game.Grid[row, col] != 0)
            {
                throw new ArgumentException($"[Bad Request] cell with {index} is already marked!");
            }


        }
    }
}
