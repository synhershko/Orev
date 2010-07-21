using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orev.Helpers;

namespace Orev.Domain
{
    public class User : IOrevEntity
    {
        protected static readonly byte SaltLength = 5;

        // TODO: User roles, user status

        public virtual int Id { get; set; }
        public virtual String Email { get; set; }

        /// <summary>
        /// Hashed password, can only be set by consumers - use SetPassword(String) to do that
        /// </summary>
        public virtual String Password { get; set; }
        public virtual String Salt { get; set; }

        public virtual void SetPassword(String plainTextPwd)
        {
            Salt = Randomizer.GetRandomString(5, true, true);
            Password = Encryption.HashString(Salt + plainTextPwd);
        }

        public virtual bool Authenticate(String plainTextPwd)
        {
            String hashedPwd = Encryption.HashString(Salt + plainTextPwd);
            return Password.CompareTo(hashedPwd) == 0;
        }

        public virtual DateTime Registered { get; set; }
        public virtual DateTime LastSeen { get; set; }
    }
}
