using System;
using System.Text;

namespace Nuglet.Extensions
{
	internal static class StringBuilderExtensions
	{
		public static void RemoveTrailing(this StringBuilder stringBuilder, string str)
		{
			var count = 0;
			var chars = str.AsSpan();
			for (int i = stringBuilder.Length - chars.Length; i >= 0; i -= chars.Length)
			{
				var bk = false;
				for (int j = chars.Length - 1; j >= 0; --j)
				{
					if (stringBuilder[i + j] != chars[j])
					{
						bk = true;
						break;
					}
				}

				if (bk)
				{
					break;
				}

				count += chars.Length;
			}

			if (stringBuilder.Length >= count && count > 0)
			{
				stringBuilder.Length -= count;
			}
		}
	}
}
