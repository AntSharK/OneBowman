using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyFirstGame.Collider;

namespace MyFirstGame.Sprites
{
    public class Arrow: BaseSprite
    {
        public Arrow(GameRunner game):
            base(game.Content.Load<Texture2D>("Images\\arrow.png"), 
            new Vector2(game.GraphicsDevice.Viewport.TitleSafeArea.X, game.GraphicsDevice.Viewport.TitleSafeArea.Y + game.GraphicsDevice.Viewport.TitleSafeArea.Height / 2))
        {
            this.collider.addSphere(new CollisionSphere(25, this, 56, 0));
            this.collider.addSphere(new CollisionSphere(15, this, 86, 0));
        }

        public BaseSprite somesprite;

        /// <summary>
        /// Updates stuff.
        /// REMEMBER TO CALL BASE.UPDATE to draw things.
        /// </summary>
        /// <param name="gameTime">GameTime from main game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float r = 0;
            if (InputState.IsKeyDown(Keys.Right))
                r = CurrentGame.GetDelta(gameTime);

            if (InputState.IsKeyDown(Keys.Left))
                r = - CurrentGame.GetDelta(gameTime);

            float s = 0;

            if (InputState.IsKeyDown(Keys.Down))
                s = s - CurrentGame.GetDelta(gameTime) * 100;
            if (InputState.IsKeyDown(Keys.Up))
                s = s + CurrentGame.GetDelta(gameTime) * 100;

            float dx = s * (float)Math.Cos(this.rotation);
            float dy = s * (float)Math.Sin(this.rotation);

            this.position = this.position + new Vector2(dx, dy);
            this.rotation = this.rotation + r;

            if (this.collider.CollidesWith(somesprite))
            {
                this.position = this.position - new Vector2(dx, dy);
                this.rotation = this.rotation - r;
            }
            //somesprite.position = this.collider.smallSpheres[1].position;
        }
    }
}
