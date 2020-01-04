using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Engine.Exceptions
{
    public enum InvalidMoveReason
    {
        Unspecified = 0,
        PieceNotAtSpecifiedSquare,
        TargetSquareOccupiedByPlayer,
        CapturingButNotMarkedAsCapture
    }
}
