//'use strict';

//const { Buffer } = require('bitcoinjs-lib/src/types');

const LockTimeScriptHashAppr=
{
    netWork:function()
    {
        const TinySecp256k1Interface = require('tiny-secp256k1');
        const ECPairFactory  = require('ecpair');
        const bitcoin=require('bitcoinjs-lib') ;
        const ECPair =ECPairFactory.ECPairFactory(TinySecp256k1Interface);
        const network = ECPairFactory.networks.testnet;   
        return network;
    },
    generateRedeemScript:function(wif,lockTime){
        const TinySecp256k1Interface = require('tiny-secp256k1');
        const ECPairFactory  = require('ecpair');
        const bitcoin=require('bitcoinjs-lib') ;
        const ECPair =ECPairFactory.ECPairFactory(TinySecp256k1Interface);
        const network = this.netWork();
        const keyPair = ECPair.fromWIF(wif,LockTimeScriptHashAppr.netWork());
        var p2pkh  = bitcoin.payments.p2pkh({ pubkey: keyPair.publicKey, network });
        console.log('p2pkh地址为：',p2pkh.address); 
        var bitcoinLockTime=  bitcoin.script.number.encode(lockTime).toString('hex');
        console.log('bitcoinLockTime',bitcoinLockTime);

        var redeemScript=this.getLockScriptHashAddrReturnRedeemScriptHex(wif,lockTime);

        return redeemScript;
        


// var GenerateScriptAddrInput= require('./GenerateScriptAddrInput.js')
    },
    getLockScriptHashAddrReturnRedeemScriptHex:function(wif,lockTime)
    {
        const TinySecp256k1Interface = require('tiny-secp256k1');
        const ECPairFactory  = require('ecpair');
        const bitcoin=require('bitcoinjs-lib') ;

        const ECPair =ECPairFactory.ECPairFactory(TinySecp256k1Interface);
        const network = this.netWork();
        const keyPair = ECPair.fromWIF(wif,network);
        var p2pkh  = bitcoin.payments.p2pkh({ pubkey: keyPair.publicKey, network });
       // keyPair.publicKey.

        //var operateLockTime=lockTime;
        

        
        const redeemScript =this. cltvCheckSigOutput(keyPair, lockTime,p2pkh.hash.toString('hex'));

        var asmStr=this.ObjToASM(redeemScript)
        console.log('asmStr',asmStr);

        const { address } = bitcoin.payments.p2sh({
            redeem: { output: redeemScript, network: network },
            network: network,
        });
        console.log('P2SH 地址:',address.toString());

        // const p2sh = bitcoin.payments.p2sh({
        //     redeem: { output: scriptObj, network: network },
        //     network: network
        // });
        
        // console.log("P2SH 地址:", p2sh.address); 
        return redeemScript.toString('hex');
    },
    payToTargetAddr:function()
    {
        const bitcoin = require('bitcoinjs-lib');
        const network = bitcoin.networks.bitcoin; // 使用比特币主网，测试网则为 bitcoin.networks.testnet

        const redeemScript = Buffer.from('您的赎回脚本的十六进制字符串', 'hex');
        const p2sh = bitcoin.payments.p2sh({
            redeem: { output: redeemScript, network },
            network
          });
          // 创建交易
          const txb = new bitcoin.TransactionBuilder(network);
          // 添加输入：这里的参数分别为：UTXO的交易ID，UTXO在其交易中的索引
          txb.addInput('UTXO交易ID', UTXO索引);
          // 添加输出（发送到的地址和金额（单位：聪））
          txb.addOutput('接收地址', 金额);
          // 签名交易：此处的参数分别为：要签名的输入的索引，私钥，赎回脚本
          txb.sign(0, 私钥, redeemScript);
          // 构建交易并获取其序列化形式
          const rawTransaction = txb.build().toHex();
          console.log("交易原文:", rawTransaction);




    },
    generateTranstraction:function(wifStr,redeemScriptHex)
    {
        const bitcoin = require('bitcoinjs-lib');
        const network = LockTimeScriptHashAppr.netWork(); // 对于测试网使用 bitcoin.networks.testnet


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
            txId: '95adc26ad6f34b8113296235b658830f8afa013c64549e01dbd59e497efed747',
            vout: 0,
            value: 1834 , //satoshi
          }];
        const destinationAddress = 'tb1qks4urhlfap4rzphf3eptslzu9tvlra4jppxxyq';
        const fee = 500;

// 创建一个新的PSBT（Partially Signed Bitcoin Transaction）
        let psbt = new bitcoin. Psbt({ network: network });
        //psbt.locktime=820001;

// 添加UTXOs
          utxos.forEach(utxo => {
            psbt.addInput({
              hash: utxo.txId,
              index: utxo.vout,
              sequence: 0xfffffffe,//sequence 字段是一个 32 位的数字，当您想要使用交易的 nLockTime 特性时，必须将输入的 sequence 数值设置为小于 0xffffffff（即 4294967295）。如果 sequence 设置为 0xffffffff，将会禁用 nLockTime 功能。
              nonWitnessUtxo: Buffer.from('020000000001013d272f141dd9ef6f830b72d1f0a34eb79e8e4ae72d6b9200d3762a40dcb3498b0c00000000fdffffff012a0700000000000017a914cc63381584cf4672c08143d537e6ae366149e5da870247304402204a043b84f77567668bb49d86509e526dba3447462ed35b077902fb05a142a1f402207dadd759263464cf8600f56c5df0671e89ca4217b0061b3d28598abedb4708a0012103f94641a5ef1b93a336dfdcae79cc0dfde3748d6366ea1e698cbda0452cc5ae9b98462700', 'hex'),  //nonWitnessUtxo: Buffer.from('FULL_TRANSACTION_HEX_OF_UTXO', 'hex'),
              redeemScript: redeemScript,
            });
          });

// 添加输出（接收地址和金额）
        const totalUtxoValue = utxos.reduce((acc, utxo) => acc + utxo.value, 0);
        psbt.addOutput({
          address: destinationAddress,
          value: 1334, // 确保包含交易费
        });
        psbt.setLocktime(820001);

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
    generateRandomPrivake:function()
    {
        const TinySecp256k1Interface = require('tiny-secp256k1');
        const ECPairFactory  = require('ecpair');
        const bitcoin=require('bitcoinjs-lib') ;
        const ECPair =ECPairFactory.ECPairFactory(TinySecp256k1Interface);
        const network =  this.netWork();
        const keyPair = ECPair.makeRandom({ network: this.netWork() });
        var p2pkh  = bitcoin.payments.p2pkh({ pubkey: keyPair.publicKey, network });
        console.log('p2pkh地址为：',p2pkh.address);
        console.log('私钥为',keyPair.toWIF());

        //var redeemScript=this.getLockScriptHashAddrReturnRedeemScriptHex(wif,lockTime);

        return redeemScript;
    },
    ObjToASM:function(scriptObj)
    {  
        const bitcoin = require('bitcoinjs-lib');
       // const scriptObj = bitcoin.script.decompile(Buffer.from(hexString, 'hex')); 
        var asmString= bitcoin.script.toASM(scriptObj); 
        console.log('ASM String:', asmString);
        return asmString;
    },
    HexToBuffer:function(scriptHex)
    {  
        //const bitcoin = require('bitcoinjs-lib');
        return  Buffer.from(scriptHex, 'hex'); 
        //return scriptObj; 
    },
    Validator: function  (publicKeyBuffer, sighash, signature) {
       
        const TinySecp256k1Interface = require('tiny-secp256k1'); 
        const ECPairFactory  = require('ecpair'); 
        const bitcoin=require('bitcoinjs-lib') ; 
        
        const ECPair =ECPairFactory.ECPairFactory(TinySecp256k1Interface);  
        const publicKey =  ECPair.fromPublicKey(publicKeyBuffer); 
        
        var calResult=  publicKey.verify(sighash, signature);   
         return calResult;  
    },
     customFinalizeInput:function(indexValue, parameter2, parameter3,parameter4,parameter5,parameter6) {

        //var sigArray= Array.from( parameter2.partialSig.signature)
        const bitcoin=require('bitcoinjs-lib') ; 

        const scriptSigChunks = [
            parameter2.partialSig[indexValue].signature,
            parameter2.partialSig[indexValue].pubkey,
            parameter2.partialSig[indexValue].redeemScript,
        ];

         const finalScriptSig = bitcoin.script.compile(scriptSigChunks);
         return finalScriptSig;




        // const scriptSigChunks = [
        //     parameter2.partialSig.pubkey,
        //     pubkey,
        //     redeemScript,
        // ];

        console.log('parameter1',parameter1);
        console.log('parameter2',parameter2);
        console.log('parameter3',parameter3);
        console.log('parameter4',parameter4);
        console.log('parameter5',parameter5);
        console.log('parameter6',parameter6);
       // console.log('parameter7',parameter7);
        // 假设 customScript 是一个Buffer，包含自定义的脚本
        // 这里添加您的逻辑来构建最终的脚本
        // const finalScriptSig = bitcoin.script.compile([
        //     // ...构建脚本的元素...
        // ]);
    
        // // 设置最终的脚本
        // psbt.setInputScript(inputIndex, finalScriptSig);
    },
     csvGetFinalScripts:function(
        inputIndex,//number
        input,//PsbtInput
        script,//Buffer
        isSegwit,//boolean
        isP2SH,//boolean
        isP2WSH,//boolean
      ) {
        const bitcoin = require('bitcoinjs-lib');
        // Step 1: Check to make sure the meaningful script matches what you expect.
        //const decompiled = bitcoin.script.decompile(script);
        // Checking if first OP is OP_IF... should do better check in production!
        // You may even want to check the public keys in the script against a
        // whitelist depending on the circumstances!!!
        // You also want to check the contents of the input to see if you have enough
        // info to actually construct the scriptSig and Witnesses.
        // if (!decompiled || decompiled[0] !== bitcoin.opcodes.OP_IF) {
        //   throw new Error(`Can not finalize input #${inputIndex}`);
        // }
      
        // Step 2: Create final scripts
       var regtest=LockTimeScriptHashAppr.netWork();
        // let payment = {
        //   network: regtest,
        //   output: script,
        //   // This logic should be more strict and make sure the pubkeys in the
        //   // meaningful script are the ones signing in the PSBT etc.
        //   input: input.redeemScript,
        // };
        // if (isP2WSH && isSegwit)
        //   payment = bitcoin.payments.p2wsh({
        //     network: regtest,
        //     redeem: payment,
        //   });
      //  let payment=null;

        if (isP2SH){  
            var signature=  input.partialSig[0].signature;
            var pubkey=  input.partialSig[0].pubkey;
            var redeemScript=script;
            const finalScriptSig = Buffer.concat([
                //Buffer.from([0x1]),
               // Buffer.from([signature.length]), 
               
                //Buffer.from([pubkey.length]),  
                

             //   Buffer.from([0x1]),

                // 添加签名
                Buffer.from([redeemScript.length]), 
                redeemScript, // 添加赎回脚本
                signature,
                pubkey,


               // Buffer.from([redeemScript.length]), 
               // redeemScript, // 添加赎回脚本
                
                //Buffer.from([redeemScript.length]),
                //Buffer.from([redeemScript.length]),
              //  Buffer.from([redeemScript.length]), // 添加赎回脚本长度（这是 OP_PUSHDATA1 的标志）
                
              ]);


          
            return {
                finalScriptSig: finalScriptSig,
                finalScriptWitness: undefined,
              };
        }
        else
        {
            throw ('others not finised');
        }
      
       
    },
    utcNow: function (){
        return Math.floor(Date.now() / 1000);
    },
    cltvCheckSigOutput:function(aQ,//: KeyPair
    // bQ,//: KeyPair
     lockTime,//: number
     HASH160Hex
     ) 
     {
      console.log('HASH160Hex',HASH160Hex);
        var bQ=aQ;
        const bitcoin=require('bitcoinjs-lib') ;
        let lockTimeHex=  bitcoin.script.number.encode(lockTime).toString('hex');
        while(lockTimeHex.length<8)
        {
          lockTimeHex+='00';
        }
        console.log('lockTimeHex:',lockTimeHex); 
        // var scriptObj=  bitcoin.script.fromASM(
        //   `
        //   ${lockTimeHex}
        //   OP_CHECKLOCKTIMEVERIFY
        //   OP_DROP
        //   OP_DUP
        //   OP_HASH160
        //   OP_PUSHDATA1
        //   14
        //   ${HASH160Hex}
        //   OP_EQUALVERIFY
        //   OP_CHECKSIG
        //   `
        //       .trim()
        //       .replace(/\s+/g, ' '),
        //   );
          
            let scriptElements = [
              //bitcoin.opcodes.OP_PUSHDATA1,
              //4,
              Buffer.from(lockTimeHex, 'hex'),
              bitcoin.opcodes.OP_CHECKLOCKTIMEVERIFY,
              bitcoin.opcodes.OP_DROP,
              bitcoin.opcodes.OP_DUP,
              bitcoin.opcodes.OP_HASH160,
              //bitcoin.opcodes.OP_PUSHDATA1,
              //20,
              Buffer.from(HASH160Hex, 'hex'),
              bitcoin.opcodes.OP_EQUALVERIFY,
              bitcoin.opcodes.OP_CHECKSIG, 
          ];
          let scriptObj = bitcoin.script.compile(scriptElements);
        var asmStr=this.ObjToASM(scriptObj);
        return scriptObj;
    },
     idToHash:function(txid )  {
        return Buffer.from(txid, 'hex').reverse();
      }

}
 

exports.LockTimeScriptHashAppr =  LockTimeScriptHashAppr; 

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