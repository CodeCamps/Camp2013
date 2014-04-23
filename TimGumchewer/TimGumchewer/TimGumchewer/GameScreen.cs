﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TimGumchewer
{
    class GameScreen : Screen
    {
        Texture2D texSprites;
        Texture2D texBackground;
        ContentManager Content;

        Dictionary<String, Rectangle> spriteRects = 
            new Dictionary<string, Rectangle>();

        Dictionary<PlayerIndex, Player> players =
            new Dictionary<PlayerIndex, Player>();

        public GameScreen(ContentManager content)
        {
            this.Content = content;
            this.texBackground = content.Load<Texture2D>("cave");
            this.texSprites = content.Load<Texture2D>("Sprites");

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

        public override void Update(GameTime gameTime)
        {
            //Console.Write(players[PlayerIndex.One].Location);
            //Console.Write(" / ");
            //Console.WriteLine(players[PlayerIndex.Two].Location);

            foreach (var playerIndex in players.Keys)
            {
                var player = players[playerIndex];
                var gamepad = GamePad.GetState(playerIndex);

                player.Speed = gamepad.ThumbSticks.Left.X * 3.0f;
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
                }
                else if (tactic == Tactic.SLIDE && status != PlayerStatus.SLIDING)
                {
                    player.Health--;
                    player.Move(96);
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
            }

            batch.End();
        }
    }
}
