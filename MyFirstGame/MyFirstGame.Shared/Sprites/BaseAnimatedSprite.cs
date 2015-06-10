using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyFirstGame.Sprites
{
    /// <summary>
    /// Base class for an animated sprite
    /// </summary>
    public class BaseAnimatedSprite : BaseSprite
    {
        /// <summary>
        /// Number of columns to chop the texture into
        /// </summary>
        public int numberOfColumns;
        
        /// <summary>
        /// Number of rows to chop the texture into
        /// </summary>
        public int numberOfRows;

        /// <summary>
        /// Animations and their names
        /// </summary>
        public Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        /// <summary>
        /// Currently active animation
        /// </summary>
        public Animation currentAnimation;

        /// <summary>
        /// Stores the current state
        /// </summary>
        public string currentState;

        /// <summary>
        /// Initializes a new animated sprite
        /// </summary>
        /// <param name="texture">Texture to put into the sprite</param>
        /// <param name="position">Position of sprite</param>
        /// <param name="numberOfColumns">Number of columns to chop texture into</param>
        /// <param name="numberOfRows">Number of rows to chop texture into</param>
        public BaseAnimatedSprite(Texture2D texture, Vector2 position, int numberOfColumns = 1, int numberOfRows = 1): base(texture, position)
        {
            this.numberOfColumns = numberOfColumns;
            this.numberOfRows = numberOfRows;
            
            // To correct for origin to be calculated off size of a single square, not the whole texture
            this.origin.X = this.origin.X / numberOfColumns;
            this.origin.Y = this.origin.Y / numberOfRows;
        }

        /// <summary>
        /// To be overridden. Just updates the current animation if it is active.
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.currentAnimation.isActive)
                this.currentAnimation.Update(gameTime);
        }

        /// <summary>
        /// Draws the current animation. Don't need to check if it's visible, since visibility is a sprite issue.
        /// </summary>
        /// <param name="batch">Spritebatch to draw on</param>
        public override void Draw(SpriteBatch batch)
        {
            this.currentAnimation.Draw(batch);
        }

        /// <summary>
        /// Draws the current animation, for the currentgame's spritebatch
        /// </summary>
        public override void Draw()
        {
            this.currentAnimation.Draw(CurrentGame.spriteBatch);
        }

        /// <summary>
        /// Sets the current animation to a given name
        /// </summary>
        /// <param name="animationName">Name of animation</param>
        /// <param name="resetCurrent">Whether to reset the current animation to the start</param>
        /// <param name="deactivate">Whether to deactivate the current animation</param>
        public void SetAnimation(string animationName, bool resetCurrent = true, bool deactivate = true)
        {
            if (this.currentAnimation == null)
                this.currentAnimation = this.animations[animationName];
            if (resetCurrent)
                this.currentAnimation.Reset();
            if (deactivate)
                this.currentAnimation.isActive = false;
            this.currentAnimation = animations[animationName];
            this.currentAnimation.isActive = true;
        }

        /// <summary>
        /// Sets current animation and also sets the state variable
        /// </summary>
        /// <param name="animationName">Name of animation</param>
        /// <param name="resetCurrent">Whether to reset the current animation to the start</param>
        /// <param name="deactivate">Whether to deactivate the current animation</param>
        public void SetStateAndAnimation(string animationName, bool resetCurrent = true, bool deactivate = true)
        {
            this.currentState = animationName;
            this.SetAnimation(animationName, resetCurrent, deactivate);
        }

        /// <summary>
        /// Adds an animation to the animated sprite
        /// </summary>
        /// <param name="xStart">X position of square to start from, with left being 0</param>
        /// <param name="yStart">Y position of square to start from, with top being 0.</param>
        /// <param name="xEnd">X position of square to end on. If we have 8 columns, the rightmost square is 7.</param>
        /// <param name="yEnd">Y position of square to end on. If we have 7 rows, the bottommost square is 6.</param>
        /// <param name="timePerFrame">Time per frame</param>
        /// <param name="animationName">Name to give the animation</param>
        /// <returns></returns>
        public bool addAnimation(int xStart, int yStart, int xEnd, int yEnd, float timePerFrame = 0.1f, string animationName = "", bool isReversible = false, bool isLooping = true, Action onFinish = null)
        {
            // Return false if there are invalid parameters
            if (xStart >= this.numberOfColumns || xEnd >= this.numberOfColumns || yEnd >= this.numberOfRows || yStart >= this.numberOfRows)
            {
                return false;
            }
            
            // Otherwise try creating a new animation and adding it
            try
            {
                Animation newAnimation = new Animation(this, xStart, yStart, xEnd, yEnd, timePerFrame, isReversible, isLooping);
                if (onFinish != null)
                    newAnimation.onFinish = onFinish;
                this.animations.Add(animationName, newAnimation);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Stores an animation
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// Whether the animation is active. Inactive animations can be drawn on active sprites, but don't animate.
        /// </summary>
        public bool isActive;

        /// <summary>
        /// An animation is only finished if it is PAST its last frame
        /// </summary>
        public bool isFinished;

        /// <summary>
        /// Whether the animation loops or just stops at its last frame
        /// </summary>
        public bool isLooping;

        /// <summary>
        /// Current frame number
        /// </summary>
        public int currentFrame;

        /// <summary>
        /// Total number of frames
        /// </summary>
        public int numberOfFrames;

        /// <summary>
        /// Squares to advance by for each frame
        /// Useful to set to a negative number. Say we want an animation to go from a later square to an earlier square.
        /// </summary>
        public int frameAdvance;

        /// <summary>
        /// Time between each frame
        /// </summary>
        public float timePerFrame;

        /// <summary>
        /// Time since last frame change
        /// </summary>
        public float timeElapsed;

        /// <summary>
        /// The sprite this animation is attached to
        /// </summary>
        public BaseAnimatedSprite sprite;

        /// <summary>
        /// The x position of the square of the texture this animation starts at
        /// </summary>
        public int xStart;

        /// <summary>
        /// The y position of the square of the texture this animation starts at
        /// </summary>
        public int yStart;

        /// <summary>
        /// The x position of the ending square of the texture for this animation
        /// </summary>
        public int xEnd;

        /// <summary>
        /// The y position of the ending square of the texture for this animation
        /// </summary>
        public int yEnd;

        /// <summary>
        /// The width of a square this animation takes up
        /// </summary>
        public int frameWidth;

        /// <summary>
        /// The height of a square that this animation takes up
        /// </summary>
        public int frameHeight;

        /// <summary>
        /// The rectangle array to mark out where in the texture this animation's frames are
        /// </summary>
        public Rectangle[] rectangles;

        /// <summary>
        /// Reversible animations go back and forth
        /// </summary>
        public bool isReversible;

        /// <summary>
        /// Action to perform on the animation's finishing
        /// </summary>
        public Action onFinish; 

        /// <summary>
        /// Initializes a new animation
        /// </summary>
        /// <param name="sprite">Sprite to attach animation to</param>
        /// <param name="xStart">X position of square in sprite to start this animation</param>
        /// <param name="yStart">Y position of square in sprite to start this animation</param>
        /// <param name="xEnd">X position of square in sprite to end this animation</param>
        /// <param name="yEnd">Y position of square in sprite to end this animation</param>
        /// <param name="timePerFrame">Duration of each frame</param>
		public Animation(BaseAnimatedSprite sprite, int xStart, int yStart, int xEnd, int yEnd, float timePerFrame = 0, bool isReversible = false, bool isLooping = true, int border = 0)
        {
            // Initialize a bunch of stuff
            this.isActive = true;
            this.isFinished = false;
            this.currentFrame = 0;
            this.timePerFrame = timePerFrame;
            this.timeElapsed = 0;
            this.sprite = sprite;
            this.xStart = xStart;
            this.yStart = yStart;
            this.xEnd = xEnd;
            this.yEnd = yEnd;

            // If we set the start before the end, the animation should go backwards
            if (yEnd < yStart || yEnd == yStart && xEnd < xStart)
            {
                this.frameAdvance = -1;
                this.numberOfFrames = xStart + sprite.numberOfColumns - xEnd + sprite.numberOfColumns * (yStart - yEnd - 1) + 1;
            }
            else
            {
                this.frameAdvance = 1;
                this.numberOfFrames = sprite.numberOfColumns - xStart + sprite.numberOfColumns * (yEnd - yStart - 1) + xEnd + 1;
            }

            this.isLooping = isLooping;
            this.isReversible = isReversible;
            this.frameWidth = sprite.texture.Width / sprite.numberOfColumns;
            this.frameHeight = sprite.texture.Height / sprite.numberOfRows;

            // Initialize rectangle positions
            if (this.isReversible && numberOfFrames > 1)
            {
                rectangles = new Rectangle[numberOfFrames * 2 - 2];
            }
            else
            {
                rectangles = new Rectangle[numberOfFrames];
            }

            int x = xStart;
            int y = yStart;
            for (int i = 0; i < numberOfFrames; i++)
            {
				rectangles[i] = new Rectangle(x * this.frameWidth+x*border, y * this.frameHeight+y*border, this.frameWidth, this.frameHeight);
                x = x + this.frameAdvance;
                if (x >= this.sprite.numberOfColumns)
                {
                    x = 0;
                    y = y + 1;
                }
                if (x < 0)
                {
                    x = this.sprite.numberOfColumns - 1;
                    y = y - 1;
                }
            }

            // Reversible animations have extra rectangles!
            if (this.isReversible && numberOfFrames > 1)
            {
                for (int i = 0; i < numberOfFrames - 2; i++)
                {
                    rectangles[numberOfFrames + i] = rectangles[numberOfFrames - i - 2];
                }

                this.numberOfFrames = this.numberOfFrames * 2 - 2;
            }
        }

        /// <summary>
        /// Updates the animation
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public void Update(GameTime gameTime)
        {
            if (this.isActive)
            {
				this.timeElapsed = this.timeElapsed + CurrentGame.GetDelta(gameTime);
                while (this.timeElapsed > this.timePerFrame)
                {
                    this.currentFrame++;
                    // Condition: If not looping, then stop at last frame and exit.
                    if (!this.isLooping && this.currentFrame >= this.numberOfFrames)
                    {
                        this.currentFrame = this.numberOfFrames - 1;
                        this.isFinished = true;
                        if (this.onFinish != null)
                        {
                            this.onFinish();
                        }
                        break;
                    }

                    this.timeElapsed = this.timeElapsed - this.timePerFrame;
                }
                while (this.currentFrame >= this.numberOfFrames)
                {
                    this.currentFrame = this.currentFrame - this.numberOfFrames;
                }
            }
        }

        /// <summary>
        /// Draws the current frame of this animation
        /// </summary>
        /// <param name="batch">Sprite batch to draw on</param>
        public void Draw(SpriteBatch batch)
        {
			batch.Draw(this.sprite.texture, this.sprite.position + new Vector2(this.rectangles[this.currentFrame].Width/2, this.rectangles[this.currentFrame].Height/2), this.rectangles[this.currentFrame], Color.White * this.sprite.fade, this.sprite.rotation, this.sprite.origin, this.sprite.scale, SpriteEffects.None, this.sprite.depth);
        }

        /// <summary>
        /// Resets current animation
        /// </summary>
        public void Reset()
        {
            this.currentFrame = 0;
            this.timeElapsed = 0;
            this.isFinished = false;
        }

        /// <summary>
        /// Stops current animation
        /// </summary>
        public void Stop()
        {
            this.isActive = false;
            this.Reset();
        }
    }
}
