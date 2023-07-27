using Microsoft.Extensions.Logging;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.NetworkShared;
using TTT.Server.NetworkShared.Attributes;
using TTT.Server.PacketHandlers;

namespace TTT.Server.ServerPacketHandlers
{
    [HandlerRegister(PacketType.CancelFindOpponentRequest)]
    public class CancelFindOpponentRequestHandler : IPacketHandler
    {
        private readonly ILogger<CancelFindOpponentRequestHandler> logger;
        public CancelFindOpponentRequestHandler(ILogger<CancelFindOpponentRequestHandler> logger)
        {
            this.logger = logger;
        }

        public void Handle(INetPacket packet, int connectionID)
        {
            logger.LogInformation("Recived cancel find opponent request!");
        }
    }
}
