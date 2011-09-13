using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Orev.Models
{
	public class User
	{
		private const string ConstantSalt = "t2fy^cerv3f4#";

		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		protected string HashedPassword { get; private set; }
		public bool Enabled { get; set; }

		public DateTime DateJoined { get; set; }
		public DateTime LastSeen { get; set; }

		private Guid _passwordSalt;
		private Guid PasswordSalt
		{
			get
			{
				if (_passwordSalt == Guid.Empty)
					_passwordSalt = Guid.NewGuid();
				return _passwordSalt;
			}
			set { _passwordSalt = value; }
		}

		public User SetPassword(string pwd)
		{
			HashedPassword = GetHashedPassword(pwd);
			return this;
		}

		private string GetHashedPassword(string pwd)
		{
			string hashedPassword;
			using (var sha = SHA256.Create())
			{
				var computedHash = sha.ComputeHash(
					PasswordSalt.ToByteArray().Concat(
						Encoding.Unicode.GetBytes(PasswordSalt + pwd + ConstantSalt)
						).ToArray()
					);

				hashedPassword = Convert.ToBase64String(computedHash);
			}
			return hashedPassword;
		}

		public bool ValidatePassword(string maybePwd)
		{
			return HashedPassword == GetHashedPassword(maybePwd);
		}
	}
}