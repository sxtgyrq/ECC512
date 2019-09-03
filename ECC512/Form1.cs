using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace ECC512
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //var a = new BigInteger(2);

            //{
            //    var b = BigInteger.Pow(a, 10);

            //    MessageBox.Show(b.ToString());
            //}
            //{
            //    var b = BigInteger.Pow(a, 512);

            //    MessageBox.Show(b.ToString());

            //    var c = HexToBigInteger.inputHex("AADD9DB8DBE9C48B3FD4E6AE33C9FC07CB308DB3B3C9D20ED6639CCA703308717D4D9B009BC66842AECDA12AE6A380E62881FF2F2D82C68528AA6056583A48F3")
            //    //BigInteger P = BigInteger.p "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F".HexToBigInteger();
            //}
            //       Curve - ID: brainpoolP512r1

            //p = AADD9DB8DBE9C48B3FD4E6AE33C9FC07CB308DB3B3C9D20ED6639CCA703308
            // 717D4D9B009BC66842AECDA12AE6A380E62881FF2F2D82C68528AA6056583A48F3

            // A = 7830A3318B603B89E2327145AC234CC594CBDD8D3DF91610A83441CAEA9863
            // BC2DED5D5AA8253AA10A2EF1C98B9AC8B57F1117A72BF2C7B9E7C1AC4D77FC94CA

            // B = 3DF91610A83441CAEA9863BC2DED5D5AA8253AA10A2EF1C98B9AC8B57F1117
            // A72BF2C7B9E7C1AC4D77FC94CADC083E67984050B75EBAE5DD2809BD638016F723

            // x = 81AEE4BDD82ED9645A21322E9C4C6A9385ED9F70B5D916C1B43B62EEF4D009
            // 8EFF3B1F78E2D0D48D50D1687B93B97D5F7C6D5047406A5E688B352209BCB9F822

            // y = 7DDE385D566332ECC0EABFA9CF7822FDF209F70024A57B1AA000C55B881F81
            // 11B2DCDE494A5F485E5BCA4BD88A2763AED1CA2B2FA8F0540678CD1E0F3AD80892

            // q = AADD9DB8DBE9C48B3FD4E6AE33C9FC07CB308DB3B3C9D20ED6639CCA703308
            // 70553E5C414CA92619418661197FAC10471DB1D381085DDADDB58796829CA90069

            // h = 1


            //     public static readonly BigInteger P = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F".HexToBigInteger();
            //public static readonly ECPoint G = ECPoint.DecodePoint("0479BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8".HexToBytes());
            //public static readonly BigInteger N = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141".HexToBigInteger();
            //var a = BigInteger.Parse("934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952");
            //    var b = BigInteger.Parse("934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952934157136952");
            //    var c = a + b;
            //    var d = a * b;
            //    MessageBox.Show(c.ToString());
            //    MessageBox.Show(c.ToString().Length.ToString());

            //    MessageBox.Show(d.ToString());
            //    MessageBox.Show(d.ToString().Length.ToString());
            //  BigInteger.Parse(largeNumber);
        }


        //BigInteger extended_gcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        //{
        //    if (b == 0)
        //    {
        //        x = 1;
        //        y = 0;
        //        return a;
        //    }
        //    else
        //    {
        //        BigInteger gcd = extended_gcd(b, a % b, out x, out y);
        //        int t = x % mod;
        //        x = y % mod;
        //        y = ((t - a / b * x) % mod + mod) % mod;
        //        return gcd;
        //    }
        //}
        //void exgcd(BigInteger a, BigInteger b, ref BigInteger d, ll& x, ll& y)
        //{
        //    if (!b) { d = a; x = 1; y = 0; }
        //    else { exgcd(b, a % b, d, y, x); y -= x * (a / b); }
        //}

        BigInteger ex_gcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            BigInteger ret, tmp;
            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }
            ret = ex_gcd(b, a % b, out x, out y);
            tmp = x;
            x = y;
            y = tmp - a / b * y;
            return ret;
        }
    }
}
