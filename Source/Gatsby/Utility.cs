using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gatsby
{
	public static class Utility
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		/// <remarks>Credit to Joan from StackOverflow: http://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c </remarks>
		public static string UrlSlug(string url)
		{
			//First to lower case
			url = url.ToLowerInvariant();

			//Remove all accents
			var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(url);
			url = Encoding.ASCII.GetString(bytes);

			//Replace spaces
			url = Regex.Replace(url, @"\s", "-", RegexOptions.Compiled);

			//Remove invalid chars
			url = Regex.Replace(url, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

			//Trim dashes from end
			url = url.Trim('-', '_');

			//Replace double occurences of - or _
			url = Regex.Replace(url, @"([-_]){2,}", "$1", RegexOptions.Compiled);

			return url;
		}
	}
}
