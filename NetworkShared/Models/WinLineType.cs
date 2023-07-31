using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.NetworkShared.Models
{
    public enum WinLineType
    {
        None,
        Diagonal,
        AntiDiagonal,
        ColLeft,
        ColMid,
        ColRight,
        RowTop,
        RowMiddle,
        RowBottom,
    }
}
