using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Utilities
{
    public static class BasicExtensions
    {
        public static (byte, byte) GetRowCol(byte index)
        {
            byte row = (byte)(index / 3);
            byte col = (byte)(index % 3);

            return (row, col);
        }
    }
}
