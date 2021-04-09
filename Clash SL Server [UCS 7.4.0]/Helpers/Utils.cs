using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CSS.Core;
using CSS.Files.Logic;
using CSS.Logic;
using CSS.Utilities.ZLib;

namespace CSS.Helpers
{
    internal static class GamePlayUtil
    {
        public static int CalculateResourceCost(int sup, int inf, int supCost, int infCost, int amount) =>
            (int)Math.Round((supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;

        public static int CalculateSpeedUpCost(int sup, int inf, int supCost, int infCost, int amount) =>
            (int)Math.Round((supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;

        public static int GetResourceDiamondCost(int resourceCount, ResourceData resourceData) =>
            Globals.GetResourceDiamondCost(resourceCount, resourceData);

        public static int GetSpeedUpCost(int seconds) =>
            Globals.GetSpeedUpCost(seconds);
    }

    internal static class Utils
    {
        public static string Padding(string _String, int _Limit = 23)
        {
            if (_String.Length > _Limit)
            {
                _String = _String.Remove(_String.Length - (_String.Length - _Limit + 3), _String.Length - _Limit + 3) + "...";
            }
            else if (_String.Length < _Limit)
            {
                int _Length = _Limit - _String.Length;

                for (int i = 0; i < _Length; i++)
                {
                    _String += " ";
                }
            }

            return _String;
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static byte[] CreateRandomByteArray()
        {
            byte[] buffer = new byte[Resources.Random.Next(20)];
            Resources.Random.NextBytes(buffer);
            return buffer;
        }

        public static void Increment(this byte[] nonce, int timesToIncrease = 2)
        {
            for (int j = 0; j < timesToIncrease; j++)
            {
                ushort c = 1;
                for (UInt32 i = 0; i < nonce.Length; i++)
                {
                    c += (ushort)nonce[i];
                    nonce[i] = (byte)c;
                    c >>= 8;
                }
            }
        }
        public static int ParseConfigInt(string str) => int.Parse(ConfigurationManager.AppSettings[str]);

        public static bool ParseConfigBoolean(string str) => Boolean.Parse(ConfigurationManager.AppSettings[str]);

        public static string ParseConfigString(string str) => ConfigurationManager.AppSettings[str];

        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key)
        {
            TValue ignored;
            return self.TryRemove(key, out ignored);
        }

        public static byte[] ToBytes(this string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public static bool Contains(this String str, String substring, StringComparison comp)
        {
            if (substring == null)
                throw new ArgumentNullException("substring",
                                                "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
                throw new ArgumentException("comp is not a member of StringComparison",
                                            "comp");

            return str.IndexOf(substring, comp) >= 0;
        }
    }
}
