using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MyFirstGame.Sprites;

namespace MyFirstGame
{
	public class Camera
	{
        /// <summary>
		/// Position of camera. Denotes center of viewport.
		/// </summary>
		private Vector2 position;

		/// <summary>
		/// Rotation in radians.
		/// </summary>
		private float rotation;

		/// <summary>
		/// Camera zoom. Greater than 1 zooms in, and less than 1 zooms out
		/// </summary>
		private float scale;

		/// <summary>
		/// The sprite the camera will follow around on the X axis. If set to null, camera remains still.
		/// </summary>
		public BaseSprite spriteToFollowX;

        /// <summary>
        /// The sprite the camera will follow around on the Y axis. If set to null, camera remains still.
        /// </summary>
        public BaseSprite spriteToFollowY;

        /// <summary>
        /// A soft camera lock will lag behind the sprite it follows. A hard lock immediately updates position.
        /// </summary>
        public bool softLock;

		/// <summary>
		/// The linear transformation represented by position, rotation, and scale of the camera. Passed into SpriteBatch.Begin()
		/// </summary>
		public Matrix matrix;

		/// <summary>
		/// Viewing window of the camera. 
		/// </summary>
		public Vector2 viewport;           

		/// <summary>
		/// How fast the camera locks on to the character. 
		/// </summary>
		public float acceleration = 4f;
	
		/// <summary>
		/// How fast the camera changes scale
		/// </summary>
		public float zoomSpeed = 4;

		/// <summary>
		/// Setting this will change the zoom of the camera.
		/// </summary>
		public float targetScale;

        /// <summary>
		/// Determine whether the camera is currently shaking.
		/// </summary>
		private bool isShaking;

		/// <summary>
		/// Power of the shake.
		/// </summary>
		private float shakeMagnitude;

		/// <summary>
		/// How long the current shake lasts.
		/// </summary>
		private float shakeDuration;

		/// <summary>
		/// Duration of currentShake. Shake ends when the timer surpasse the duration.
		/// </summary>
		private float shakeTimer;

		/// <summary>
		/// The vector determining the current amount the shake moves the camera.
		/// </summary>
		private Vector2 shakeOffset;

		/// <summary>
		/// Property for changing position.
		/// </summary>
		public Vector2 Position
		{
			get { 
				return this.position; 
			}
			set {
                this.position = value;
                this.UpdateMatrix();
			}
		}
			
		/// <summary>
		/// Property for changing the rotation.
		/// </summary>
		public float Rotation
		{
			get
			{
                return this.rotation;
			}
			set
			{
                this.rotation = value;
                this.UpdateMatrix();
			}
		}

		/// <summary>
		/// Property for changing the scale
		/// </summary>
		public float Scale
		{
			get
			{
                return this.scale;
			}
			set
			{
                this.scale = value;
                this.UpdateMatrix();
			}
		}

		/// <summary>
		/// Initializes the camera
		/// </summary>
		/// <param name="width">Width of the viewport</param>
		/// <param name="height">Height of the viewport</param>
		public Camera(float width, float height)
		{
            this.position = Vector2.Zero;
            this.rotation = 0;
            this.scale = 0.5f;
            this.targetScale = 1f;
            this.viewport = new Vector2(width, height);
            this.softLock = false;
            this.UpdateMatrix();
		}

		/// <summary>
		/// Updates the camera's matrix. Called after any change to position, rotation, or scale.
		/// </summary>
		private void UpdateMatrix()
		{
            this.position = new Vector2((float)Math.Round(position.X, 3), (float)Math.Round(position.Y, 3));
            this.matrix = Matrix.CreateTranslation(-this.position.X, -this.position.Y, 0.0f) *
                Matrix.CreateRotationZ(this.rotation) *
                Matrix.CreateScale(this.scale) *
                Matrix.CreateTranslation(this.viewport.X / 2, this.viewport.Y / 2, 0.0f);
		}

		/// <summary>
		/// Updates the viewport with a new width and height.
		/// </summary>
		/// <param name="width">Width of the viewport</param>
		/// <param name="height">Height of the viewport</param>
		public void UpdateViewport(float width, float height)
		{
            this.viewport.X = width;
            this.viewport.Y = height;
            this.UpdateMatrix();
		}

		/// <summary>
		/// Begins a camera shake with a magnitude and duration.
		/// </summary>
		/// <param name="duration">How long the shake will be</param>
		/// <param name="magnitude">The furthest length of a single shake/param>
		public void Shake(float duration, float magnitude)
		{
            this.isShaking = true;

            this.shakeMagnitude = magnitude;
            this.shakeDuration = duration;

            this.shakeTimer = 0f;
		}

		/// <summary>
		/// Returns a point applied to the camera's transformation matrix.
		/// </summary>
		/// <param name="position">Point to transform</param>
		public Vector2 GetPointRelativeToCamera(Vector2 position)
		{
			return Vector2.Transform(position, matrix);
		}

		/// <summary>
		/// Transforms a camera point to a point in the world.
		/// </summary>
		/// <param name="position">Point to transform</param>
		public Vector2 CameraToWorldPoint(Vector2 position)
		{
			return Vector2.Transform(position, Matrix.Invert(matrix));
		}

		/// <summary>
		/// Moves camera to location of Sprite
		/// </summary>
		/// <param name="sprite">Sprite to move to</param>
        /// <param name="axis">X for x axis, Y for y axis, XY for both</param>
		public void ZoomToTarget(BaseSprite sprite, string axis = "XY")
		{
            if (axis.Contains("X"))
                this.position.X = sprite.position.X + sprite.origin.X;
            if (axis.Contains("Y"))
                this.position.Y = sprite.position.Y + sprite.origin.Y;
		}

		/// <summary>
		/// Sets the follow target for the camera.
		/// </summary>
		/// <param name="sprite">Sprite to followo</param>
        /// <param name="axis">X for x axis, Y for y axis, XY for both</param>
		public void SetTarget(BaseSprite sprite, string axis = "XY")
		{
            this.ZoomToTarget(sprite, axis);
            if (axis.Contains("X"))
			    this.spriteToFollowX = sprite;
            if (axis.Contains("Y"))
                this.spriteToFollowY = sprite;
		}

		/// <summary>
		/// Updates the shaking and handles the target following.
		/// </summary>
		/// <param name="gameTime">GameTime from main game</param>
		public void Update(GameTime gameTime)
		{
            if (this.isShaking)
			{
				// Move our timer ahead based on the elapsed time
                this.shakeTimer += CurrentGame.GetDelta(gameTime);

				// If we're at the max duration, we're not going to be shaking
				// anymore
                if (this.shakeTimer >= this.shakeDuration)
				{
                    this.isShaking = false;
                    this.shakeTimer = this.shakeDuration;
				}

				// Compute our progress in a [0, 1] range
                float progress = this.shakeTimer / this.shakeDuration;

				// Compute our magnitude based on our maximum value and our
				// progress. This causes
				// the shake to reduce in magnitude as time moves on, giving us a
				// smooth transition
				// back to being stationary. We use progress * progress to have a
				// non-linear fall
				// off of our magnitude. We could switch that with just progress if
				// we want a linear
				// fall off.
                float magnitude = this.shakeMagnitude * (1f - (progress * progress));

				// Generate a new offset vector with three random values and our
				// magnitude
                this.shakeOffset = new Vector2((float)CurrentGame.random.NextDouble() * 2 - 1,
                    (float)CurrentGame.random.NextDouble() * 2 - 1) * magnitude;
                this.Position += this.shakeOffset;
			}

            float delta = CurrentGame.GetDelta (gameTime);

            this.scale += (this.targetScale - this.scale) * this.zoomSpeed * delta;

            if (this.spriteToFollowX != null)
            {
				float spriteCenterX = spriteToFollowX.Center.X;

                if (this.softLock)
                {
                    this.position.X += ((spriteCenterX - position.X) * acceleration * delta);
                }
                else
                {
                    this.position.X = spriteCenterX;
                }
            }

            if (this.spriteToFollowY != null)
            {
                float spriteCenterY = spriteToFollowY.Center.Y;

                if (this.softLock)
                {
                    this.position.Y += ((spriteCenterY - position.Y) * acceleration * delta);
                }
                else
                {
                    this.position.Y = spriteCenterY;
                }
            }

			UpdateMatrix ();
		}

        /// <summary>
		/// Prevents the camera from moving out of the specificed rectangle.
		/// </summary>
		/// <param name="z">X value of rect</param>
		/// <param name="y">Y value of rect</param>
		/// <param name="width">Width of rect</param>
		/// <param name="height">Height of rect</param>
		public void ClampToArea(int x, int y, int width, int height)
		{
			// Calculate bounds of rectangle
			int left = x, right = x + width, top = y, bottom = y + height;

			// Calculate bounds of camera viewport
			float cameraLeft = position.X - (viewport.X / 2) / scale, cameraRight = position.X + (viewport.X / 2) / scale,
			cameraTop = position.Y - (viewport.Y / 2) / scale, cameraBottom = position.Y + (viewport.Y / 2) / scale;

			// use bounds to calcualate new camera bounds

			if (cameraLeft < left) {
				cameraRight += left - cameraLeft;
				cameraLeft = left;
			}
			if (cameraRight > right) {
				cameraLeft -= cameraRight - right;
				cameraRight = right;
			}
			if (cameraTop < top) {
				cameraBottom += top - cameraTop;
				cameraTop = top;
			}
			if (cameraBottom > bottom) {
				cameraTop -= cameraBottom - bottom;
				cameraBottom = bottom;
			}

			// Average camera bounds to determine new caemra position
			Position = new Vector2 ((cameraLeft + cameraRight) / 2, (cameraTop + cameraBottom) / 2);
		}
	}
}
