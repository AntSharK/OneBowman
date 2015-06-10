using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyFirstGame.Screens.ScreenDecorators
{
    /// <summary>
    /// Base class for screen decorators
    /// The decorator pattern is used to give actions to screens (e.g. cameras, or anything else)
    /// </summary>
    public class BaseScreenDecorator
    {
        /// <summary>
        /// The screen that this decorates
        /// </summary>
        protected BaseScreen screen;

        /// <summary>
        /// Whether this is active
        /// </summary>
        public bool isActive;

        /// <summary>
        /// Initializes the screen decorator
        /// </summary>
        /// <param name="screen">The screen to pass the decorator</param>
        public BaseScreenDecorator(BaseScreen screen)
        {
            this.isActive = true;
            this.screen = screen;
        }

        /// <summary>
        /// Override this method to do whatever you want it to
        /// </summary>
        public virtual void Act(GameTime gameTime){}
    }
}
