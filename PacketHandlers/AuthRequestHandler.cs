using Microsoft.Extensions.Logging;
using Networkshared.Packets.ClientServer;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Game;
using TTT.Server.NetworkShared;
using TTT.Server.NetworkShared.Attributes;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        private readonly ILogger<AuthRequestHandler> logger;
        private readonly UsersManager usersManager;

        public AuthRequestHandler(
            ILogger<AuthRequestHandler> logger,
            UsersManager usersManager)
        {
            this.logger = logger;
            this.usersManager = usersManager;
        }
        public void Handle(INetPacket packet, int connectionID)
        {
            var msg = (NetAuthRequest)packet;

            logger.LogInformation($"Reciver login request for {msg.Username} with pass: {msg.Password}");

            //var loginRecord = usersManager.LoginOrRegister(connectionID, msg.Username, msg.Password);
        }
    }
}
