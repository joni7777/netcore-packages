using System;

namespace Bp.Common.Extensions
{
	public static class DateTimeExtensions
	{
		public static DateTime StartOfDay(this DateTime @this)
		{
			return @this.Date;
		}
		
		public static DateTime EndOfDay(this DateTime @this)
		{
			return @this.Date.AddDays(1).AddTicks(-1);
		}
	}
}