using System.Text.RegularExpressions;

namespace Bp.Common.Extensions
{
	public static class StringExtensions
	{
		public static string ToSnakeCase(this string @this)
		{
			if (string.IsNullOrEmpty(@this))
			{
				return @this;
			}

			return Regex.Match(@this, "^_+") + Regex.Replace(@this, "([a-z0-9])([A-Z])", "$1_$2").ToLower();
		}
		
		public static string ToKebabCase(this string @this)
		{
			if (string.IsNullOrEmpty(@this))
			{
				return @this;
			}

			return Regex.Match(@this, "^-+") + Regex.Replace(@this, "([a-z0-9])([A-Z])", "$1-$2").ToLower();
		}
	}
}