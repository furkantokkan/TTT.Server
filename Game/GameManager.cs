using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Game
{
    public class GameManager
    {
        private List<GameData> activeGames = new List<GameData>();

        public Guid RegisterGame(List<string> playerUsernames, string currentUser)
        {
            GameData newGame = new GameData(playerUsernames, currentUser);
            activeGames.Add(newGame);
            return newGame.ID;
        }
        public GameData FindGame(string username)
        {
            return activeGames.FirstOrDefault(x => x.players.Select(x => x.Player).Contains(username));
        }
        public GameData CloseGame(string username)
        {
            GameData game = FindGame(username);
            activeGames.Remove(game);
            return game;
        }
        public bool GameExists(string username) 
        {
            return activeGames.Any(x => x.players.Select(x => x.Player).Contains(username));
        }

        public int GetGameCount()
        {
            return activeGames.Count;
        }
    }
}
