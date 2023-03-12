using NBitcoin;
using NBitcoin.Crypto;
using System.Net;
using System.Security.Cryptography;
using static NBitcoin.RPC.SignRawTransactionRequest;

namespace OtherBitCoinLibrary
{
    public class CLTV
    {
        static Network NetworkForTest = NBitcoin.Network.TestNet;

        public static void GetHex4(string privateKeyStr, uint lockTime)
        {
            var netWork = NBitcoin.Network.Main;
            var privateKey = new BitcoinSecret(privateKeyStr, netWork);
            Console.WriteLine($"privateKeyHex{Convert.ToHexString(privateKey.ToBytes())}");
            var address = privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, netWork);
            // n4dEJzZxe4Uguup1SRGF83M8ko6nBV73Ec
            Console.WriteLine($"p2pkh地址为：{address}");

            var publicKey = privateKey.PubKey;


            //HASH160Hex fd7ac2296b8600dcbc864e54d6497f8e30940cd0
            Console.WriteLine($"HASH160Hex {publicKey.Hash.ToString()}");

            var operateLockTime = lockTime;
            //string blockIndexCanLockHex = "";
            List<byte> blockIndexCanUse = new List<byte>();
            for (int indexOfLoop = 0; indexOfLoop < 3; indexOfLoop++)
            {
                var byteValue = Convert.ToByte(operateLockTime % 256);
                blockIndexCanUse.Add(byteValue);
                operateLockTime /= 256;
            }
            var redeemScriptHexStr = $"03{Convert.ToHexString(blockIndexCanUse.ToArray()).ToLower()}b17576a914{Convert.ToHexString(publicKey.Hash.ToBytes()).ToLower()}88ac";
            Console.WriteLine($"redeemScript is:{redeemScriptHexStr}");
            NBitcoin.Script script = new NBitcoin.Script(redeemScriptHexStr);
            Console.WriteLine($"{script.ToString()}");
            //Redeem Script Asm	03<20830c> OP_CHECKLOCKTIMEVERIFY OP_DROP OP_DUP OP_HASH160 14<fd7ac2296b8600dcbc864e54d6497f8e30940cd0> OP_EQUALVERIFY OP_CHECKSIG


            Script redeemScrip = new Script(Convert.FromHexString(redeemScriptHexStr));

            // if (inputScript != null)
            {
                BitcoinAddress? resultAddress = redeemScrip.Hash.GetAddress(netWork);
                Console.WriteLine($"inputScript-hex:{redeemScrip.ToHex()}");
                if (resultAddress != null)
                {
                    Console.WriteLine($"NBitcoinResult:{resultAddress.ToString()}");
                }
            }
            Console.WriteLine($"https://www.btcschools.net/bitcoin/bitcoin_script_p2sh_locktime.php");

        }

        public static void Test()
        {
            var privateKeyStr = "cMmmWc5ZptX5TwsZjw32F7KTBkLFFXJZRVfzeXLq9eoNyuSiQLY5";


            const int lockTime = 820000;

            var privateKey = new BitcoinSecret(privateKeyStr, NetworkForTest);
            Console.WriteLine($"privateKeyHex{Convert.ToHexString(privateKey.ToBytes())}");
            var address = privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, NetworkForTest);
            // n4dEJzZxe4Uguup1SRGF83M8ko6nBV73Ec
            Console.WriteLine($"p2pkh地址为：{address}");

            var publicKey = privateKey.PubKey;


            //HASH160Hex fd7ac2296b8600dcbc864e54d6497f8e30940cd0
            Console.WriteLine($"HASH160Hex {publicKey.Hash.ToString()}");

            var operateLockTime = lockTime;
            //string blockIndexCanLockHex = "";
            List<byte> blockIndexCanUse = new List<byte>();
            for (int indexOfLoop = 0; indexOfLoop < 3; indexOfLoop++)
            {
                var byteValue = Convert.ToByte(operateLockTime % 256);
                blockIndexCanUse.Add(byteValue);
                operateLockTime /= 256;
            }
            var redeemScriptHexStr = $"03{Convert.ToHexString(blockIndexCanUse.ToArray()).ToLower()}b17576a914{Convert.ToHexString(publicKey.Hash.ToBytes()).ToLower()}88ac";
            Console.WriteLine($"redeemScript is:{redeemScriptHexStr}");

            //Redeem Script Asm	03<20830c> OP_CHECKLOCKTIMEVERIFY OP_DROP OP_DUP OP_HASH160 14<fd7ac2296b8600dcbc864e54d6497f8e30940cd0> OP_EQUALVERIFY OP_CHECKSIG

            //https://www.btcschools.net/bitcoin/bitcoin_script_p2sh_locktime.php


            Script redeemScrip = new Script(Convert.FromHexString(redeemScriptHexStr));

            // if (inputScript != null)
            {
                BitcoinAddress? resultAddress = redeemScrip.Hash.GetAddress(NetworkForTest);
                Console.WriteLine($"inputScript-hex:{redeemScrip.ToHex()}");
                if (resultAddress != null)
                {
                    Console.WriteLine($"NBitcoinResult:{resultAddress.ToString()}");
                }
            }
            Sign2(NetworkForTest, privateKey, redeemScrip);

        }

        //static string Sign(Network network, BitcoinSecret privateKey, Script redeemScript)
        //{
        //    //var network = Network.Main;

        //    // 替换为你的私钥和赎回脚本
        //    // var privateKey = new Key(); // 你的私钥
        //    //  var redeemScript = new Script("你的赎回脚本");

        //    // 创建一个支付到脚本哈希地址的交易输出（UTXO）
        //    var p2shAddress = redeemScript.GetDestinationAddress(network);
        //    var sourceTransactionId = uint256.Parse("95adc26ad6f34b8113296235b658830f8afa013c64549e01dbd59e497efed747");
        //    var sourceTransactionIndex = 0; // UTXO的输出索引

        //    // 构建交易
        //    var transaction = Transaction.Create(network);
        //    transaction.Inputs.Add(new TxIn(new OutPoint(sourceTransactionId, sourceTransactionIndex)));

        //    // 添加输出 - 替换为目的地址和金额
        //    var destinationAddress = BitcoinAddress.Create("n4dEJzZxe4Uguup1SRGF83M8ko6nBV73Ec", network);
        //    transaction.Outputs.Add(new TxOut(new Money(1434, MoneyUnit.Satoshi), destinationAddress));



        //    var scriptCoin = new ScriptCoin(new Coin(sourceTransactionId, (uint)sourceTransactionIndex, Money.Satoshis(1834), redeemScript.PaymentScript), redeemScript);

        //    var builder = network.CreateTransactionBuilder();
        //    //var signer = ;
        //    var signedTx = builder
        //        .AddCoins(scriptCoin)
        //        .AddKeys(privateKey)
        //        .SignTransaction(transaction);

        //    // 手动处理脚本
        //    var txIn = signedTx.Inputs[0];
        //    var sig = transaction.SignInput(privateKey, scriptCoin);
        //    // if (signedTx != null)
        //    // var sig = 
        //    signedTx.Sign(privateKey, scriptCoin);
        //    var p2shSig = PayToScriptHashTemplate.Instance.GenerateScriptSig(signedTx.Inp, privateKey.PubKey, redeemScript);
        //    txIn.ScriptSig = p2shSig;

        //    // 广播交易（这里只是打印交易的十六进制表示）
        //    Console.WriteLine(transaction.ToHex());
        //    return transaction.ToHex();
        //}

        static string Sign2(Network network, BitcoinSecret privateKey, Script redeemScript)
        {
            return "";
            // return signedTransaction.ToHex();
            //  Console.WriteLine($"Transaction is verified: {isVerified}");
        }
    }
}
