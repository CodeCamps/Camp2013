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
        Texture2D texTimWin;
        Texture2D texTimLose;
        SpriteFont fontEndScreen;

        public EndScreen(ContentManager content)
        {
            texTimWin = content.Load<Texture2D>("TimFaceWin");
            texTimLose = content.Load<Texture2D>("TimFaceLose");
            fontEndScreen = content.Load<SpriteFont>("EndScreenFont");
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

            Texture2D texPlayer1 = texTimWin;
            Texture2D texPlayer2 = texTimLose;

            var players = Game1.GameScreen.players;
            if (players[PlayerIndex.Two].Location > players[PlayerIndex.One].Location)
            {
                texPlayer1 = texTimLose;
                texPlayer2 = texTimWin;
            }
            else if (players[PlayerIndex.Two].Location == players[PlayerIndex.One].Location)
            {
                texPlayer1 = texTimLose;
                texPlayer2 = texTimLose;
            }

            Vector2 loc = new Vector2(10, Game1.rectScreen.Height - texTimWin.Height - 10);
            batch.Draw(texPlayer1, loc, Color.White);

            loc.Y -= 55;
            loc.X += 40;
            batch.DrawString(fontEndScreen, "Player One", loc, Color.White);

            loc = new Vector2(
                Game1.rectScreen.Width - texTimWin.Width - 10, 
                Game1.rectScreen.Height - texTimWin.Height - 10);
            batch.Draw(texPlayer2, loc, Color.White);

            loc.Y -= 55;
            loc.X += 40;
            batch.DrawString(fontEndScreen, "Player Two", loc, Color.White);

            if (Math.Sin(gameTime.TotalGameTime.TotalSeconds * 9.0) < 0.0)
            {
                loc.X = 250;
                loc.Y -= 70;
                batch.DrawString(fontEndScreen, "Press Start", loc, Color.White);
            }

            batch.End();
        }
    }
}
