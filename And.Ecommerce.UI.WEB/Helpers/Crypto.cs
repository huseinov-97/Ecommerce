﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace And.Ecommerce.UI.WEB.Helpers
{
    public static class Crypto
    {
        public static string Hash(string password)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create().
                ComputeHash(Encoding.UTF8.GetBytes(password))
                );
        }
    }
}