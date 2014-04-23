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
        public float startX;

        public int runFrame = 0;
        public float elapsedFrameTime = 0.0f;

        public string[] runFrames = { "run1", "run2", "run3", "run4", "run5", "run6" };
        public string getCurrentRunFrame()
        {
            return runFrames[runFrame % runFrames.Length];
        }

        public bool wasJumpButtonPressed = false;
        public int jumpFrame = 0;
        public string[] jumpFrames = 
        { 
            "roll1", "roll1",
            "fly", "fly", "fly", "fly", "fly", "fly", "fly", 
            "roll2", "roll3", "roll4", "roll5", "roll6",
            "END"
        };
        public string getCurrentJumpFrame()
        {
            return jumpFrames[jumpFrame % jumpFrames.Length];
        }

        public void Reset(int randomSeed)
        {
            this.Location = 0.0f;
            this.Speed = 3.0f;
            this.PlayerStatus = TimGumchewer.PlayerStatus.STANDING;
            this.Health = 3;
            this.SpeedBonus = 0.0f;
            this.CandiesCollected = 0;
            this.startX = -96;
            this.rand = new Random(randomSeed);
            this.tiles.Clear();
            this.FillTiles();
        }

        public void Move(float distance)
        {
            startX -= distance;
            Location += distance / 96.0f;
            if (startX < -96 * 2)
            {
                tiles.RemoveAt(0);
                startX += 96;
            }

            if (tiles.Count < 11)
            {
                FillTiles();
            }
        }

        public List<Tile> tiles = new List<Tile>();
        public Random rand = null;

        public void FillTiles()
        {
            while (tiles.Count < 11)
            {
                AddTiles();
            }
        }

        public bool isObstacle = false;

        public void AddTiles()
        {
            if (isObstacle)
            {
                int value = rand.Next(4);
                var tile = new Tile();

                if (value == 0)
                {
                    tile.TileType = TileType.GUM;
                    tile.Tactic = Tactic.JUMP_OR_DIVE;
                }
                else if (value == 1)
                {
                    tile.TileType = TileType.SAW_TOP;
                    tile.Tactic = Tactic.SLIDE;
                }
                else if (value == 2)
                {
                    tile.TileType = TileType.SPIKE;
                    tile.Tactic = Tactic.JUMP_OR_DIVE;
                }
                else if (value == 3)
                {
                    tile.TileType = TileType.FIRE;
                    tile.Tactic = Tactic.JUMP_OR_DIVE;
                }

                tiles.Add(tile);
            }
            else
            {
                int numTiles = rand.Next(6) + 2;
                for (int i = 0; i < numTiles; i++)
                {
                    var tile = new Tile();
                    tile.TileType = TileType.NORMAL;
                    tile.Tactic = Tactic.ANY;
                    tiles.Add(tile);
                }
            }

            isObstacle = !isObstacle;
        }

    }
}
