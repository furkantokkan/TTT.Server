using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Data;
using TTT.Server.Extensions;
using TTT.Server.Game;
using TTT.Server.Matchmaking;
using TTT.Server.NetworkShared.Registries;

namespace TTT.Server.Infrastructure
{
    public static class Container
    {
        public static IServiceProvider Configure()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            return services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(c => c.AddSimpleConsole());
            services.AddSingleton<NetworkServer>();
            services.AddSingleton<PacketRegistry>();
            services.AddSingleton<HandlerRegistry>();
            services.AddSingleton<UsersManager>();
            services.AddSingleton<Matchmaker>();
            services.AddSingleton<GameManager>();
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddPacketHandlers();
        }
    }
}
