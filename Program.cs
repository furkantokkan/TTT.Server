using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using TTT.Server;
using TTT.Server.Infrastructure;

//var server = new NetworkServer();
//server.Start();

var serviceProvider = Container.Configure();
var server = serviceProvider.GetRequiredService<NetworkServer>();
server.Start();

while (true)
{
    server.PollEvent();
    Thread.Sleep(15);
}