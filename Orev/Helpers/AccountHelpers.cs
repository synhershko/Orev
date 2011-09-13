using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orev.Models;
using Raven.Client;

namespace Orev.Helpers
{
	public static class AccountHelpers
	{
		public static User GetUser(this IDocumentSession session, string login)
		{
			return session.Load<User>("users/" + login);
		}
	}
}