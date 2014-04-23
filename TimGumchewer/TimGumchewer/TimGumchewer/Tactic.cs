using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimGumchewer
{
    [Flags]
    enum Tactic
    {
        RUN   = 1,
        SLIDE = 2,
        JUMP  = 4,
        DIVE  = 8,

        ANY = RUN | SLIDE | JUMP | DIVE,
        JUMP_OR_DIVE = JUMP | DIVE,
    }
}
