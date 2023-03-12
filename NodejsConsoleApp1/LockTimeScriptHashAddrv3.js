//'use strict';

//const { Buffer } = require('bitcoinjs-lib/src/types');

const LockTimeScriptHashApprMainNet=
{
    netWork:function()
    {
        const TinySecp256k1Interface = require('tiny-secp256k1');
        const ECPairFactory  = require('ecpair');
        const bitcoin=require('bitcoinjs-lib') ;
        const ECPair =ECPairFactory.ECPairFactory(TinySecp256k1Interface);
        const network = ECPairFactory.networks.bitcoin;   
        return network;
    }, 
    generateTranstraction:function(wifStr,redeemScriptHex)
    {
        const bitcoin = require('bitcoinjs-lib');
        const network = LockTimeScriptHashApprMainNet.netWork(); // 对于测试网使用 bitcoin.networks.testnet


        const TinySecp256k1Interface = require('tiny-secp256k1');
        const ECPairFactory  = require('ecpair');
        const ECPair =ECPairFactory.ECPairFactory(TinySecp256k1Interface);

        // const alice = ECPair.fromWIF(
        //   wifStr,
        //     network
        //   );

        //const lockTime = bip65.encode({ utc: utcNow() - 3600 * 3 });
        const redeemScript = Buffer.from(redeemScriptHex, 'hex');
        const { address } = bitcoin.payments.p2sh({
          redeem: { output: redeemScript, network: network },
          network: network,
        });
  
        // fund the P2SH(CLTV) address
       //const unspent = await regtestUtils.faucet(address!, 1e5);
        
        //tx.setInputScript(0, redeemScriptSig!); 
        const privateKey = ECPair.fromWIF(wifStr, network);
        const utxos = [
          { // 可能需要多个UTXO
            txId: 'b65f1184c2ff7dc262942513a4f85774c91951d8e0b05cbece8bab27fae85da5',
            vout: 0,
            value: 10000 , //satoshi
          }];
        const destinationAddress = '1APMnP2awYDhvarL1YRvFDUMGpgEAJBq8E';
        //const fee = 5500;

// 创建一个新的PSBT（Partially Signed Bitcoin Transaction）
        let psbt = new bitcoin. Psbt({ network: network });
        //psbt.locktime=820001;

// 添加UTXOs
          utxos.forEach(utxo => {
            psbt.addInput({
              hash: utxo.txId,
              index: utxo.vout,
              sequence: 0xfffffffe,//sequence 字段是一个 32 位的数字，当您想要使用交易的 nLockTime 特性时，必须将输入的 sequence 数值设置为小于 0xffffffff（即 4294967295）。如果 sequence 设置为 0xffffffff，将会禁用 nLockTime 功能。
              nonWitnessUtxo: Buffer.from('02000000000101e686a9343a72c0eb2621550090d1a1fdae3a9b8896bf6f939cc187b2dc20e7750100000017160014bba4037a96cfec7d72e94ff4e8146028fe86fbb4fdffffff02102700000000000017a91454733dd9b8ff8b0678a0c7b21a3bad31f73b2be08786d26a000000000017a914002f8e7ec2299211ff56eef5b73e1b31c07b63c0870247304402200f0cd17809a3f7caa8157a7786239834a06f247f80a8c10c31d8e2a2a82f67b602202c673a6e01e14a6aa3816f3d358307d432cf6a0d00aaa6c8dcea647235abfcf60121039892e7f43284ba71c95743cc7ea7de064d8e1314845a9d6ea4b995ed7a88fe3d8c9d0c00', 'hex'),  //nonWitnessUtxo: Buffer.from('FULL_TRANSACTION_HEX_OF_UTXO', 'hex'),
              redeemScript: redeemScript,
            });
          });

// 添加输出（接收地址和金额）
        const totalUtxoValue = utxos.reduce((acc, utxo) => acc + utxo.value, 0);
        psbt.addOutput({
          address: destinationAddress,
          value: 4500, // 确保包含交易费
        });
        psbt.setLocktime(826905);

// 签名所有输入
        utxos.forEach((_, index) => {
          psbt.signInput(index, privateKey);
        });
       
        // 手动完成输入的函数
        function finalizeInputFn(inputIndex, input, script, value) {
          // 这里需要根据您的赎回脚本的具体逻辑来构建输入脚本
          // 下面是一个基本的结构，您可能需要调整
          const signature = input.partialSig[0].signature;
          const pubkey = privateKey.publicKey;
          const scriptSigChunks = [
            signature,
            pubkey,
            redeemScript
          ];
          return {
            finalScriptSig: bitcoin.script.compile(scriptSigChunks)
          };
        }
        psbt.finalizeInput(0, finalizeInputFn);

// 完成签名
         // psbt.finalizeAllInputs();

// 获取交易的最终序列化形式
          const rawTransaction = psbt.extractTransaction().toHex();
          console.log('rawTransactionHex',rawTransaction);
          return rawTransaction;
    }, 

}
 

exports.LockTimeScriptHashApprMainNet =  LockTimeScriptHashApprMainNet; 

// 在比特币交易中，每个输入都有一个名为 sequence 的字段。这个字段最初被设计用于实现未来的交易替换功能，但随着时间的发展，它的用途已经扩展，尤其是在引入比特币协议的某些更新后，比如 BIP 68 和 BIP 125。下面是 sequence 字段的一些主要功能：

// 1. 替换交易（Replace-by-Fee，RBF）
// 初始设计：sequence 字段最初设计用于交易的替换。如果一个交易的 sequence 值小于 0xffffffff，那么它可以被带有相同输入和更高手续费的新交易替换。
// BIP 125（RBF）：随着 BIP 125 的实施，sequence 字段用于标识一个交易是否可被替换。如果所有输入的 sequence 值都小于 0xfffffffd，交易被认为是可替换的。
// 2. 相对锁定时间（BIP 68）
// BIP 68：这个提案引入了使用 sequence 字段来指定相对锁定时间。这意味着交易的某个输入可以指定在其引用的UTXO被确认一定数量的区块后或一定时间后才能被花费。
// 怎么工作：如果 sequence 字段的最高位（第31位）为0，那么这个字段表示一个相对锁定时间。如果是时间锁定，其单位是512秒；如果是区块锁定，其单位是1个区块。
// 3. 禁用 nLockTime
// 如果 sequence 值为 0xffffffff，则表示该输入忽略交易的 nLockTime 字段，即该输入不受 nLockTime 的影响。
// 实际应用
// 在实际交易构建中，根据您的需要选择适当的 sequence 值非常重要。例如，如果您想使用交易的 nLockTime 特性，您需要将所有输入的 sequence 值设置为小于 0xffffffff 的值。如果您不需要 nLockTime 或相对锁定时间的功能，通常可以使用默认的最大值 0xffffffff。

// 综上所述，sequence 字段是一个灵活的工具，可用于实现交易的替换、设置相对锁定时间，或者影响 nLockTime 的行为。在构建交易时正确理解和使用这个字段是非常重要的。