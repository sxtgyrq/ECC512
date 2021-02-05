using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECCMain
{
    public class Secp256k1
    {
        public static System.Numerics.BigInteger p = ECCMain.HexToBigInteger.inputHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F");
        public static System.Numerics.BigInteger a = ECCMain.HexToBigInteger.inputHex("0");
        public static System.Numerics.BigInteger b = ECCMain.HexToBigInteger.inputHex("7");
        public static System.Numerics.BigInteger x = ECCMain.HexToBigInteger.inputHex("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798");
        public static System.Numerics.BigInteger y = ECCMain.HexToBigInteger.inputHex("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8");
        public static System.Numerics.BigInteger q = ECCMain.HexToBigInteger.inputHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141");
        //// public class UnCompress
        // {

        // }

        //public static System.Numerics.BigInteger p = new BigInteger(17);
        //public static System.Numerics.BigInteger a = new BigInteger(2);
        //public static System.Numerics.BigInteger b = new BigInteger(2);
        //public static System.Numerics.BigInteger x = new BigInteger(5);
        //public static System.Numerics.BigInteger y = new BigInteger(1);
        //public static System.Numerics.BigInteger q = new BigInteger(19);

    }
}
