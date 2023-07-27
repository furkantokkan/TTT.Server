using Microsoft.Extensions.Logging;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.NetworkShared;
using TTT.Server.NetworkShared.Attributes;

namespace TTT.Server.PacketHandlers
{
    [HandlerRegister(PacketType.FindOpponentRequest)]
    public class FindOpponentRequestHandler : IPacketHandler
    {
        private readonly ILogger<FindOpponentRequestHandler> logger;
        public FindOpponentRequestHandler(ILogger<FindOpponentRequestHandler> logger)
        {
            this.logger = logger;
        }
        public void Handle(INetPacket packet, int connectionID)
        {
            logger.LogInformation("Recived Find Opponent Request");
        }
    }
}
