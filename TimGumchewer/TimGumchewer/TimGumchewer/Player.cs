using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TimGumchewer
{
    public class Player
    {
        public float Location;
        public float Speed;
        public PlayerIndex PlayerIndex;
        public PlayerStatus PlayerStatus;
        public int Health; // 0-3
        public float SpeedBonus; // can be negative
        public int CandiesCollected;
        public float startX;

        public int rumbleCounter = 0;

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
            "fly", "fly", "fly", "fly", "fly", 
            "roll2", "roll3", "roll4", "roll5", "roll6",
            "END"
        };

        public string getCurrentJumpFrame()
        {
            return jumpFrames[jumpFrame % jumpFrames.Length];
        }

        public bool wasSlideButtonPressed = false;
        public int slideFrame = 0;
        public string[] slideFrames = 
        { 
            "slide", "slide", "slide", "slide", "slide", 
            "slide", "slide", "slide", "slide", "slide",
            "END"
        };

        public string getCurrentSlideFrame()
        {
            return slideFrames[slideFrame % slideFrames.Length];
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
            this.wasJumpButtonPressed = true;
            //this.wasSlideButtonPressed = true;
            this.isObstacle = false;
            this.jumpFrame = 0;
            this.slideFrame = 0;
            this.runFrame = 0;
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
            if (isObstacle && tiles.Count > 5)
            {
                int value = rand.Next(4);
                var tile = new Tile();

                if (value == 0)
                {
                    tile.TileType = TileType.GUM;
                    tile.Tactic = PlayerStatus.JUMPING;
                }
                else if (value == 1)
                {
                    tile.TileType = TileType.SAWS;
                    tile.Tactic = PlayerStatus.SLIDING;
                }
                else if (value == 2)
                {
                    tile.TileType = TileType.SPIKE;
                    tile.Tactic = PlayerStatus.JUMPING;
                }
                else if (value == 3)
                {
                    tile.TileType = TileType.FIRE;
                    tile.Tactic = PlayerStatus.JUMPING;
                }

                tiles.Add(tile);
            }
            else
            {
                int numTiles = rand.Next(3) + 2;
                for (int i = 0; i < numTiles; i++)
                {
                    var tile = new Tile();
                    tile.TileType = TileType.NORMAL;
                    tile.Tactic = PlayerStatus.ANY;
                    tiles.Add(tile);
                }
            }

            isObstacle = !isObstacle;
        }

    }
}
