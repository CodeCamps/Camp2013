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
    public class EndScreen : Screen
    {
        public EndScreen(ContentManager content)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                Game1.CurrentScreen = Game1.TitleScreen;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Begin();

            batch.Draw(Game1.texCave, Game1.rectScreen, null, Color.White); 

            batch.End();
        }
    }
}
