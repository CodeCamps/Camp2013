using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TimGumchewer
{
    class GameScreen : Screen
    {
        Texture2D texNormal;
        Texture2D texVolcano;
        Texture2D texObsticalGum;
        Texture2D texObsticalSawTop;
        Texture2D texObsticalSpikes;
        Texture2D texObsticalFire;
        ContentManager Content;

        Dictionary<PlayerIndex, Player> players =
            new Dictionary<PlayerIndex, Player>();

        public GameScreen(ContentManager content)
        {
            this.Content = content;
            this.texNormal = content.Load<Texture2D>("normal");
            this.texObsticalGum = content.Load<Texture2D>("mouse");
            this.texVolcano = content.Load<Texture2D>("cave");
            this.texObsticalSawTop = content.Load<Texture2D>("sawTop");
            this.texObsticalSpikes = content.Load<Texture2D>("spikes");
            this.texObsticalFire = content.Load<Texture2D>("fire");

            var player = new Player();
            player.PlayerIndex = PlayerIndex.One;
            player.Reset(1234567);

            players.Add(PlayerIndex.One, player);

            player = new Player();
            player.PlayerIndex = PlayerIndex.Two;
            player.Reset(1234567);

            players.Add(PlayerIndex.Two, player);
        }

        public override void Update(GameTime gameTime)
        {
            var player = players[PlayerIndex.One];
            player.Move(player.Speed);

            player = players[PlayerIndex.Two];
            player.Move(player.Speed);
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

            batch.Draw(texVolcano, rectScreen, Color.White);

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
                    
                    Texture2D texture = texNormal;
                    if (tile.TileType == TileType.GUM)
                    {
                        texture = texObsticalGum;
                    }
                    else if (tile.TileType == TileType.SAW_TOP)
                    {
                        texture = texObsticalSawTop;
                    }
                    else if (tile.TileType == TileType.FIRE)
                    {
                        texture = texObsticalFire;
                    }
                    else if (tile.TileType == TileType.SPIKE)
                    {
                        texture = texObsticalSpikes;
                    }

                    batch.Draw(texture, loc, Color.White);
                    loc.X += 96;
                }
            }

            batch.End();
        }
    }
}
