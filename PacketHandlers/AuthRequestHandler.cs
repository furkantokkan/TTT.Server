using Microsoft.Extensions.Logging;
using Networkshared.Packets.ClientServer;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Data;
using TTT.Server.Game;
using TTT.Server.NetworkShared;
using TTT.Server.NetworkShared.Attributes;
using TTT.Server.NetworkShared.Packets.ServerClient;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        private readonly ILogger<AuthRequestHandler> logger;
        private readonly UsersManager usersManager;
        private readonly NetworkServer server;
        private readonly IUserRepository userRepository;

        public AuthRequestHandler(
            ILogger<AuthRequestHandler> logger,
            UsersManager usersManager,
            NetworkServer server,
            IUserRepository userRepository)
        {
            this.logger = logger;
            this.usersManager = usersManager;
            this.server = server;
            this.userRepository = userRepository;
        }
        public void Handle(INetPacket packet, int connectionID)
        {
            var msg = (NetAuthRequest)packet;

            logger.LogInformation($"Reciver login request for {msg.Username} with pass: {msg.Password}");

            var loginRecord = usersManager.LoginOrRegister(connectionID, msg.Username, msg.Password);

            INetPacket responseMessage;

            if (loginRecord)
            {
                responseMessage = new NetOnAuth();
            }
            else
            {
                responseMessage = new NetOnAuthFail();
            }

            server.SendClient(connectionID, responseMessage);

            if (loginRecord)
            {
                NotifyOtherPlayers(connectionID);
            }
        }

        private void NotifyOtherPlayers(int connectionID)
        {
            var response = new NetOnServerStatus()
            {
                PlayersCount = userRepository.GetTotalCount(),
                TopPlayers = usersManager.GetTopPlayers()
            };
            var AllID = usersManager.GetOtherConnectionIds(connectionID);
            
            foreach (var otherPlayers in AllID)
            {
                server.SendClient(otherPlayers, response);
            }
        }
    }
}
