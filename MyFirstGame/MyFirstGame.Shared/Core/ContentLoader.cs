using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;


namespace MyFirstGame
{
	class ContentLoader
	{
		public static ContentManager content;
		public static Dictionary<string, Texture2D> texturePool = new Dictionary<string, Texture2D>();
		public static Dictionary<string, SoundEffect> soundEffectPool = new Dictionary<string, SoundEffect>();

		// Loads and returns texture if not in dictionary; otherwise, returns already loaded texture
		public static Texture2D GetTexture(string assetName)
		{
			if (texturePool.Keys.Contains(assetName))
			{
				return texturePool[assetName];
			}
			Texture2D texture;
			try
			{
				texture = content.Load<Texture2D>("Images/" + assetName);
			}
			catch (Exception e)
			{
				//Console.WriteLine(e.Message);
				return null;
			}
			texturePool[assetName] = texture;
			return texture;
		}

		// Loads and returns texture if not in dictionary; otherwise, returns already loaded texture
		public static SoundEffect GetSoundEffect(string assetName)
		{
			if (soundEffectPool.Keys.Contains(assetName))
			{
				return soundEffectPool[assetName];
			}
			SoundEffect soundEffect;
			try
			{
				soundEffect = content.Load<SoundEffect>("Sounds/" + assetName);
			}
			catch (Exception e)
			{
				//Console.WriteLine(e.Message);
				return null;
			}
			soundEffectPool[assetName] = soundEffect;
			return soundEffect;
		}

		public static SpriteFont LoadFont(string assetName)
		{
			return content.Load<SpriteFont>(assetName);

		}


	}
}