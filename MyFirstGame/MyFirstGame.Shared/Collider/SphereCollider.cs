using System;
using System.Collections.Generic;
using System.Text;
using MyFirstGame.Sprites;

namespace MyFirstGame.Collider
{
    /// <summary>
    /// Attach a sphere collider to any sprite, and use it to check for collisions with other sprites
    /// </summary>
    public class SphereCollider
    {
        public CollisionSphere largeSphere;

        // Note that without small spheres, no collision detection actually happens
        public List<CollisionSphere> smallSpheres = new List<CollisionSphere>();

        public BaseSprite attachedSprite;

        public SphereCollider(BaseSprite sprite)
        {
            this.attachedSprite = sprite;
            
            // By default, the collision sphere is centered, with the diagonal as a radius
            this.largeSphere = new CollisionSphere((float) Math.Sqrt(sprite.texture.Width * sprite.texture.Width + sprite.texture.Height * sprite.texture.Height), sprite);
        }

        public bool CollidesWith(BaseSprite s)
        {
            this.largeSphere.update();
            s.collider.largeSphere.update();

            // The first exit. False if out of radius
            if (!this.largeSphere.CollidesWith(s.collider.largeSphere))
            {
                return false;
            }

            // The second exit. True if small spheres collide
            foreach (CollisionSphere s1 in smallSpheres)
            {
                s1.update();
                foreach (CollisionSphere s2 in s.collider.smallSpheres)
                {
                    s2.update();
                    if (s1.CollidesWith(s2))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void addSphere(CollisionSphere newSphere)
        {
            smallSpheres.Add(newSphere);
        }

        public void update()
        {
            this.largeSphere.update();
            foreach (CollisionSphere s in smallSpheres)
                s.update();
        }
    }
}
