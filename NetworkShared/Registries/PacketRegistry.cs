using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.NetworkShared.Registries
{
    public class PacketRegistry
    {
        private Dictionary<PacketType, Type> packetTypes = new Dictionary<PacketType, Type>();

        public Dictionary<PacketType, Type> PacketTypes
        {
            get
            {
                if (packetTypes.Count == 0)
                {
                    Initialize();
                }

                return packetTypes;
            }
        }

        private void Initialize()
        {
            //Get all INetPacket's to the Dictionary

            var packetType = typeof(INetPacket);
            var packets = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(p => packetType.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var item in packets)
            {
                var instance = (INetPacket)Activator.CreateInstance(item);
                packetTypes.Add(instance.Type, item);
            }
        }
    }
}
