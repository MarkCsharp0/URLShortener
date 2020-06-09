using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortener.Model;

namespace URLShortener.Util
{
    public static class UtilLinks
    {
        public static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly int Base = Alphabet.Length;
       
        public static string Encode(int i)
        {
            if (i == 0) return Alphabet[0].ToString();

            var s = string.Empty;

            while (i > 0)
            {
                s += Alphabet[i % Base];
                i = i / Base;
            }

            return string.Join(string.Empty, s.Reverse());
        }

        public static int Decode(string s)
        {
            var i = 0;

            foreach (var c in s)
            {
                i = (i * Base) + Alphabet.IndexOf(c);
            }

            return i;
        }

    

        public static string GetShortLink(int ID)
        {
            return Encode(ID) + ".ly";

        }

        public static double GetDateDifference(DateTime currentTime, DateTime dateOfCreation)
        {
            var res = currentTime - dateOfCreation;
            return res.TotalMinutes;
        }

    }
}
