using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyFirstGame.Sprites
{
    /// <summary>
    /// This test sprite is just a black ball
    /// </summary>
    public class TestSprite : BaseSprite
    {
        public TestSprite(GameRunner game):
            base(game.Content.Load<Texture2D>("Images\\blackball.png"), 
            new Vector2(game.GraphicsDevice.Viewport.TitleSafeArea.X, game.GraphicsDevice.Viewport.TitleSafeArea.Y + game.GraphicsDevice.Viewport.TitleSafeArea.Height / 2))
        {
        }

        /// <summary>
        /// Updates stuff.
        /// REMEMBER TO CALL BASE.UPDATE to draw things.
        /// </summary>
        /// <param name="gameTime">GameTime from main game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InputState.IsKeyDown(Keys.Z))
            {
                this.isVisible = !this.isVisible;
            }
        }
    }
}
