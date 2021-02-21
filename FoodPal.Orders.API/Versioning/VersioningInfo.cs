using System.Reflection;

namespace FoodPal.Orders.API.Versioning
{
	/// <summary>
	/// API Versioning utility class.
	/// </summary>
	public class VersioningInfo
	{
		private const int DefaultVersionNumber = 0;

		/// <summary>
		/// Major version.
		/// </summary>
		public static int MajorVersion => Assembly.GetExecutingAssembly().GetName().Version?.Major ?? DefaultVersionNumber;
	}
}
