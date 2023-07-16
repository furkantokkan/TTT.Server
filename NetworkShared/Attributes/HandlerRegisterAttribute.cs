using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.NetworkShared.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlerRegisterAttribute : Attribute
    {
        public HandlerRegisterAttribute(PacketType type)
        {
            PacketType = type;
        }

        public PacketType PacketType { get; set; }
    }
}
