using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Screens;

namespace MyFirstGame.Sprites
{
    public class BaseSprite
    {
        /// <summary>
        /// Texture that this sprite displays
        /// </summary>
        public Texture2D texture;
        
        /// <summary>
        /// Position of sprite
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// Whether sprite is active. Active sprites are updated.
        /// </summary>
        public bool isActive;

        /// <summary>
        /// Whether sprite is visible. Visible sprites are drawn.
        /// </summary>
        public bool isVisible;

        /// <summary>
        /// Screen this sprite is attached to. Sprites cannot exist without screens/stages.
        /// </summary>
        public BaseScreen screen;

        /// <summary>
        /// Rotation in RADIANS. Clockwise.
        /// </summary>
        public float rotation;

        /// <summary>
        /// Origin point. By definition, the center of the sprite, unless otherwise needed.
        /// </summary>
        public Vector2 origin;

        /// <summary>
        /// Default of 1
        /// </summary>
        public Vector2 scale;

        /// <summary>
        /// Fade to white
        /// </summary>
        public float fade;

        /// <summary>
        /// Depth, for drawing
        /// </summary>
        public float depth;
        
        /// <summary>
        /// Initializes the sprite
        /// </summary>
        /// <param name="texture">Enter a texture here to load</param>
        /// <param name="position">Enter a position here</param>
        public BaseSprite(Texture2D texture, Vector2 position)
        {
            this.Initialize(texture, position);
        }

        /// <summary>
        /// Initializes a new sprite
        /// </summary>
        /// <param name="texture">Texture to draw for the sprite</param>
        /// <param name="position">Position, as a vector2</param>
        /// <param name="screen">Screen to attach the sprite to</param>
        public void Initialize(Texture2D texture, Vector2 position, BaseScreen screen = null)
        {
            this.texture = texture;
            this.position = position;
            this.isActive = true;
            this.isVisible = true;
            this.rotation = 0f;
            this.fade = 1.0f;
            this.origin = new Vector2(this.texture.Width / 2, this.texture.Height / 2);
            this.scale = new Vector2(1, 1);
            this.depth = 0;
            this.screen = screen;
        }

        /// <summary>
        /// The center of the sprite
        /// </summary>
		public Vector2 Center {
			get {
				return this.position + this.origin/2 * this.scale;
			}
		}

        /// <summary>
        /// Scales the sprite, in both dimensions, equally, by a FACTOR
        /// </summary>
        /// <param name="newScale">Factor to scale by</param>
        public virtual void SetScale(float newScale)
        {
            this.origin.X = this.origin.X * newScale;
            this.origin.Y = this.origin.Y * newScale;
            this.scale.X = this.scale.X * newScale;
            this.scale.Y = this.scale.Y * newScale;
        }
        
        /// <summary>
        /// Draws the sprite for the spritebatch
        /// </summary>
        /// <param name="batch">Spritebatch to draw on</param>
        public virtual void Draw(SpriteBatch batch)
        {
			batch.Draw(this.texture, this.position + this.origin, null, Color.White * fade, this.rotation, this.origin, this.scale, SpriteEffects.None, this.depth);
        }

        /// <summary>
        /// Draws the sprite for the current game's spritebatch
        /// </summary>
        public virtual void Draw()
        {
			CurrentGame.spriteBatch.Draw(this.texture, this.position + this.origin, Color.White);
        }

        /// <summary>
        /// Override this method
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        public virtual void Update(GameTime gameTime) { }
    }
}
