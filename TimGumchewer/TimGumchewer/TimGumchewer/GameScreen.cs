using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TimGumchewer
{
    public class GameScreen : Screen
    {
        Texture2D texSprites;
        Texture2D texBackground;
        SpriteFont fontScore;
        ContentManager Content;

        Dictionary<String, Rectangle> spriteRects = 
            new Dictionary<string, Rectangle>();

        public Dictionary<PlayerIndex, Player> players =
            new Dictionary<PlayerIndex, Player>();

        public GameScreen(ContentManager content)
        {
            this.Content = content;
            this.texBackground = content.Load<Texture2D>("cave");
            this.texSprites = content.Load<Texture2D>("Sprites");
            this.fontScore = content.Load<SpriteFont>("ScoreFont");

            InitSpriteRectangles();

            var player = new Player();
            player.PlayerIndex = PlayerIndex.One;
            player.Reset(1234567);

            players.Add(PlayerIndex.One, player);

            player = new Player();
            player.PlayerIndex = PlayerIndex.Two;
            player.Reset(1234567);

            players.Add(PlayerIndex.Two, player);
        }

        public void InitSpriteRectangles()
        {
            spriteRects.Add("fly", new Rectangle(0,0,128,128));
            spriteRects.Add("roll1", new Rectangle(128,0,128,128));
            spriteRects.Add("roll2", new Rectangle(256,0,128,128));
            spriteRects.Add("roll3", new Rectangle(384,0,128,128));
            spriteRects.Add("roll4", new Rectangle(0,128,128,128));
            spriteRects.Add("roll5", new Rectangle(128,128,128,128));
            spriteRects.Add("roll6", new Rectangle(256,128,128,128));
            spriteRects.Add("run1", new Rectangle(384,128,128,128));
            spriteRects.Add("run2", new Rectangle(0,256,128,128));
            spriteRects.Add("run3", new Rectangle(128,256,128,128));
            spriteRects.Add("run4", new Rectangle(256,256,128,128));
            spriteRects.Add("run5", new Rectangle(384,256,128,128));
            spriteRects.Add("run6", new Rectangle(0,384,128,128));
            spriteRects.Add("slide", new Rectangle(128, 384, 128, 128));
            spriteRects.Add("stand", new Rectangle(256, 384, 128, 128));
            spriteRects.Add("normal", new Rectangle(128, 512, 128, 128));
            spriteRects.Add("fire", new Rectangle(384,384,128,128));
            spriteRects.Add("gum", new Rectangle(0,512,128,128));
            spriteRects.Add("saws", new Rectangle(256,512,128,128));
            spriteRects.Add("spikes", new Rectangle(384,512,128,128));
            spriteRects.Add("heart0", new Rectangle(0, 640, 128, 128));
            spriteRects.Add("heart1", new Rectangle(128, 640, 128, 128));
            spriteRects.Add("heart2", new Rectangle(256, 640, 128, 128));
            spriteRects.Add("heart3", new Rectangle(384, 640, 128, 128));
        }

        DateTime gameStart = DateTime.Now;

        public void ResetGame()
        {
            Random rand = new Random();
            int seed = rand.Next();

            foreach (var player in players.Values)
            {
                player.Reset(seed);
            }

            gameStart = DateTime.Now;
        }

        public bool BothPlayersAreDead()
        {
            var result = true;
            foreach (var player in players.Values)
            {
                result = result && player.PlayerStatus == PlayerStatus.DEAD;
            }
            return result;
        }

        public override void Update(GameTime gameTime)
        {
            if (BothPlayersAreDead())
            {
                Game1.CurrentScreen = Game1.EndScreen;
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
                //ResetGame();
                return;
            }

            foreach (var playerIndex in players.Keys)
            {
                var player = players[playerIndex];
                var gamepad = GamePad.GetState(playerIndex);

                if (player.rumbleCounter > 0)
                {
                    GamePad.SetVibration(playerIndex, 1.0f, 1.0f);
                    player.rumbleCounter--;
                }
                else
                {
                    GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
                }

                if (player.Health == 0)
                {
                    player.PlayerStatus = PlayerStatus.DEAD;
                    continue;
                }

                var seconds = DateTime.Now.Subtract(gameStart).TotalSeconds;
                var speed = (float)Math.Min(3.0f + seconds / 60.0f, 15.0f);

                player.Speed = gamepad.ThumbSticks.Left.X * speed;
                var status = PlayerStatus.STANDING;

                if (player.Speed < 0.0f)
                {
                    player.Speed = 0.0f;
                }

                bool isJumping = player.PlayerStatus == PlayerStatus.JUMPING;

                bool isSliding =
                    (gamepad.Buttons.B == ButtonState.Pressed) ||
                    (gamepad.Triggers.Right > 0.0f);

                if (isSliding && !isJumping)
                {
                    status = PlayerStatus.SLIDING;
                }
                else if((!player.wasJumpButtonPressed && gamepad.Buttons.A == ButtonState.Pressed) || isJumping)
                {
                    player.wasJumpButtonPressed = true;
                    isJumping = true;
                    status = PlayerStatus.JUMPING;
                    player.elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (player.elapsedFrameTime > 100.0f)
                    {
                        player.jumpFrame++;
                        player.elapsedFrameTime = 0.0f;
                        if (player.jumpFrame == player.jumpFrames.Length - 1)
                        {
                            status = PlayerStatus.RUNNING;
                        }
                    }
                }
                else if (player.Speed <= 0.0f)
                {
                    player.Speed = 0.0f;
                    status = PlayerStatus.STANDING;
                    player.runFrame = 0;
                    player.jumpFrame = 0;
                }
                else
                {
                    status = PlayerStatus.RUNNING;
                    player.jumpFrame = 0;
                    player.elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (player.elapsedFrameTime > 100.0f)
                    {
                        player.runFrame++;
                        player.elapsedFrameTime = 0.0f;
                    }
                }

                player.PlayerStatus = status;
                player.Move(player.Speed);
                player.wasJumpButtonPressed = gamepad.Buttons.A == ButtonState.Pressed || isJumping;

                var tactic = player.tiles[2].Tactic;
                if (tactic == Tactic.JUMP_OR_DIVE && status != PlayerStatus.JUMPING)
                {
                    player.Health--;
                    player.Move(96);
                    player.rumbleCounter = 15;
                }
                else if (tactic == Tactic.SLIDE && status != PlayerStatus.SLIDING)
                {
                    player.Health--;
                    player.Move(96);
                    player.rumbleCounter = 25;
                }
            }
        }

        Rectangle rectScreen = Rectangle.Empty;

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Begin();

            if (rectScreen == Rectangle.Empty)
            {
                var viewport = batch.GraphicsDevice.Viewport;
                rectScreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            }

            batch.Draw(texBackground, rectScreen, Color.White);

            var loc = Vector2.Zero;

            foreach (var playerIndex in players.Keys)
            {
                var player = players[playerIndex];


                loc.X = player.startX;
                if (playerIndex == PlayerIndex.One)
                {
                    loc.Y = rectScreen.Height / 2 - 128;
                }
                else
                {
                    loc.Y = rectScreen.Height - 128;
                }

                for (int i = 0; i < 11; i++)
                {
                    var tile = player.tiles[i];
                    
                    Rectangle rectTile = spriteRects["normal"];
                    if (tile.TileType == TileType.GUM)
                    {
                        rectTile = spriteRects["gum"];
                    }
                    else if (tile.TileType == TileType.SAW_TOP)
                    {
                        rectTile = spriteRects["saws"];
                    }
                    else if (tile.TileType == TileType.FIRE)
                    {
                        rectTile = spriteRects["fire"];
                    }
                    else if (tile.TileType == TileType.SPIKE)
                    {
                        rectTile = spriteRects["spikes"];
                    }

                    batch.Draw(texSprites, loc, rectTile, Color.White);
                    loc.X += 96;
                }

                loc.X = 60;
                Rectangle rectTim = spriteRects["stand"];

                switch (player.PlayerStatus)
                {
                    case PlayerStatus.STANDING:
                        rectTim = spriteRects["stand"];
                        break;
                    case PlayerStatus.DEAD:
                    case PlayerStatus.SLIDING:
                        rectTim = spriteRects["slide"];
                        break;
                    case PlayerStatus.RUNNING:
                        rectTim = spriteRects[player.getCurrentRunFrame()];
                        break;
                    case PlayerStatus.JUMPING:
                        var rectName = player.getCurrentJumpFrame();
                        if (rectName != "END")
                        {
                            rectTim = spriteRects[rectName];
                        }
                        else
                        {
                            rectTim = spriteRects["stand"];
                        }
                        break;
                    default:
                        rectTim = spriteRects["stand"];
                        break;
                }

                batch.Draw(texSprites, loc, rectTim, Color.White);
                batch.Draw(texSprites, loc, spriteRects["heart" + player.Health], Color.White);
                loc.X = 5.0f;
                loc.Y = loc.Y + 128.0f - fontScore.LineSpacing - 2.0f;
                batch.DrawString(fontScore, player.Location.ToString("00000.0"), loc, Color.Black);

            }

            batch.End();
        }
    }
}
