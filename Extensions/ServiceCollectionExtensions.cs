using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.NetworkShared.Attributes;
using TTT.Server.NetworkShared;
using System.Reflection;

namespace TTT.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPacketHandlers(this IServiceCollection services)
        {
            var handlersInProject = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.DefinedTypes)
            .Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition)
            .Where(x => typeof(IPacketHandler).IsAssignableFrom(x))
            .Select(x => (type: x, attribute: x.GetCustomAttribute<HandlerRegisterAttribute>()))
            .Where(x => x.attribute != null);

            foreach (var (type, attribute) in handlersInProject)
            {
                services.AddScoped(type);
            }

            return services;
        }
    }
}
