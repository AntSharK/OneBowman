using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyFirstGame.Sprites
{
    /// <summary>
    /// The MultiLayerSprite is an abstraction that manages a bunch of sprites that have to change draw ordering
    /// Pretty much maintains a list of sprites and ensures they're always sorted from back to front
    /// </summary>
    public class MultiLayerSprite : BaseSprite
    {
        /// <summary>
        /// List of sprites that we want to draw in some order
        /// </summary>
        public List<BaseSprite> sprites = new List<BaseSprite>();

        /// <summary>
        /// A bit that we flip every update cycle, for swapping
        /// </summary>
        public bool swapSwitch = false;

        /// <summary>
        /// Texture and position don't matter.
        /// </summary>
        public MultiLayerSprite()
            : base(ContentLoader.GetTexture("placeholder.png"), new Microsoft.Xna.Framework.Vector2(0, 0))
        {
        }

        /// <summary>
        /// Not cheap to sort at every cycle. Can be smarter, but smart is hard.
        /// </summary>
        /// <param name="gameTime">GameTime for updating</param>
        public override void Update(GameTime gameTime)
        {
            foreach (BaseSprite s in this.sprites)
            {
                s.Update(gameTime);
            }
        }

        /// <summary>
        /// Overriding draw to just draw the sprites
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            int index = this.swapSwitch ? 2 : 1;
            while (index < this.sprites.Count)
            {
                if (this.sprites[index].Center.Y < this.sprites[index-1].Center.Y)
                {
                    // Swap the sprites
                    BaseSprite temp = this.sprites[index];
                    this.sprites[index] = this.sprites[index - 1];
                    this.sprites[index - 1] = temp;
                }
                index = index + 2;
            }
            // Flip the switch on even or odd bubbling
            this.swapSwitch = !this.swapSwitch;

            foreach (BaseSprite s in this.sprites)
            {
                s.Draw(batch);
            }
        }

        /// <summary>
        /// Overriding draw to draw just the sprites
        /// </summary>
        public override void Draw()
        {
            this.Draw(CurrentGame.spriteBatch);
        }

        /// <summary>
        /// Class to compare sprites by their centers' Y coordinate
        /// </summary>
        private class OrderByCenter : IComparer<BaseSprite>
        {
            public int Compare(BaseSprite x, BaseSprite y)
            {
                if (x.Center.Y > y.Center.Y)
                {
                    return 1;
                }
                else if (x.Center.Y < y.Center.Y)
                {
                    return -1;
                }
                return 0;
            }
        }
    }
}
