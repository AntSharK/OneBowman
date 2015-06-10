using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstGame
{
    /// <summary>
    /// InputState contains static methods to handle all input
    /// </summary>
	public static class InputState
	{
        /// <summary>
        /// Current Keyboard state
        /// </summary>
		private static KeyboardState currentKeyboardState;

        /// <summary>
        /// Previous Keyboard state
        /// </summary>
		private static KeyboardState previousKeyboardState;

        /// <summary>
        /// Duration which key has been down for, in seconds
        /// </summary>
		private static Dictionary<Keys, double> durations = new Dictionary<Keys, double>();
        
        /// <summary>
        /// Checks if a key is currently down
        /// </summary>
        /// <param name="key">The key we want to check</param>
        /// <returns>True if that key is down, false otherwise</returns>
		public static bool IsKeyDown(Keys key) {
			return currentKeyboardState.IsKeyDown (key);
		}


		/// <summary>
		/// Checks if a number of keys are currently down
		/// </summary>
		/// <param name="keys">The keys we want to check</param>
		/// <returns>True if all keys are down, false otherwise</returns>
		public static bool AreKeysDown(params Keys[] keys) {
			foreach (Keys key in keys) {
				if (!IsKeyDown(key))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Checks if a number of keys are currently up
		/// </summary>
		/// <param name="keys">The keys we want to check</param>
		/// <returns>True if all keys are up, false otherwise</returns>
		public static bool AreKeysUp(params Keys[] keys) {
			foreach (Keys key in keys) {
				if (IsKeyDown(key))
					return false;
			}
			return true;
		}


		/// <summary>
		/// Checks if any one of a number of keys are currently down
		/// </summary>
		/// <param name="keys">The keys we want to check</param>
		/// <returns>True if any key is down, false otherwise</returns>
		public static bool AnyKeysDown(params Keys[] keys) {
			foreach (Keys key in keys) {
				if (IsKeyDown(key))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Checks if any one number of keys are currently up
		/// </summary>
		/// <param name="keys">The keys we want to check</param>
		/// <returns>True if any key is  up, false otherwise</returns>
		public static bool AnyKeysUp(params Keys[] keys) {
			foreach (Keys key in keys) {
				if (!IsKeyDown(key))
					return true;
			}
			return false;
		}


		/// <summary>
		/// Checks if a number of keys have just been pressed in this update cycle
		/// </summary>
		/// <param name="keys">The keys we want to check</param>
		/// <returns>True if all keys are pressed, false otherwise</returns>
		public static bool AreKeysPressed(params Keys[] keys) {
            bool result = true;

			foreach (Keys key in keys) {
				result = result && IsKeyPress(key);
			}
			return result;
		}

		/// <summary>
		/// Checks if a number of keys have just been released in this update cycle
		/// </summary>
		/// <param name="keys">The keys we want to check</param>
		/// <returns>True if all keys are released, false otherwise</returns>
		public static bool AreKeysReleased(params Keys[] keys) {
			bool result = true;

			foreach (Keys key in keys) {
				result = result && IsKeyRelease(key);
			}
			return result;
		}

        /// <summary>
        /// Checks if any key is down
        /// </summary>
        /// <returns>True if a key is down, false otherwise</returns>
		public static bool AnyKeyDown() {
			return durations.Skip (1).Sum (x => x.Value) > 0;
		}
			
        /// <summary>
        /// Checks if a key has been down for more than a duration
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <param name="duration">Duration in seconds</param>
        /// <returns>True if key has been down for more than duration, false otherwise</returns>
		public static bool IsKeyDownFor(Keys key, double duration) { 
			return durations [key] > duration;
		}

        /// <summary>
        /// Checks if a key is up
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key is up, false otherwise</returns>
		public static bool IsKeyUp(Keys key) {
			return currentKeyboardState.IsKeyUp (key);
		}

        /// <summary>
        /// Checks if a key has just been pressed in this update cycle
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if key has been pressed this update cycle, false otherwise</returns>
		public static bool IsKeyPress(Keys key) {
			return currentKeyboardState.IsKeyDown (key) && previousKeyboardState.IsKeyUp (key);
		}

		/// <summary>
        /// Checks if a key has just been released in this update cycle
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if key has been released this update cycle, false otherwise</returns>
        public static bool IsKeyRelease(Keys key) {
			return currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key);
		}

		/// <summary>
        /// Called at the start of every update cycle to determine the keyboard state
        /// </summary>
        /// <param name="gameTime">Game time</param>
		public static void BeginUpdate(GameTime gameTime) {
			currentKeyboardState = Keyboard.GetState();

			// Iterate through all possible keys and put durations into dictionary
			foreach (Keys key in Enum.GetValues(typeof(Keys)).Cast<Keys>()) {
				// Ignore the option for where Key is None
				if (key != Keys.None) { 
					if (currentKeyboardState.IsKeyDown (key)) {
						durations [key] += CurrentGame.GetDelta(gameTime);
					} else
						durations [key] = 0;
				}
			}
		}

		/// <summary>
		/// Called at the end of every update cycle, to store the previous keyboard state
		/// </summary>
		public static void EndUpdate() {
			previousKeyboardState = currentKeyboardState;
		}
	}
}

