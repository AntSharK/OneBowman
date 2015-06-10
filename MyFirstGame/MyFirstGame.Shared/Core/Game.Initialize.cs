using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MyFirstGame.Sprites;
using MyFirstGame.Screens;
using Microsoft.Xna.Framework;

namespace MyFirstGame
{
    /// <summary>
    /// For better organization, everything built on startup is set here.
    /// </summary>
    public partial class GameRunner : Game
    {
        /// <summary>
        /// Ordered list of screens, to determine what to draw first
        /// </summary>
        List<BaseScreen> screens = new List<BaseScreen>();
        
        /// <summary>
        /// Dictionary of screens, to retrieve a specific screen
        /// </summary>
        Dictionary<string, BaseScreen> screenDictionary = new Dictionary<string, BaseScreen>();

        /// <summary>
        /// Constants that hold names of screens
        /// </summary>
        public class ScreenNames
        {
            public const string Test = "test";
        }
        
        /// <summary>
        ///  Initializes a new instance of the GameRunner class
        /// </summary>
        public GameRunner()
        {
            // Initialize current game's static variables for graphics
            CurrentGame.game = this;
            CurrentGame.graphics = new GraphicsDeviceManager(this);
			CurrentGame.graphicsDevice = GraphicsDevice;

            // Initialize root directory of content
            Content.RootDirectory = "Content";
            CurrentGame.content = Content;
			ContentLoader.content = Content;
			CurrentGame.graphics.PreferredBackBufferWidth = 800;
			CurrentGame.graphics.PreferredBackBufferHeight = 600;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            CurrentGame.spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load all the screens at the start here
            this.AddScreen(new TestScreen(), ScreenNames.Test);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
    }
}
