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

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.FindOpponentRequest)]
    public class FindOpponentRequestHandler : IPacketHandler
    {
        private readonly ILogger<FindOpponentRequestHandler> logger;
        private readonly UsersManager usersManager;
        private readonly Matchmaker matchmaker;
        public FindOpponentRequestHandler(ILogger<FindOpponentRequestHandler> logger, UsersManager usersManager,
            Matchmaker matchmaker)
        {
            this.logger = logger;
            this.usersManager = usersManager;
            this.matchmaker = matchmaker;
        }
        public void Handle(INetPacket packet, int connectionID)
        {
            var connection = usersManager.GetConnection(connectionID);
            matchmaker.RegisterPlayer(connection);

            logger.LogInformation("Recived Find Opponent Request");
        }
    }
}
