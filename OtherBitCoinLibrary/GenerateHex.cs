using NBitcoin.Protocol;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using System.Net;
using NBitcoin.Crypto;

namespace OtherBitCoinLibrary
{
    public class GenerateHex
    {
        static Network mainNet = Network.Main;
        public static void GetHex(string privateKeyStr, uint lockTime)
        {
            if (lockTime < 1)
            {
                Console.WriteLine($"lockTime 的值必须大于1");
                return;
            }
            var operateLockTime = lockTime;
            var privateKey = new BitcoinSecret(privateKeyStr, Network.Main);
            var network = privateKey.Network;
            //  var address = privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, network);

            var publicKey = privateKey.PubKey;
            //  privateKey.PubKeyHash

            List<byte> script = new List<byte>();
            script.Add(0x4c); // OP_PUSHDATA1 76
            script.Add(0x4); // 长度为4
            {
                /*
                 * 在比特币脚本中，当您使用 locktime 值，如数字 1，
                 * 并且它占用四个字节时，这个值在脚本中以小端序格
                 * 式表示。小端序意味着最低有效字节在前。因此，数
                 * 字 1 表示为四个字节时，将以如下格式出现：
                 * 01 00 00 00
                 * 数字 257 以四字节的小端序格式表示时会是这样的：
                 * 01 01 00 00
                 */
                for (int indexOfLoop = 0; indexOfLoop < 4; indexOfLoop++)
                {
                    script.Add(Convert.ToByte(operateLockTime % 256));
                    operateLockTime /= 256;
                }
            }
            //script.AddRange(locktimeBytes);             // 添加 locktime
            script.Add(0xb1);                           // OP_CHECKLOCKTIMEVERIFY 177
            script.Add(0x75);                           // OP_DROP
            script.Add(0x76);                           // OP_DUP
            script.Add(0xa9);                           // OP_HASH160
            script.AddRange(publicKey.Hash.ToBytes());        // 添加RIPEMD-160 哈希
            script.Add(0x88);                           // OP_EQUALVERIFY
            script.Add(0xac);

            Script redeemScript = new Script(script);

            //  redeemScript

            List<byte> script2 = new List<byte>();
            script2.Add(0xa9); //
            script2.Add(0x14); //
            script2.AddRange(redeemScript.Hash.ToBytes()); //
            script2.Add(0x87);  //
            Script scriptPubKey = new Script(script2);
            // 创建交易
            Transaction transaction = Transaction.Create(Network.Main); //network.CreateTransaction();

            //   Script customScript = new Script("您的自定义脚本");


            List<ICoin> coins = new List<ICoin>();
            while (true)
            {
                Console.WriteLine("输入end，表示结束输入;");
                Console.WriteLine("或者输入16进制，以0x开头的txID");
                // var input 
                var input = Console.ReadLine();
                if (input == null)
                {
                    return;
                }
                else if (input.ToLower() == "end")
                {
                    break;
                }
                string txHex01 = input;
                Console.WriteLine($"交易ID 为{input}");
                uint256 txID = new uint256(input);
                Console.WriteLine("序号(格式为十进制数字)");
                input = Console.ReadLine();
                if (input == null)
                {
                    return;
                }

                var outPoint = new OutPoint(txID, int.Parse(input));
                transaction.Inputs.Add(new TxIn(outPoint));

                // transaction.Inputs[transaction.Inputs.Count - 1].sc

                var fromTxID = txID;
                var fromOutputIndex = uint.Parse(input);
                Console.WriteLine($"有多少聪？输入整数！");
                input = Console.ReadLine();
                if (input == null)
                {
                    return;
                }
                int satoshi = int.Parse(input);
                var money = new Money(satoshi, MoneyUnit.Satoshi);
                var coin = new Coin(outPoint, new TxOut(money, scriptPubKey));
                coin.ScriptPubKey = scriptPubKey;
                coin.ToScriptCoin(redeemScript);
                // ScriptCoin scriptCoin = coin.ToScriptCoin(inputScript1);
                coins.Add(coin);
            }

            Console.WriteLine("开始添加输出，即转出的地址");

            while (true)
            {
                Console.WriteLine("输入end，表示结束输入;");
                Console.WriteLine("或者输入Base58格式的比特币地址");
                // var input 
                var input = Console.ReadLine();
                if (input == null)
                {
                    return;
                }
                else if (input.ToLower() == "end")
                {
                    break;
                }
                var destinationAddress = BitcoinAddress.Create(input, network);
                Console.WriteLine($"要给{input}转多少聪？输入整数！");
                input = Console.ReadLine();
                if (input == null)
                {
                    return;
                }
                int satoshi = int.Parse(input);
                transaction.Outputs.Add(new TxOut(new Money(satoshi, MoneyUnit.Satoshi), destinationAddress));
            }


            // Script script = new Script()

            for (int i = 0; i < transaction.Inputs.Count; i++)
            {

            }
            transaction.LockTime = lockTime + 10;

            transaction.Sign(privateKey, coins);
            //transaction.Sign()

            string txHex = transaction.ToHex();

            Console.WriteLine($"交易内容: {transaction.GetHash()}");
            Console.WriteLine($"交易内容: {txHex}");
            // 广播交易
            //using (var node = Node.Connect(network, "https://de.poiuty.com:50002"))
            //{
            //    node.VersionHandshake();
            //    node.SendMessage(new InvPayload(InventoryType.MSG_TX, transaction.GetHash()));
            //    node.SendMessage(new TxPayload(transaction));
            //    Thread.Sleep(500); // 等待一段时间以确保交易被发送
            //}

            Console.WriteLine($"交易广播成功: {transaction.GetHash()}");
        }

        public static void GetAddr(string privateKeyStr, uint lockTime)
        {
            if (lockTime < 1)
            {
                Console.WriteLine($"lockTime 的值必须大于1");
                return;
            }

            var privateKey = new BitcoinSecret(privateKeyStr, Network.Main);
            var network = privateKey.Network;
            var address = privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, network);

            var publicKey = privateKey.PubKey;


            List<byte> script = new List<byte>();
            script.Add(0x4c); // OP_PUSHDATA1 76
            script.Add(0x4); // 长度为4
            {
                /*
                 * 在比特币脚本中，当您使用 locktime 值，如数字 1，
                 * 并且它占用四个字节时，这个值在脚本中以小端序格
                 * 式表示。小端序意味着最低有效字节在前。因此，数
                 * 字 1 表示为四个字节时，将以如下格式出现：
                 * 01 00 00 00
                 * 数字 257 以四字节的小端序格式表示时会是这样的：
                 * 01 01 00 00
                 */
                for (int indexOfLoop = 0; indexOfLoop < 4; indexOfLoop++)
                {
                    script.Add(Convert.ToByte(lockTime % 256));
                    lockTime /= 256;
                }
            }
            //script.AddRange(locktimeBytes);             // 添加 locktime
            script.Add(0xb1);                           // OP_CHECKLOCKTIMEVERIFY 177
            script.Add(0x75);                           // OP_DROP
            script.Add(0x76);                           // OP_DUP
            script.Add(0xa9);                           // OP_HASH160
            script.Add(0x14);                           // 长度为20
            script.AddRange(publicKey.Hash.ToBytes());        // 添加RIPEMD-160 哈希
            script.Add(0x88);                           // OP_EQUALVERIFY
            script.Add(0xac);

            Script inputScript = new Script(script);

            // if (inputScript != null)
            {
                BitcoinAddress? resultAddress = inputScript.Hash.GetAddress(Network.Main);
                if (resultAddress != null)
                {
                    Console.WriteLine($"NBitcoinResult:{resultAddress.ToString()}");
                }
            }

        }


        public static void GetHex2(string privateKeyStr, uint lockTime)
        {
            GetAddr(privateKeyStr, lockTime);
            if (lockTime < 1)
            {
                Console.WriteLine($"lockTime 的值必须大于1");
                return;
            }
            var operateLockTime = lockTime;
            var privateKey = new BitcoinSecret(privateKeyStr, mainNet);
            var network = privateKey.Network;
            //  var address = privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, network);

            var publicKey = privateKey.PubKey;
            //  privateKey.PubKeyHash

            List<byte> script = new List<byte>();
            script.Add(0x4c); // OP_PUSHDATA1 76
            script.Add(0x4); // 长度为4
            {
                /*
                 * 在比特币脚本中，当您使用 locktime 值，如数字 1，
                 * 并且它占用四个字节时，这个值在脚本中以小端序格
                 * 式表示。小端序意味着最低有效字节在前。因此，数
                 * 字 1 表示为四个字节时，将以如下格式出现：
                 * 01 00 00 00
                 * 数字 257 以四字节的小端序格式表示时会是这样的：
                 * 01 01 00 00
                 */
                for (int indexOfLoop = 0; indexOfLoop < 4; indexOfLoop++)
                {
                    script.Add(Convert.ToByte(operateLockTime % 256));
                    operateLockTime /= 256;
                }
            }
            //script.AddRange(locktimeBytes);             // 添加 locktime
            script.Add(0xb1);                           // OP_CHECKLOCKTIMEVERIFY 177
            script.Add(0x75);                           // OP_DROP
            script.Add(0x76);                           // OP_DUP
            script.Add(0xa9);                           // OP_HASH160
            script.Add(0x14);                           // 长度为20
            script.AddRange(publicKey.Hash.ToBytes());        // 添加RIPEMD-160 哈希
            script.Add(0x88);                           // OP_EQUALVERIFY
            script.Add(0xac);

            Script redeemScript = new Script(script);
            Transaction transaction = Transaction.Create(mainNet);

            //var txBuilder = TransactionBuilder(;
            ScriptCoin scriptCoin;
            {
                //var uin5256ID = uint256.Parse("2c21cf938ec3f335133a4a776db435ea34c98e88b755797876f177478068580f");
                //scriptCoin = new ScriptCoin(
                //uin5256ID,
                //  0,
                //  new Money(9163, MoneyUnit.Satoshi),
                //  redeemScript.Hash.ScriptPubKey,
                //  redeemScript);
                //transaction.Inputs.Add(scriptCoin.Outpoint);
                //var transactions= Transaction.Load()
                var uin5256ID = uint256.Parse("0x06936301f97f00cf9fdea459bb64f84c432d201af2680b6f9c24a5ca847e9c96");
                var outPoint = new OutPoint(uin5256ID, 0);
                var coin = new Coin(outPoint, new TxOut(new Money(10000, MoneyUnit.Satoshi), redeemScript.Hash));
                //var coin = new ScriptCoin(outPoint, new TxOut(new Money(amount, MoneyUnit.BTC), redeemScript.Hash), redeemScript);

                transaction.Inputs.Add(new TxIn(outPoint));
                //transaction.Inputs[transaction.Inputs.Count - 1].ScriptSig = redeemScript;
                scriptCoin = new ScriptCoin(outPoint, new TxOut(new Money(10000, MoneyUnit.Satoshi), redeemScript.Hash), redeemScript);
            }
            {
                var destinationAddress = BitcoinAddress.Create("1APMnP2awYDhvarL1YRvFDUMGpgEAJBq8E", network);
                //添加输出
                transaction.Outputs.Add(new TxOut(new Money(6000, MoneyUnit.Satoshi), destinationAddress));

            }


            transaction.LockTime = lockTime + 10;

            transaction.Sign(privateKey, new[] { scriptCoin });

            // transaction.

            string txHex = transaction.ToHex();

            Console.WriteLine($"交易内容: {transaction.GetHash()}");
            Console.WriteLine($"交易内容: {txHex}");

            //var txBuilder = new TransactionBuilder()
        }

        public static void Test()
        {
            //var network= Network.TestNet;
            //var privateKey = new Key();
            //var bitcoinAddress = privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, network);

            //Transaction transaction = new Transaction();

        }

        public static void GetHex3(string privateKeyStr, uint lockTime)
        {
            GetAddr(privateKeyStr, lockTime);
            if (lockTime < 1)
            {
                Console.WriteLine($"lockTime 的值必须大于1");
                return;
            }
            var operateLockTime = lockTime;
            var privateKey = new BitcoinSecret(privateKeyStr, mainNet);
            var network = privateKey.Network;
            //  var address = privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, network);

            var publicKey = privateKey.PubKey;
            //  privateKey.PubKeyHash

            //var aliceBobNicoAddress = AliceBobNicoCorp.GetScriptAddress(Network.Main);

            List<byte> script = new List<byte>();
            script.Add(0x4c); // OP_PUSHDATA1 76
            script.Add(0x4); // 长度为4
            {
                /*
                 * 在比特币脚本中，当您使用 locktime 值，如数字 1，
                 * 并且它占用四个字节时，这个值在脚本中以小端序格
                 * 式表示。小端序意味着最低有效字节在前。因此，数
                 * 字 1 表示为四个字节时，将以如下格式出现：
                 * 01 00 00 00
                 * 数字 257 以四字节的小端序格式表示时会是这样的：
                 * 01 01 00 00
                 */
                for (int indexOfLoop = 0; indexOfLoop < 4; indexOfLoop++)
                {
                    script.Add(Convert.ToByte(operateLockTime % 256));
                    operateLockTime /= 256;
                }
            }
            //script.AddRange(locktimeBytes);             // 添加 locktime
            script.Add(0xb1);                           // OP_CHECKLOCKTIMEVERIFY 177
            script.Add(0x75);                           // OP_DROP
            script.Add(0x76);                           // OP_DUP
            script.Add(0xa9);                           // OP_HASH160
            script.Add(0x14);                           // 长度为20
            script.AddRange(publicKey.Hash.ToBytes());        // 添加RIPEMD-160 哈希
            script.Add(0x88);                           // OP_EQUALVERIFY
            script.Add(0xac);

            Script redeemScript = new Script(script);
            Transaction transaction = Transaction.Create(mainNet);


            var aliceBobNicoAddress = redeemScript.PaymentScript.GetDestinationAddress(mainNet); // AliceBobNicoCorp.GetScriptAddress(Network.Main);

            if (aliceBobNicoAddress != null)
            {
                Console.WriteLine($"要用钱的地址--{aliceBobNicoAddress.ToString()}");
            }
            else
            {
                return;
            }
            //  transaction.Outputs.Add(new TxOut( ));

            //var txBuilder = TransactionBuilder(;
            // ScriptCoin scriptCoin;
            TxOutList moneyWillCost = new TxOutList();
            {
                //var uin5256ID = uint256.Parse("2c21cf938ec3f335133a4a776db435ea34c98e88b755797876f177478068580f");
                //scriptCoin = new ScriptCoin(
                //uin5256ID,
                //  0,
                //  new Money(9163, MoneyUnit.Satoshi),
                //  redeemScript.Hash.ScriptPubKey,
                //  redeemScript);
                //transaction.Inputs.Add(scriptCoin.Outpoint);
                //var transactions= Transaction.Load()
                var uin5256ID = uint256.Parse("0x06936301f97f00cf9fdea459bb64f84c432d201af2680b6f9c24a5ca847e9c96");
                var outPoint = new OutPoint(uin5256ID, 0);
                var coin = new Coin(outPoint, new TxOut(new Money(10000, MoneyUnit.Satoshi), redeemScript.Hash));
                //var coin = new ScriptCoin(outPoint, new TxOut(new Money(amount, MoneyUnit.BTC), redeemScript.Hash), redeemScript);

                transaction.Inputs.Add(new TxIn(outPoint));
                //transaction.Inputs[transaction.Inputs.Count - 1].ScriptSig = redeemScript;
                // scriptCoin = new ScriptCoin(outPoint, new TxOut(new Money(10000, MoneyUnit.Satoshi), redeemScript.Hash), redeemScript);
                moneyWillCost.Add(new TxOut(coin.Amount, redeemScript.PaymentScript));
            }
            {
                var destinationAddress = BitcoinAddress.Create("1APMnP2awYDhvarL1YRvFDUMGpgEAJBq8E", network);
                //添加输出
                transaction.Outputs.Add(new TxOut(new Money(6000, MoneyUnit.Satoshi), destinationAddress));

            }
            Coin[] corpCoinsP2SH = moneyWillCost
                        .Select((o, i) => new ScriptCoin(new OutPoint(transaction.GetHash(), i), o, redeemScript))
                        .ToArray();

            transaction.LockTime = lockTime + 10;

            transaction.Sign(privateKey, corpCoinsP2SH);

            // transaction.

            string txHex = transaction.ToHex();

            Console.WriteLine($"交易内容: {transaction.GetHash()}");
            Console.WriteLine($"交易内容: {txHex}");

            //var txBuilder = new TransactionBuilder()
        }


        public static void GetHex4(string privateKeyStr, uint lockTime, string redeemScriptStr)
        {
            var privateKey = new BitcoinSecret(privateKeyStr, Network.TestNet);
            Script redeemScript = new Script(Convert.FromHexString(redeemScriptStr));

            Transaction transaction = Transaction.Create(Network.TestNet);

            //  transaction.Inputs.Add(new OutPoint())
        }
    }
}
