using LiteNetLib.Utils;
using NetworkShared;
using System;
using System.Collections.Generic;
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
    [HandlerRegister(PacketType.ServerStatusRequest)]
    public class ServerStatusRequestHandler : IPacketHandler
    {
        private readonly UsersManager usersManager;
        private readonly NetworkServer server;
        private readonly IUserRepository userRepository;

        public ServerStatusRequestHandler(
            UsersManager usersManager,
            NetworkServer server,
            IUserRepository userRepository
            )
        {
            this.usersManager = usersManager;
            this.server = server;
            this.userRepository = userRepository;
        }
        public void Handle(INetPacket packet, int connectionID)
        {
            var responseMessage = new NetOnServerStatus()
            {
                PlayersCount = userRepository.GetTotalCount(),
                TopPlayers = usersManager.GetTopPlayers()
            };
            server.SendClient(connectionID, responseMessage);
        }
    }
}
