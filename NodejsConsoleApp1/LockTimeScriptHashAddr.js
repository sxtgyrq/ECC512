'use strict';

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

        var operateLockTime=lockTime;
        var script=[bitcoin.opcodes.OP_PUSHDATA1,0x4];
        for(var i=0;i<4;i++)
        {
            script.push(operateLockTime%256);
            operateLockTime=operateLockTime>>8;;// /= 256;
        }
        script.push(bitcoin.opcodes.OP_CHECKLOCKTIMEVERIFY);//177
        script.push(bitcoin.opcodes.OP_DROP);//177
        script.push(bitcoin.opcodes.OP_DUP);//177
        script.push(bitcoin.opcodes.OP_HASH160);//177
        script.push(0x14);//长度为20

        
        var buffer=keyPair.publicKey;
        console.log('publicKey',keyPair.publicKey);
        console.log('publicKeyHex',keyPair.publicKey.toString('hex'));


        const crypto = require('crypto');
        const cryptoPublicKeySha256 = crypto.createHash('sha256').update(keyPair.publicKey.toString('hex'),'hex').digest('hex');




        const ripemd160HashHex = crypto.createHash('ripemd160').update(cryptoPublicKeySha256,'hex').digest('hex'); 
        console.log('ripemd160',ripemd160HashHex); 
        if(ripemd160HashHex.length!=40)
        {
            throw 'ripemd160HashHex.length!=40';
        }
        for(var i=0;i<20;i++)
        { 
            script.push(parseInt('0x'+ripemd160HashHex.slice(i*2,i*2+2))); 
        }

        script.push(bitcoin.opcodes.OP_EQUALVERIFY);//
        script.push(bitcoin.opcodes.OP_CHECKSIG);//

        console.log('script',script);

        const scriptObj = bitcoin.script.compile(script);

        console.log('scriptASM',this.HexToASM(scriptObj.toString('hex')));

        console.log('scriptHex',scriptObj.toString('hex'))

        const p2sh = bitcoin.payments.p2sh({
            redeem: { output: scriptObj, network: network },
            network: network
        });
        
        console.log("P2SH 地址:", p2sh.address); 
        return scriptObj.toString('hex');
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
    generateTranstraction:function(privateKey,redeemScriptHex)
    {
        const bitcoin = require('bitcoinjs-lib');
        const network = LockTimeScriptHashAppr.netWork(); // 对于测试网使用 bitcoin.networks.testnet

        // 创建一个交易构建器
        const psbt = new bitcoin.Psbt({ network: network });
        
        psbt.locktime=830000;

        psbt.addInput({
            hash: 'cf935c8f3d0111bcbdb12bce4fe82343a25046af0089ef3958614fb172a24d96',
            index: 0,
            nonWitnessUtxo: Buffer.from('020000000001017a6372de74a88e202b603de9a34199e89b855dff9684c85d70e9aab9b0dea5e70c00000000fdffffff01420300000000000017a914d86a2141719e9be3de1e17ee06de2790edfa86cd870247304402205b89b40374d3fa3624a6181ea699823100825da6a3952272799b5e3f2c0388db02203bc55a1f8d0f67c622aa20c042277cf5914ddbe6ac7c1cb6016247694a76ece601210260ee85eec76d21a696eb105ba967405b6d376e3ebec8ed5587b237074414b06662422700', 'hex'),
            // witnessUtxo:
            // {
            //     script:Buffer.from('a914d86a2141719e9be3de1e17ee06de2790edfa86cd87', 'hex'),
            //     value: 834
            // },
            redeemScript: Buffer.from(redeemScriptHex, 'hex')
          });
        //https://blockstream.info/testnet/api/tx/cf935c8f3d0111bcbdb12bce4fe82343a25046af0089ef3958614fb172a24d96/hex

        //https://api.blockcypher.com/v1/btc/main/txs/<txid>?includeHex=true 的URL（这是BlockCypher的API示例）。
        //   psbt.addInput({
        //     hash: '前一个交易的哈希2',
        //     index: 0,
        //     nonWitnessUtxo: Buffer.from('前一个交易的完整未签名的交易数据的十六进制字符串2', 'hex'),
        //     redeemScript: Buffer.from('赎回脚本的十六进制字符串', 'hex'),
        //   });
    
        psbt.addOutput({
            address: 'n4dEJzZxe4Uguup1SRGF83M8ko6nBV73Ec',
            value: 200, // 单位: 聪
          });

        const TinySecp256k1Interface = require('tiny-secp256k1');
        const ECPairFactory  = require('ecpair');
        //const bitcoin=require('bitcoinjs-lib') ;
        const ECPair =ECPairFactory.ECPairFactory(TinySecp256k1Interface);
     //   const network = ECPairFactory.networks.bitcoin;
        const keyPair = ECPair.fromWIF(privateKey,network);

        
        //  const privateKeyEcpair = bitcoin.ECPair.fromWIF(privateKey, network);
        //psbt.signAllInputs(keyPair);
         // psbt.signInput(0, keyPair);
         psbt.signInput(0,keyPair);
         

         console.log('valid signature: ', psbt.validateSignaturesOfAllInputs(this.Validator));

         psbt.finalizeInput(0, this.csvGetFinalScripts)

          //psbt.validateSignaturesOfAllInputs();
          // 完成交易
         // psbt.finalizeAllInputs();
          
          // 获取交易的序列化格式
          const rawTransaction = psbt.extractTransaction().toHex();
          console.log('translation',rawTransaction);
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
    HexToASM:function(hexString)
    {  
        const bitcoin = require('bitcoinjs-lib');
        const scriptObj = bitcoin.script.decompile(Buffer.from(hexString, 'hex')); 
        var asmString= bitcoin.script.toASM(scriptObj); 
        console.log('ASM String:', asmString);
        return asmString;
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
      
       
    }

}
 

exports.LockTimeScriptHashAppr =  LockTimeScriptHashAppr; 