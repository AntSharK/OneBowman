using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MyFirstGame.Sprites;
using MyFirstGame.Screens.ScreenDecorators;

namespace MyFirstGame.Screens
{
    public class TestScreen: BaseScreen
    {
        /// <summary>
        /// Internal class of strings, used for reserved keywords
        /// See debate on: enum vs const string
        /// </summary>
        public class Elements
        {
            public const string Ground = "ground";
            public const string Tower = "tower";
            public const string Archer = "archer";
        }

        /// <summary>
        /// Initializes the test screen
        /// </summary>
        /// <param name="game">Our main game class</param>
        public TestScreen(): base()
        {
            this.AddSprite(new BaseSprite(
                CurrentGame.game.Content.Load<Texture2D>("Images\\towerback.png"),
                new Vector2(50, 250)), Elements.Tower);

            this.AddSprite(new BaseSprite(
                CurrentGame.game.Content.Load<Texture2D>("Images\\bigball.png"),
                new Vector2(125, 255)), Elements.Archer);

            this.AddSprite(new BaseSprite(
                CurrentGame.game.Content.Load<Texture2D>("Images\\groundlevel.png"), 
                new Vector2(0, 525)), Elements.Ground);

            this.AddSprite(new BaseSprite(
                CurrentGame.game.Content.Load<Texture2D>("Images\\towerfront.png"),
                new Vector2(50, 250)), Elements.Tower);

			this.camera = new Camera (800, 600) {
				Position = new Vector2 (400, 300)
			};
					
			CurrentGame.camera = this.camera;
            this.AddDecorator(new TestCameraDecorator(this));
        }

        /// <summary>
        /// Updates stuff.
        /// REMEMBER TO CALL BASE.UPDATE to draw things.
        /// </summary>
        /// <param name="gameTime">GameTime from main game</param>
        public override void Update(GameTime gameTime)
        {
			base.Update(gameTime);
        }
    }
}
