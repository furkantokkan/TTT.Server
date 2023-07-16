using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Game;
using TTT.Server.NetworkShared;
using TTT.Server.NetworkShared.Registries;

namespace TTT.Server
{
    public class NetworkServer : INetEventListener
    {
        NetManager netManager;

        private readonly ILogger<NetworkServer> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly NetDataWriter cachedWriter = new NetDataWriter();
        private UsersManager usersManager;

        public NetworkServer(ILogger<NetworkServer> logger, IServiceProvider provider)
        {
            this.logger = logger;
            this.serviceProvider = provider;
        }


        public void Start()
        {
            netManager = new NetManager(this)
            {
                DisconnectTimeout = 300000
            };

            int port = 9050;

            netManager.Start(port);

            usersManager = serviceProvider.GetRequiredService<UsersManager>();

            Console.WriteLine("Server is listening on port " + port.ToString());
        }

        public void PollEvent()
        {
            netManager.PollEvents();
        }
        public void OnConnectionRequest(ConnectionRequest request)
        {
            Console.WriteLine("Connection has requeset from " + request.RemoteEndPoint.ToString());
            request.Accept();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {

        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            using(var scope = serviceProvider.CreateScope())
            {
                try
                {
                    PacketType packetType = (PacketType)reader.GetByte();
                    var packet = ResolvePacket(packetType, reader);
                    var handler = ResolveHandler(packetType);

                    handler.Handle(packet, peer.Id);

                    reader.Recycle();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing message of type");
                }
            }

            //var data = Encoding.UTF8.GetString(reader.RawData);
            //Console.WriteLine("Data recived from client: " + data);

            ////reply client
            //var reply = "Test Reply!";
            //var bytes = Encoding.UTF8.GetBytes(reply);
            //peer.Send(bytes, DeliveryMethod.ReliableOrdered);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }

        public void OnPeerConnected(NetPeer peer)
        {
            logger.LogInformation($"Client connected to server: {peer.EndPoint}: ID: {peer.Id}");
            usersManager.AddConnection(peer);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            var connection = usersManager.GetConnection(peer.Id);
            netManager.DisconnectPeer(peer);
            usersManager.SetPlayerDisconnected(peer.Id);
            logger.LogInformation($"{connection.User?.Id} disconnected: {peer.Id}");
        }

        public IPacketHandler ResolveHandler(PacketType packetType)
        {
            var registry = serviceProvider.GetRequiredService<HandlerRegistry>();
            var type = registry.Handlers[packetType];
            return (IPacketHandler)serviceProvider.GetRequiredService(type);
        }

        private INetPacket ResolvePacket(PacketType packetType, NetPacketReader reader)
        {
            var registry = serviceProvider.GetRequiredService<PacketRegistry>();
            var type = registry.PacketTypes[packetType];
            var packet = (INetPacket)Activator.CreateInstance(type);
            packet.Deserialize(reader);
            return packet;
        }

        public void SendClient(int peerID, INetPacket packet, DeliveryMethod method = DeliveryMethod.ReliableOrdered)
        {
            var peer = usersManager.GetConnection(peerID).Peer;
            peer.Send(WriteSerializeable(packet), method);
        }

        private NetDataWriter WriteSerializeable(INetPacket packet)
        {
            cachedWriter.Reset();
            packet.Serialize(cachedWriter);
            return cachedWriter;
        }
    }
}
