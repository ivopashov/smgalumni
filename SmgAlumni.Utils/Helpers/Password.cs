﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace SmgAlumni.Utils.Helpers
{
    public static class Password
    {
        public static string CreateSalt(int length = 64)
        {
            var randomArray = new byte[length];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);
            var randomString = Convert.ToBase64String(randomArray);
            return randomString;
        }

        public static string HashPassword(string password)
        {
            var hasher = new SHA512Managed();
            var hashedBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
