using System;
using System.IO;
using Foundation;

using static Foundation.NSUserDefaults;

namespace Letters.Unified
{
	public static class Settings// : ISettings
	{
		#region Utilities

#if __IOS__

		public static void RegisterDefaultSettings ()
		{
			var path = Path.Combine (NSBundle.MainBundle.PathForResource ("Settings", "bundle"), "Root.plist");

			using (NSString keyString = new NSString ("Key"), defaultString = new NSString ("DefaultValue"), preferenceSpecifiers = new NSString ("PreferenceSpecifiers"))
			using (var settings = NSDictionary.FromFile (path))
			using (var preferences = (NSArray)settings.ValueForKey (preferenceSpecifiers))
			using (var registrationDictionary = new NSMutableDictionary ()) {
				for (nuint i = 0; i < preferences.Count; i++)
					using (var prefSpecification = preferences.GetItem<NSDictionary> (i))
					using (var key = (NSString)prefSpecification.ValueForKey (keyString))
						if (key != null)
							using (var def = prefSpecification.ValueForKey (defaultString))
								if (def != null)
									registrationDictionary.SetValueForKey (def, key);

				StandardUserDefaults.RegisterDefaults (registrationDictionary);

				Synchronize ();
			}
		}

#else

		public void RegisterDefaultSettings ()
		{
			//SetSetting ("ApplePressAndHoldEnabled", true);

			//Synchronize ();
		}

#endif

		public static void Synchronize () => StandardUserDefaults.Synchronize ();

		public static void SetSetting (string key, string value) => StandardUserDefaults.SetString (value, key);

		public static void SetSetting (string key, bool value) => StandardUserDefaults.SetBool (value, key);

		public static void SetSetting (string key, int value) => StandardUserDefaults.SetInt (value, key);

		public static void SetSetting (string key, double value) => StandardUserDefaults.SetDouble (value, key);

		public static void SetSetting (string key, DateTime value) => SetSetting (key, value.ToString ());

		// public static void SetSetting (string key, IDictionary<string, string> value) => SetSetting (key, value.ToJson ());

		public static int Int32ForKey (string key) => Convert.ToInt32 (StandardUserDefaults.IntForKey (key));

		public static double DoubleForKey (string key) => StandardUserDefaults.DoubleForKey (key);

		public static bool BoolForKey (string key) => StandardUserDefaults.BoolForKey (key);

		public static string StringForKey (string key) => StandardUserDefaults.StringForKey (key);

		public static DateTime DateTimeForKey (string key)
		{
			DateTime outDateTime;

			return DateTime.TryParse (StandardUserDefaults.StringForKey (key), out outDateTime) ? outDateTime : DateTime.MinValue;
		}

		//public static IDictionary<string, string> DictionaryForKey (string key) => JsonObject.Parse (StringForKey (key));

		#endregion


		#region About

		public static string VersionNumber => StringForKey (SettingsKeys.VersionNumber);

		public static string BuildNumber => StringForKey (SettingsKeys.BuildNumber);

		public static string VersionBuildString => $"v{VersionNumber} b{BuildNumber}";

		#endregion


		#region Configuration

		public static double LetterDrawDuration {
			get {
				var letterDrawDuration = DoubleForKey (SettingsKeys.LetterDrawDuration);

				if (letterDrawDuration < 1) {

					SetSetting (SettingsKeys.LetterDrawDuration, 3.0);

					return 3.0;
				}

				return letterDrawDuration;
			}
			set {
				SetSetting (SettingsKeys.LetterDrawDuration, value);
			}
		}

		public static int LetterDrawDelay {
			get {
				var letterDrawDuration = Int32ForKey (SettingsKeys.LetterDrawDelay);

				if (letterDrawDuration < 1) {

					SetSetting (SettingsKeys.LetterDrawDelay, 3);

					return 3;
				}

				return letterDrawDuration;
			}
			set {
				SetSetting (SettingsKeys.LetterDrawDelay, value);
			}
		}

		public static bool LetterDrawLoop {
			get {
				return BoolForKey (SettingsKeys.LetterDrawLoop);
			}
			set {
				SetSetting (SettingsKeys.LetterDrawLoop, value);
			}
		}

		#endregion


		#region Internal

		public static bool FirstLaunch {
			get {
				// this is actually false if it's the first time the app is launched
				var firstL = !BoolForKey (SettingsKeys.FirstLaunch);

				if (firstL) {
					SetSetting (SettingsKeys.FirstLaunch, true);
				}

				return firstL;
			}
		}

		#endregion


		#region Reporting

		public static string UserReferenceKey {
			get {
				var key = StringForKey (SettingsKeys.UserReferenceKey);

				if (string.IsNullOrEmpty (key)) {

#if DEBUG
					key = "DEBUG";
#else
					key = "ANONYMOUS";
#endif
					SetSetting (SettingsKeys.UserReferenceKey, key);
				}

				return key;
			}
		}

		#endregion
	}
}