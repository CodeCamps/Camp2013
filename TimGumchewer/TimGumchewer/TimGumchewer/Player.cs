using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TimGumchewer
{
    class Player
    {
        public float Location;
        public float Speed;
        public PlayerIndex PlayerIndex;
        public PlayerStatus PlayerStatus;
        public int Health; // 0-3
        public float SpeedBonus; // can be negative
        public int CandiesCollected;

        public void Reset()
        {
            this.Location = 0.0f;
            this.Speed = 0.0f;
            this.PlayerStatus = TimGumchewer.PlayerStatus.WAITING;
            this.Health = 3;
            this.SpeedBonus = 0.0f;
            this.CandiesCollected = 0;
        }
    }
}
