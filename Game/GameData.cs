using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.NetworkShared.Models;
using TTT.Server.Utilities;

namespace TTT.Server
{
    public class GameData
    {
        private const int GRID_SIZE = 3;
        public GameData(List<string> playersList, string currentUser)
        {
            ID = Guid.NewGuid();

            StartTime = DateTime.UtcNow;
            CurrentRoundStartTime = DateTime.UtcNow;

            players = new PlayerData[playersList.Count];

            for (int i = 0; i < playersList.Count; i++)
            {
                players[i].Player = playersList[i];
            }

            Round = 1;
            Grid = new MarkType[GRID_SIZE, GRID_SIZE];

            CurrentUser = currentUser;
        }


        public Guid ID { get; set; }

        public ushort Round { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime CurrentRoundStartTime { get; set; }

        public PlayerData[] players { get; set; }

        public string CurrentUser { get; set; }

        public MarkType[,] Grid { get; }

        public string GetOpponent(string otherUserID)
        {
            if (otherUserID == players[0].Player)
            {
                return players[1].Player;
            }
            else
            {
                return players[0].Player;
            }
        }
        public void SwitchCurrentPlayer()
        {
            CurrentUser = GetOpponent(CurrentUser);
        }

        public void AddWin(string winnerUserID)
        {
            MarkType winnerType = GetPlayerType(winnerUserID);
            if (winnerType == MarkType.X)
            {
                players[0].PlayerWins++;
            }
            else
            {
                players[1].PlayerWins++;
            }
        }
        public void SetRematchState(string userID)
        {
            MarkType playerType = GetPlayerType(userID);
            if (playerType == MarkType.X)
            {
                players[0].PlayerWantsRematch = true;
            }
            else
            {
                players[1].PlayerWantsRematch = true;
            }
        }

        #region CheckForWin
        public MarkResult MarkCell(byte index)
        {
            var (row, col) = BasicExtensions.GetRowCol(index);
            Grid[row, col] = GetPlayerType(CurrentUser);

            var (isWin, lineType) = CheckWin(row, col);
            bool draw = CheckDraw();

            MarkResult result = new MarkResult();

            if (isWin)
            {
                result.Outcome = MarkOutcome.Win;
                result.WinLineType = lineType;
            }
            else if (draw)
            {
                result.Outcome = MarkOutcome.Draw;
            }

            return result;
        }

        private bool CheckDraw()
        {
            for (int row = 0; row < GRID_SIZE; row++)
            {
                for (int col = 0; col < GRID_SIZE; col++)
                {
                    if (Grid[row, col] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private (bool isWin, WinLineType lineType) CheckWin(byte row, byte col)
        {
            MarkType type = Grid[row, col];

            //check col
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if (Grid[row, i] != type)
                {
                    break;
                }
                if (i == GRID_SIZE - 1)
                {
                    return (true, ResolveLineTypeRow(row));
                }
            }

            //check row
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if (Grid[i, col] != type)
                {
                    break;
                }

                if (i == GRID_SIZE - 1)
                {
                    return (true, ResolveLineTypeCol(col));
                }
            }

            //check diagonal

            if (row == col)
            {
                //we are on a diagonal

                for (int i = 0; i < GRID_SIZE; i++)
                {
                    if (Grid[i, i] != type)
                    {
                        break;
                    }

                    if (i == GRID_SIZE - 1)
                    {
                        return (true, WinLineType.Diagonal);
                    }
                }
            }

            //check anti diagonal

            if (row + col == GRID_SIZE - 1)
            {
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    if (Grid[i, (GRID_SIZE - 1) - i] != type)
                    {
                        break;
                    }

                    if (i == GRID_SIZE - 1)
                    {
                        return (true, WinLineType.AntiDiagonal);
                    }
                }
            }

            return (false, WinLineType.None);
        }

        private WinLineType ResolveLineTypeCol(byte col)
        {
            return (WinLineType)(col + 3);
        }

        private WinLineType ResolveLineTypeRow(byte row)
        {
            return (WinLineType)(row + 6);
        }

        private MarkType GetPlayerType(string userID)
        {
            if (userID == players[0].Player)
            {
                return MarkType.X;
            }
            else
            {
                return MarkType.O;
            }
        }
    }
    public struct MarkResult
    {
        public MarkOutcome Outcome { get; set; }

        public WinLineType WinLineType { get; set; }
    }
    #endregion
}
