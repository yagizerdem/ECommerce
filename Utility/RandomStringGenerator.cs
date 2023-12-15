using Microsoft.AspNetCore.Mvc.Infrastructure;
using RandomString4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class RandomStringGenerator
    {
        public static string Generate()
        {
            string result = "";
            List<string> randomAlphabetWithCustomSymbols = RandomString.GetStrings(Types.ALPHANUMERIC_MIXEDCASE_WITH_SYMBOLS, 100, "/+*-", forceOccuranceOfEachType: true);
            foreach(var item in randomAlphabetWithCustomSymbols)
            {
                result += item;
            }
            return result;
        }

        public static string GenerateMini()
        {
            return  RandomString.GetString(Types.ALPHABET_LOWERCASE);
        }
    }
}
