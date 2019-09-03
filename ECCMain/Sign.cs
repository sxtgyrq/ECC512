using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECCMain
{
    public class Sign
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        public static BigInteger[] GenerateSignature(BigInteger privateKey, byte[] hash)
        {
            BigInteger? k = null;
            for (int i = 0; i < 100; i++)
            {

                byte[] kBytes = new byte[33];
                rngCsp.GetBytes(kBytes);
                kBytes[32] = 0;

                k = new BigInteger(kBytes);
                var z = Bytes32.ConvetToBigInteger(hash);

                if (k.Value.IsZero || k >= Secp256k1.q) continue;

                var r = Calculate.getPublicByPrivate(k.Value)[0] % Secp256k1.q;

                if (r.IsZero) continue;

                var ss = (z + r * privateKey);
                var s = (ss * (k.Value.ModInverse(Secp256k1.q))) % Secp256k1.q;

                if (s.IsZero) continue;

                return new BigInteger[] { r, s };
            }

            throw new Exception("Unable to generate signature");
        }

        public static bool VerifySignature(BigInteger[] publicKey, byte[] hash, BigInteger r, BigInteger s)
        {
            if (r >= Secp256k1.q || r.IsZero || s >= Secp256k1.q || s.IsZero)
            {
                return false;
            }

            var z = Bytes32.ConvetToBigInteger(hash); ;
            var w = s.ModInverse(Secp256k1.q);
            var u1 = (z * w) % Secp256k1.q;
            var u2 = (r * w) % Secp256k1.q;
            bool isZero;
            var pt = Calculate.pointPlus(Calculate.getPublicByPrivate(u1), Calculate.getMulValue(u2, publicKey), out isZero);// (publicKey.Multiply(u2));

            if (pt == null)
            {
                return false;
            }
            else
            {
                var pmod = pt[0] % Secp256k1.q;

                return pmod == r;
            }
        }
    }

    public static class SignM
    {
        public static BigInteger ModInverse(this BigInteger n, BigInteger p)
        {
            BigInteger x = 1;
            BigInteger y = 0;
            BigInteger a = p;
            BigInteger b = n;

            while (b != 0)
            {
                BigInteger t = b;
                BigInteger q = BigInteger.Divide(a, t);
                b = a - q * t;
                a = t;
                t = x;
                x = y - q * t;
                y = t;
            }

            if (y < 0)
                return y + p;
            //else
            return y;
        }
    }
}
