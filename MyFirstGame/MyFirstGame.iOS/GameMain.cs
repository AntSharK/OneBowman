using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;


namespace MyFirstGame.iOS
{
	[Register ("AppDelegate")]
	class Program : UIApplicationDelegate 
	{
		private GameRunner game;

		public override void FinishedLaunching (UIApplication app)
		{
			// Fun begins..
			game = new GameRunner();
			game.Run();
		}

		static void Main (string [] args)
		{
			UIApplication.Main (args,null,"AppDelegate");
		}
	}
}
