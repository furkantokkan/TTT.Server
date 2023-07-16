using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.NetworkShared.Attributes;

namespace TTT.Server.NetworkShared.Registries
{
    public class HandlerRegistry
    {
        private Dictionary<PacketType, Type> handlers = new Dictionary<PacketType, Type>();

        public Dictionary<PacketType, Type> Handlers
        {
            get
            {
                if (handlers.Count == 0)
                {
                    Initialize();
                }

                return handlers;
            }
        }

        private void Initialize()
        {
            var handlersInProject = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.DefinedTypes)
                .Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition)
                .Where(x => typeof(IPacketHandler).IsAssignableFrom(x))
                .Select(x => (type: x, attribute: x.GetCustomAttribute<HandlerRegisterAttribute>()))
                .Where(x => x.attribute != null);

            foreach (var (type, attribute) in handlersInProject)
            {
                if (!handlers.ContainsKey(attribute.PacketType))
                {
                    handlers[attribute.PacketType] = type;
                }
                else
                {
                    throw new Exception("Multiple handlers for " + attribute.PacketType + " has detected!, Only one handler per packet type is supported!");
                }
            }
        }
    }
}
