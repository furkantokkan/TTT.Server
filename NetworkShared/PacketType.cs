using LiteNetLib.Utils;
using System.Collections;
using System.Collections.Generic;

namespace NetworkShared
{
    public enum PacketType : byte
    {
        #region ClientServer
        Invalid = 0,
        AuthRequest = 1,
        #endregion

        #region ServerClient
        OnAuth = 100
        #endregion
    }
}

