using System;
using System.Security.Cryptography;

namespace Kronos.Helpers
{
    public class Dice
    {
        private static Random r;

        static Dice()
        {
            var generator = RandomNumberGenerator.Create();
            byte[] b = new byte[4];

            generator.GetBytes(b);
            r = new Random(BitConverter.ToInt32(b, 0));
        }

        public static int Next(int value)
        {
            return r.Next(value);
        }

        public static int Next(int minValue, int maxValue)
        {
            return r.Next(minValue, maxValue);
        }
    }
}
