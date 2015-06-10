using System;
using System.Collections.Generic;
using System.Text;
using MyFirstGame.Sprites;
using Microsoft.Xna.Framework;

namespace MyFirstGame.Collider
{
    /// <summary>
    /// A sphere for determining collision
    /// </summary>
    public class CollisionSphere
    {
        public Vector2 position;

        public float radius;

        public BaseSprite attachedSprite;

        public float distance;

        // Angle is in radians. 0 is bearing east, rotates clockwise.
        public float angle;

        public CollisionSphere(float radius, BaseSprite attachedSprite, float distance = 0, float angle = 0)
        {
            this.radius = radius;
            this.attachedSprite = attachedSprite;
            this.distance = distance;
            this.angle = angle;
            this.update();
        }

        public bool CollidesWith(CollisionSphere c)
        {
            float xdist = Math.Abs(this.position.X - c.position.X);
            float ydist = Math.Abs(this.position.Y - c.position.Y);
            float combinedRadius = this.radius + c.radius;

            // Exit criteria 1: X or Y distance is greater than combined radius
            if (xdist > combinedRadius)
                return false;

            if (ydist > combinedRadius)
                return false;

            // Exit criteria 2: Actual distance, via pythagoras theorem, is larger than combined radius
            if (Math.Sqrt(xdist * xdist + ydist * ydist) > combinedRadius)
                return false;

            return true;
        }

        public void update()
        {
            Vector2 offset = new Vector2(this.distance * (float)Math.Cos(this.attachedSprite.rotation), this.distance * (float)Math.Sin(this.attachedSprite.rotation));
            this.position = this.attachedSprite.position + this.attachedSprite.origin + offset;
        }
    }
}
