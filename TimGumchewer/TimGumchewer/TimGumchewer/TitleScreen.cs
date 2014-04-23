using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TimGumchewer
{
    public class TitleScreen : Screen
    {
        Texture2D texPressA; 

        public TitleScreen(ContentManager content)
        {
            texPressA = content.Load<Texture2D>("caveTitlePressA");
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                Game1.CurrentScreen = Game1.GameScreen;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Begin();

            batch.Draw(Game1.texCave, Game1.rectScreen, null, Color.White);

            if (Math.Sin(gameTime.TotalGameTime.TotalSeconds * 9.0) < 0.0)
            {
                batch.Draw(texPressA, Game1.rectScreen, null, Color.White);
            }

            batch.End();
        }
    }
}
