using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimGumchewer
{
    [Flags]
    public enum PlayerStatus
    {
        // used by Player class as PlayerStatus
        STANDING = 1,
        JUMPING  = 2,
        RUNNING  = 4,
        SLIDING  = 8,
        INJURED  = 16,
        DEAD     = 32,

        // used by Tile class as Tactic and by Player.AddTiles method
        ANY = STANDING | JUMPING | RUNNING | SLIDING | INJURED | DEAD,

        // used by GameScreen class to indicate a mismatched status and tactic
        NONE = 0,
    }
}
