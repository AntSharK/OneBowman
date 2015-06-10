using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Text;

namespace MyFirstGame
{
    public static class CurrentGame
    {
        /// <summary>
        /// Reference to current game
        /// </summary>
        public static GameRunner game;

        /// <summary>
        /// Graphics device manager
        /// </summary>
        public static GraphicsDeviceManager graphics;

        /// <summary>
		/// Graphics device 
		/// </summary>
		public static GraphicsDevice graphicsDevice;

        /// <summary>
        /// Spritebatch to draw sprites on
        /// </summary>
        public static SpriteBatch spriteBatch;

        /// <summary>
        /// Content manager of game
        /// </summary>
        public static ContentManager content;	

        /// <summary>
        /// Our game camera
        /// </summary>
		public static Camera camera;
        
        /// <summary>
        /// Timescale for matrix elevator slow-mo scenes and other explosions
        /// </summary>
		public static float timeScale = 1f;

        /// <summary>
        /// Random generator
        /// </summary>
		public static Random random = new Random();

        /// <summary>
        /// Returns the number of seconds passed, adjusted for time scale
        /// </summary>
        /// <param name="gameTime">Game time object</param>
        /// <returns>The number of seconds elapsed, adjusted for time scale</returns>
		public static float GetDelta(GameTime gameTime)
		{
			return (float) gameTime.ElapsedGameTime.TotalSeconds*timeScale;
		}
    }
}
