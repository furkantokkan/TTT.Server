using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Data;
using TTT.Server.NetworkShared.Packets.ServerClient;

namespace TTT.Server.Game
{
    public class UsersManager
    {
        private Dictionary<int, ServerConnection> connections = new Dictionary<int, ServerConnection>();
        private readonly IUserRepository userRepository;

        public UsersManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public PlayerNetDto[] GetTopPlayers()
        {
            return userRepository.GetQuery().OrderByDescending(x => x.Score).Select(x => new PlayerNetDto()
            {
                Username = x.Id,
                Score = x.Score,
                IsOnlie = x.IsOnline
            })
            .Take(10)
            .ToArray();
        }
        public void AddConnection(NetPeer peer)
        {
            connections.Add(peer.Id, new ServerConnection()
            {
                ConnectionID = peer.Id,
                Peer = peer
            });
        }
        public void SetPlayerDisconnected(int peerID)
        {
            var connection = GetConnection(peerID);
            if (connection != null)
            {
                var userID = connection.User.Id;
                userRepository.SetOffline(userID);
            }

            connections.Remove(peerID);
        }
        public ServerConnection GetConnection(int peerID)
        {
            return connections[peerID];
        }
        public bool LoginOrRegister(int connectionID, string username, string password)
        {
            var dbUser = userRepository.Get(username);

            if (dbUser != null)
            {
                if (dbUser.Password != password)
                {
                    return false;
                }
            }

            if (dbUser == null)
            {
                var newUser = new User()
                {
                    Id = username,
                    Password = password,
                    IsOnline = true,
                    Score = 0,
                };

                userRepository.Add(newUser);
                dbUser = newUser;
            }

            if (connections.ContainsKey(connectionID))
            {
                dbUser.IsOnline = true;
                connections[connectionID].User = dbUser;
            }

            return true;
        }

        internal int[] GetOtherConnectionIds(int connectionID)
        {
            return connections.Keys.Where(x => x !=  connectionID).ToArray();
        }

    }
}
