using Microsoft.Extensions.Logging;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Game;
using TTT.Server.Matchmaking;
using TTT.Server.NetworkShared;
using TTT.Server.NetworkShared.Attributes;
using TTT.Server.PacketHandlers;

namespace TTT.Server.ServerPacketHandlers
{
    [HandlerRegister(PacketType.CancelFindOpponentRequest)]
    public class CancelFindOpponentRequestHandler : IPacketHandler
    {
        private readonly ILogger<CancelFindOpponentRequestHandler> logger;
        private readonly Matchmaker matchmaker;
        private readonly UsersManager usersManager;

        public CancelFindOpponentRequestHandler(ILogger<CancelFindOpponentRequestHandler> logger, Matchmaker matchmaker,
            UsersManager usersManager)
        {
            this.logger = logger;
            this.matchmaker = matchmaker;
            this.usersManager = usersManager;
        }

        public void Handle(INetPacket packet, int connectionID)
        {
            logger.LogInformation("Recived cancel find opponent request!");
            var connection = usersManager.GetConnection(connectionID);
            matchmaker.TryUnregisterPlayer(connection.User.Id);
        }
    }
}
