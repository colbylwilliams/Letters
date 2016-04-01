namespace Letters
{
	public interface ISettings
	{
		#region Utilities

		void RegisterDefaultSettings ();

		#endregion


		#region About

		string VersionNumber { get; }

		string BuildNumber { get; }

		string VersionBuildString { get; }

		#endregion


		#region Configuration

		double LetterDrawDuration { get; set; }

		int LetterDrawDelay { get; set; }

		#endregion


		#region Internal

		bool FirstLaunch { get; }

		#endregion


		#region Reporting

		string UserReferenceKey { get; }

		#endregion
	}
}