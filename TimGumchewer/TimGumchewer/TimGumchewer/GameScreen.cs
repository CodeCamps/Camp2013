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
        ContentManager Content;

        public GameScreen(ContentManager content)
        {
            this.Content = content;
            this.texNormal = content.Load<Texture2D>("normal");
            this.texVolcano = content.Load<Texture2D>("volcano");
        }



        float startX = -96;

        public override void Update(GameTime gameTime)
        {
            startX -= 3;
            if (startX < -96 * 2)
            {
                startX += 96;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Begin();

            Rectangle rect = new Rectangle(
                0, 0,
                batch.GraphicsDevice.Viewport.Width,
                batch.GraphicsDevice.Viewport.Height);

            batch.Draw(texVolcano, rect, Color.White);

            var loc = Vector2.Zero;

            loc.Y = batch.GraphicsDevice.Viewport.Height - 128;
            loc.X = startX;

            for (int i = 0; i < 10; i++)
            {
                batch.Draw(texNormal, loc, Color.White);
                loc.X += 96;
            }

            batch.End();
        }
    }
}
