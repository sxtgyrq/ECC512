import { yrqbitcore } from './signandverify_simple.js  ';


export function p2pkhBothFromPub(pubHex, network = 'livenet') {

    var odd = pubHex[1].isOdd()
    var addr1 = yrqbitcore().PublicKey.fromX(odd, pubHex[0]).toAddress(network).toString();
    console.log('addr1', addr1);

    var addr2 = yrqbitcore().PublicKey.fromString('04' + pubHex[0].toString(16).padStart(64, '0') + pubHex[1].toString(16).padStart(64, '0'), 'hex').toAddress(network).toString();

    
    var a = 0;
    a++;

    return [addr1, addr2]; 
}

function p2shP2wpkhFromPub(pubHex, network = 'livenet') {
    const net = network === 'testnet' ? bitcore.Networks.testnet : bitcore.Networks.livenet;

    // 1) 解析公钥（必须是压缩形式：02/03 + 32B）
    const pub = new bitcore.PublicKey(pubHex);
    if (!pub.compressed) {
        throw new Error('P2SH-P2WPKH 需要压缩公钥（前缀 02/03）');
    }

    // 2) 计算 HASH160(pubkey) 作为 witness program 的 20 字节
    const pubKeyHash = bitcore.crypto.Hash.sha256ripemd160(pub.toBuffer()); // 20B

    // 3) 构造 redeemScript: OP_0 (0x00) + PUSH20 (0x14) + pubKeyHash
    const redeemScript = Buffer.concat([Buffer.from([0x00, 0x14]), pubKeyHash]);

    // 4) P2SH 地址：HASH160(redeemScript) → Base58Check(版本=脚本哈希)
    const scriptHash = bitcore.crypto.Hash.sha256ripemd160(redeemScript); // 20B
    const address = new bitcore.Address(scriptHash, net, bitcore.Address.PayToScriptHash);
    return address.toString(); // 3 开头
}