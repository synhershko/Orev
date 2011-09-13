using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orev.Helpers
{
	public static class RavenHelpers
	{
		public static int ToIntId(this string id)
		{
			return int.Parse(id.Substring(id.LastIndexOf('/') + 1));
		}
	}
}