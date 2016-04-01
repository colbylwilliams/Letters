using Foundation;
using UIKit;

using Letters.Unified;

namespace Letters.iOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window { get; set; }

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			Bootstrap.Run ();

			return true;
		}
	}
}