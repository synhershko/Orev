using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orev.Helpers
{
    public static class Encryption
    {
        public static string HashString(string str)
        {
            string rethash = "";
            try
            {
                System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
                System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
                byte[] combined = encoder.GetBytes(str);
                hash.ComputeHash(combined);
                rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in HashCode : " + ex.Message);
            }
            return rethash;
        }
    }
}
