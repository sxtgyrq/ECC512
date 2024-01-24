//'use strict';

//const { Buffer } = require('bitcoinjs-lib/src/types');

const LockTimeScriptHashApprMainNet =
{
    netWork: function () {
        const TinySecp256k1Interface = require('tiny-secp256k1');
        const ECPairFactory = require('ecpair');
        const bitcoin = require('bitcoinjs-lib');
        const ECPair = ECPairFactory.ECPairFactory(TinySecp256k1Interface);
        const network = ECPairFactory.networks.bitcoin;
        return network;
    },
    generateTranstraction: function (wifStr, redeemScriptHex) {
        const bitcoin = require('bitcoinjs-lib');
        const network = LockTimeScriptHashApprMainNet.netWork(); // 对于测试网使用 bitcoin.networks.testnet


        const TinySecp256k1Interface = require('tiny-secp256k1');
        const ECPairFactory = require('ecpair');
        const ECPair = ECPairFactory.ECPairFactory(TinySecp256k1Interface);

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
        console.log('address', address);
        // fund the P2SH(CLTV) address
        //const unspent = await regtestUtils.faucet(address!, 1e5);

        //tx.setInputScript(0, redeemScriptSig!); 
        const privateKey = ECPair.fromWIF(wifStr, network);
        const utxos = [
            { // 可能需要多个UTXO
                txId: 'f66a1d8e68cff90b790252c0788cbbe137c8ecda5fe70822fc9cebe96c0c11b5',
                vout: 1,
                value: 2000000, //satoshi
            }];
        const destinationAddress = '3Qd5tZHQJtRhtfzMqD8XMVR5aYxxbkQ7wi';
        //const fee = 5500;

        // 创建一个新的PSBT（Partially Signed Bitcoin Transaction）
        let psbt = new bitcoin.Psbt({ network: network });
        //psbt.locktime=820001;

        // 添加UTXOs
        utxos.forEach(utxo => {
            psbt.addInput({
                hash: utxo.txId,
                index: utxo.vout,
                sequence: 0xfffffffe,//sequence 字段是一个 32 位的数字，当您想要使用交易的 nLockTime 特性时，必须将输入的 sequence 数值设置为小于 0xffffffff（即 4294967295）。如果 sequence 设置为 0xffffffff，将会禁用 nLockTime 功能。
                nonWitnessUtxo: Buffer.from('02000000000101a55de8fa27ab8bcebe5cb0e0d85119c97457f8a413259462c27dffc284115fb6010000001716001475dfe65572e5cdd2b3022b84be8b5546f4e26428fdffffff0362320f000000000017a91400324c2a936efe0787b1dd6686e29513e62b7bc68780841e000000000017a9140d8179055682ed14d4fe85e6ac2c9ee4c4e076688700093d000000000017a914fb8c8a24c34d1645059c4ae06d0af70ff0635dd0870247304402203513d288f2f78014800377dbd067bee3ac90b0f70e01fae00700270f4a0ffb6d022074259088ea2a23b964b46e36a6e308f58644add3c348a51e3b44808ffdbee9e301210393797bda27522c943ecd8c72b51025e2d1b686ae788e0719f916d61d4eddb5ea11a00c00', 'hex'),  //nonWitnessUtxo: Buffer.from('FULL_TRANSACTION_HEX_OF_UTXO', 'hex'),
                redeemScript: redeemScript,
            });
        });
        //https://mempool.space/api/tx/f66a1d8e68cff90b790252c0788cbbe137c8ecda5fe70822fc9cebe96c0c11b5/hex  通过以上连接获取 nonWitnessUtxo
        // 添加输出（接收地址和金额）
        const totalUtxoValue = utxos.reduce((acc, utxo) => acc + utxo.value, 0);
        psbt.addOutput({
            address: destinationAddress,
            value: 1995000, // 确保包含交易费
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
        console.log('rawTransactionHex', rawTransaction);
        return rawTransaction;
    },

}


exports.LockTimeScriptHashApprMainNet = LockTimeScriptHashApprMainNet;

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