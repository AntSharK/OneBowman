using System;
using System.Collections.Generic;
using System.Text;
using MyFirstGame.Sprites;
using MyFirstGame.Screens.ScreenDecorators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MyFirstGame.Screens
{
    /// <summary>
    /// Base screen class. All screens inherit from it.
    /// A screen is a layer to organize stuff with.
    /// </summary>
    public class BaseScreen
    {
        /// <summary>
        /// Game only draws visible screens. Invisible screens can be acted on.
        /// </summary>
        public bool isVisible;

        /// <summary>
        /// Game only updates active screens. Inactive screens can still be drawn.
        /// </summary>
        public bool isActive;
        
        /// <summary>
        /// Ordered list of every sprite in the screen
        /// </summary>
        public List<BaseSprite> sprites = new List<BaseSprite>();

        /// <summary>
        /// Ordered list of every decorator
        /// </summary>
        public List<BaseScreenDecorator> decorators = new List<BaseScreenDecorator>();

        /// <summary>
        /// For referencing special sprites, such as hero
        /// Note that this should reference an element in the sprites list
        /// To put something here, add it to both the sprites list as well as the dictionary
        /// </summary>
        public Dictionary<string, BaseSprite> reservedSprite = new Dictionary<string, BaseSprite>();
        
        /// <summary>
        /// Reference to this screen's special content manager
        /// </summary>
        public ContentManager content;

		public Camera camera;

        /// <summary>
        /// Initializes the screen.
        /// By default, it is visible and active on initialization.
        /// </summary>
        /// <param name="game">Reference to the main game</param>
        public BaseScreen()
        {
            this.isActive = true;
            this.isVisible = true;
        }

		public void SetCamera(Camera camera) {
			this.camera = camera;
		}

        /// <summary>
        /// Draws every sprite.
        /// Only draws visible sprites.
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public void Draw(GameTime gameTime)
        {
			if (camera != null)
				CurrentGame.spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.matrix);
			else
				CurrentGame.spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
            foreach (BaseSprite sprite in sprites)
            {
                if (sprite.isVisible)
                    sprite.Draw(CurrentGame.spriteBatch);
            }
			CurrentGame.spriteBatch.End();
        }

        /// <summary>
        /// Acts on every sprite. In order.
        /// Only acts on active sprites.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) 
        {
			if (this.camera != null)
				this.camera.Update (gameTime);
            foreach (BaseScreenDecorator decorator in decorators)
            {
                if (decorator.isActive)
                    decorator.Act(gameTime);
            }
            foreach (BaseSprite sprite in sprites)
            {
                if (sprite.isActive)
                    sprite.Update(gameTime);
            }
        }

        /// <summary>
        /// Utility method to add a sprite to the end of the list.
        /// </summary>
        /// <param name="sprite">Sprite to add</param>
        /// <param name="special">Optional parameter to add as a reserved sprite</param>
        /// <returns>True if successful, false if cannot be added to list</returns>
        public bool AddSprite(BaseSprite sprite, string special = "")
        {
            this.sprites.Add(sprite);
            sprite.screen = this;
            if (special != "")
            {
                try
                {
                    this.reservedSprite.Add(special, sprite);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Removes a reserved sprite
        /// </summary>
        /// <param name="spriteName">Name of sprite to be removed</param>
        /// <returns>True if successfully removed, false otherwise</returns>
        public bool RemoveSprite(string spriteName)
        {
            try
            {
                BaseSprite toBeRemoved;
                this.reservedSprite.TryGetValue(spriteName, out toBeRemoved);
                this.sprites.Remove(toBeRemoved);
                this.reservedSprite.Remove(spriteName);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Because it's just nicer to do this than call add
        /// </summary>
        /// <param name="decorator">Decorator to add</param>
        /// <returns>True all the time</returns>
        public bool AddDecorator(BaseScreenDecorator decorator)
        {
            this.decorators.Add(decorator);
            return true;
        }
    }
}
