import { yrqGetPrivateKeyByNumber, yrqHash, yrqBN, yrqsecp256k1, yrqCheckPrivateKey, yrqbitcore } from './signandverify_simple.js  ';
import { p2pkhBothFromPub } from "./generateAddr.js";


class InfomationClass {
    constructor(infomationOfOriginalText, step, responRealByte) {   // 构造函数，创建对象时会自动调用
        // this.infomationOfSecretText = infomationOfSecretText;
        this.infomationOfOriginalText = infomationOfOriginalText;
        this.step = step;
        this.responRealByte = responRealByte;
        // this.secretByte = secretByte;
    }
}

class BlobCursor {
    constructor(blob) {
        this.blob = blob;       // File 或 Blob
        this.pos = 0;           // 当前游标位置（字节）
    }

    // offset: 可正可负；origin: 'begin' | 'current' | 'end'
    seek(offset, origin = 'begin') {
        if (origin === 'begin') this.pos = Number(offset);
        else if (origin === 'current') this.pos += Number(offset);
        else if (origin === 'end') this.pos = this.blob.size + Number(offset);
        // 边界夹取
        this.pos = Math.max(0, Math.min(this.pos, this.blob.size));
        return this.pos;
    }

    // 读取 length 字节，并把游标后移
    async read(length) {
        const start = this.pos;
        const end = Math.min(this.pos + length, this.blob.size);
        const chunk = this.blob.slice(start, end);
        const buf = new Uint8Array(await chunk.arrayBuffer());
        this.pos = end;
        return buf; // Uint8Array
    }
    async write(data, offset = 0, count = undefined) {
        let u8;
        if (typeof data === 'string') {
            // 如果传的是字符串，按 UTF-8 写入
            u8 = new TextEncoder().encode(data);
        } else if (data instanceof Uint8Array) {
            u8 = data;
        } else if (data instanceof ArrayBuffer) {
            u8 = new Uint8Array(data);
        } else if (Array.isArray(data)) {
            u8 = new Uint8Array(data);
        } else {
            throw new Error('Unsupported data type for write');
        }
        const end = count === undefined ? u8.length : offset + count;
        const slice = u8.subarray(offset, end);

        // pos 可能在末尾之后：需要用 0 填充到 pos 再写
        if (this.pos > this.blob.size) {
            const pad = new Uint8Array(this.pos - this.blob.size);
            this.blob = new Blob([this.blob, pad, slice], { type: this.blob.type });
            this.pos += slice.length;
            return slice.length;
        }

        // 计算覆盖后的三段：before(0..pos), payload(slice), after(pos+len..end)
        const before = this.blob.slice(0, this.pos);
        const afterStart = this.pos + slice.length;
        const after =
            afterStart < this.blob.size ? this.blob.slice(afterStart) : new Blob([]);

        this.blob = new Blob([before, slice, after], { type: this.blob.type });
        this.pos += slice.length;
        return slice.length;
    }

    getBlob() {
        return this.blob;
    }
}


function ComPressPublic(publicKey) {
    var array = publicKey.point.x.toArray();
    //M.publicKey.point.x.toArray
    if (publicKey.point.y.isEven()) {
        var resultAdd = [2];
        resultAdd = resultAdd.concat(array);
        return resultAdd;
    }
    else {
        var resultAdd = [3];
        resultAdd = resultAdd.concat(array);
        return resultAdd;
    }
}
function ComPressPoint(point) {
    var array = point.x.toArray();
    //M.publicKey.point.x.toArray
    if (point.y.isEven()) {
        var resultAdd = [2];
        resultAdd = resultAdd.concat(array);
        return resultAdd;
    }
    else {
        var resultAdd = [3];
        resultAdd = resultAdd.concat(array);
        return resultAdd;
    }
}


function bytesToHex(arr, { uppercase = false, sep = '', prefix = '' } = {}) {
    let hex = '';
    for (let i = 0; i < arr.length; i++) {
        hex += arr[i].toString(16).padStart(2, '0');
        if (sep && i !== arr.length - 1) hex += sep;
    }
    return (prefix ? prefix : '') + (uppercase ? hex.toUpperCase() : hex);
}


function ByteArrayEqual(hash1, hashCode) {
    if (hash1.length == 32 && hashCode.length == 32) {
        for (var i = 0; i < 32; i++) {
            if (hash1[i] == hashCode[i]) { }
            else { return false; }
        }
        return true;
    }
    return false;
}

function ModInverse(n, p) {
    var x = yrqBN().fromNumber(1); //yrqBN().fromNumber(1)
    var y = yrqBN().fromNumber(0);
    var a = p;
    var b = n;
    var zero = yrqBN().fromNumber(0);
    while (b.cmp(zero) != 0) {
        var t = b;
        var q = a.div(t)
        b = a.sub(q.mul(t));
        a = t;
        t = x;
        x = y.sub(q.mul(t));
        y = t;
    }

    if (y.lt(zero))
        return y.add(p);
    //else
    return y;
}
function getBoolList(bigIntegers) {
    var zero = yrqBN().fromNumber(0);
    var result = [];
    while (bigIntegers.cmp(zero) != 0) {
        if (bigIntegers.isEven())
            result.push(true);
        else
            result.push(false);
        bigIntegers = bigIntegers.divn(2);
    }
    return result;
}
function getPublicByPrivate(privateV) {
    var zero = yrqBN().fromNumber(0);
    var q = yrqsecp256k1().curve.n;
    if (privateV.gt(zero)) {
        let result = null;
        //yrqsecp256k1().curve.g.y.toString(16).padStart(64, '0')
        let baseP = yrqsecp256k1().curve.g;//[yrqsecp256k1().curve.g.x, yrqsecp256k1().curve.g.y];
        let r = getBoolList(privateV);
        // privateV = privateKey.mod % q;
        for (let i = 0; i < r.length; i++) {
            if (!r[i]) {
                if (baseP == null) {
                }
                else {
                    if (result == null) {
                        result = baseP;
                    }
                    else {
                        result = result.add(baseP);
                    }
                }
            }
            baseP = baseP.dbl();
        }
        return result;
    }
    else {
        throw 'input error';
    }

    //  public static BigInteger[] getPublicByPrivate(BigInteger privateKey)
    //{
    //    if (privateKey > 0) {
    //            bool isZero;
    //        BigInteger[] result = null;
    //        BigInteger[] baseP = new System.Numerics.BigInteger[] { Secp256k1.x, Secp256k1.y };
    //        privateKey = privateKey % Secp256k1.q;
    //        var r = get(privateKey);
    //        //bool isZero = false;
    //        for (int i = 0; i < r.Length; i++)
    //        {
    //            if (!r[i]) {
    //                if (baseP == null) {
    //                }
    //                else {
    //                    if (result == null) {
    //                        result = baseP;
    //                    }
    //                    else {

    //                        result = pointPlus(result, baseP, out isZero);

    //                        //Console.WriteLine($"baseP{i} ={baseP[0]},{baseP[1]}");
    //                    }
    //                    //  result = result == null ? baseP : pointPlus(result, baseP); 
    //                }
    //            }
    //            baseP = getDoubleP(baseP, out isZero);

    //        }
    //        return result;
    //    }
    //    else if (privateKey.IsZero) {
    //        return null;
    //    }
    //    else {
    //        throw new Exception("privateKey的值不能为0和负数");
    //    }
    //}
}

function getMulValue(rValue, baseP) {
    var zero = yrqBN().fromNumber(0);
    // var q = yrqsecp256k1().curve.n;
    if (rValue.gt(zero)) {
        let result = null;
        //yrqsecp256k1().curve.g.y.toString(16).padStart(64, '0')
        // let baseP = yrqsecp256k1().curve.g;//[yrqsecp256k1().curve.g.x, yrqsecp256k1().curve.g.y];
        let r = getBoolList(rValue);
        // privateV = privateKey.mod % q;
        for (let i = 0; i < r.length; i++) {
            if (!r[i]) {
                if (baseP == null) {
                }
                else {
                    if (result == null) {
                        result = baseP;
                    }
                    else {
                        result = result.add(baseP);
                    }
                }
            }
            baseP = baseP.dbl();
        }
        return result;
    }
    else {
        throw 'input error';
    }
    //   internal static BigInteger[] getMulValue(BigInteger rValue, BigInteger[] baseP)
    //{
    //    if (rValue > 0) {
    //            bool isZero;
    //        BigInteger[] result = null;
    //        var r = get(rValue);
    //        //bool isZero = false;
    //        for (int i = 0; i < r.Length; i++)
    //        {
    //            if (!r[i]) {
    //                if (baseP == null) {
    //                }
    //                else {
    //                    if (result == null) {
    //                        result = baseP;
    //                    }
    //                    else {
    //                        result = pointPlus(result, baseP, out isZero);
    //                    }
    //                }
    //            }
    //            baseP = getDoubleP(baseP, out isZero);
    //        }
    //        return result;
    //    }
    //    else if (rValue.IsZero) {
    //        return null;
    //    }
    //    else {
    //        throw new Exception("privateKey的值不能为0和负数");
    //    }
    //}
}

function VerifySignature(publicKeyPoint, hash, r, s) {
    var q = yrqsecp256k1().curve.n;
    var zero = yrqBN().fromNumber(0);

    if (r.gt(q) || r.lt(zero) || s.gt(q) || s.lt(zero)) {
        return false;
    }
    var z = yrqBN().fromString(bytesToHex(hash), 'hex');
    z = z.mod(q);
    //var z = Bytes32.ConvetToBigInteger(hash);;
    var w = ModInverse(s, q);

    var u1 = z.mul(w).mod(q);
    var u2 = r.mul(w).mod(q);



    var pt = getPublicByPrivate(u1).add(getMulValue(u2, publicKeyPoint));//Calculate.pointPlus(Calculate.getPublicByPrivate(u1), Calculate.getMulValue(u2, publicKey), out isZero);// (publicKey.Multiply(u2));

    //var u1 = (z * w) % Secp256k1.q;
    //var u2 = (r * w) % Secp256k1.q;
    //  bool isZero;
    //  var pt = null;//Calculate.pointPlus(Calculate.getPublicByPrivate(u1), Calculate.getMulValue(u2, publicKey), out isZero);// (publicKey.Multiply(u2));

    if (pt == null) {
        return false;
    }
    else {
        var pmod = pt.x.mod(q);
        return pmod.cmp(r) == 0;
    }
}


function GetYByX(x) {


    // ===== secp256k1 常量 =====
    const P = yrqsecp256k1().curve.p; // 素数模
    //yrqsecp256k1().curve.p.toString(16).padStart(64, '0')
    const A = yrqBN().fromNumber(0);
    const B = yrqBN().fromNumber(7);
    //  const RED = yrqBN().red(P);
    const EXP = P.addn(1).div(yrqBN().fromNumber(4));  // (p+1)/4 
    let X = x;//BN.isBN(x) ? x.clone() : new BN(x, 16);
    X = X.mod(P);                            // 规范到 [0, p-1]

    let yy = X.mul(X).mul(X).add(B).mod(P);
    let y = yrqBN().fromNumber(1);
    var r = getBoolList(EXP);
    for (let i = 0; i < r.length; i++) {
        if (!r[i]) {
            y = (y.mul(yy)).mod(P);
        }
        yy = (yy.mul(yy)).mod(P);// % Secp256k1.p; 
    }
    return y;

    // var yy = (x * x * x + Secp256k1.a * x + Secp256k1.b) % Secp256k1.p;//(((x * x    * x) % Secp256k1.p + Secp256k1.a * x + Secp256k1.b) % Secp256k1.p;
    //var y=

    // yy = x^3 + a*x + b  (mod p)
    // 注意：bn.js 的 .pow 只接受小整数；x^3 直接连乘更清晰
    //let x2 = X.mul(X).mod(P);
    //let yy = x2.mul(X).mod(P);               // x^3
    //if (!A.isZero()) yy = yy.add(A.mul(X)).umod(P);
    //yy = yy.add(B).umod(P);

    //if (yy.isZero()) return new BN(0);        // y = 0 的特殊解

    //// y = yy^((p+1)/4) (mod p)   （仅当 p ≡ 3 (mod 4) 才成立）
    //const y = yy.toRed(RED).redPow(EXP).fromRed();

    //// 验证：y^2 ?= yy (mod p)；不等说明 x 不在曲线/yy 非二次剩余
    //if (!y.mul(y).umod(P).eq(yy)) {
    //    throw new Error('给定的 x 在曲线上无解（或不合法）。');
    //}
    //return y;
}

function GetXYByByte33(s1) {
    var Secp256k1_P = yrqsecp256k1().curve.p;
    var data = bytesToHex(s1);
    var pre = data.slice(0, 2);
    data = data.slice(2, data.length);
    var X = yrqBN().fromString(data, 'hex').mod(Secp256k1_P);
    var Y = GetYByX(X);
    if (pre == "02") {
        if (Y.isEven()) { }
        else {
            Y = ((Secp256k1_P.sub(Y).add(Secp256k1_P))).mod(Secp256k1_P);
        }
    }
    else {
        if (Y.isEven()) {
            Y = ((Secp256k1_P.sub(Y).add(Secp256k1_P))).mod(Secp256k1_P);
        }
        else { }
    }
    return [X, Y];
}



//let originalTextToSecretText;
//let secretToOriginCode;
async function DealWithFile(cur, lengthOfFs, originalTextToSecretText, materialForOrder) {
    var position = 0;
    cur.seek(position, 'begin');
    var readLength = 32;
    const titleU8 = await cur.read(readLength);
    // position =;

    const Title = new TextDecoder('utf-8').decode(titleU8);

    console.log('ascii', Title);

    if ('HardFuckThenWorkHardThenHardFuck' == Title) {

    }
    else {
        alert('文件头错误！');
        return;
    }
    {
        readLength = 4;
        cur.seek(0, 'current');
        const editionArray = await cur.read(readLength);

        if (
            editionArray[0] == 0x00 &&
            editionArray[1] == 0x00 &&
            editionArray[2] == 0x00 &&
            editionArray[3] == 0x01) {
        }
        else {
            alert('版本错误！');
            return;
        }
        let walletOfOwner;
        let notifyMsg;


        readLength = 33;
        cur.seek(0, 'current');
        const ownerArray = await cur.read(readLength);

        var xyValue = GetXYByByte33(ownerArray);
        //  walletOfOwner = PublicKeyF.GetAddressOfcompressed(publicKeyOwner);
        var addrs = p2pkhBothFromPub(xyValue);
        notifyMsg = "输入拖入地址" + addrs[1] + "或地址" + addrs[0] + "的私钥，钥匙.txt！";

        document.getElementById('sysInfo').value += notifyMsg;
        document.getElementById('sysInfo').value += '\n';
        walletOfOwner = addrs[0];


        //  int fileNameLength;
        //               string fileName;
        readLength = 1;
        cur.seek(0, 'current');
        const fileNameLength = (await cur.read(readLength))[0];
        if (fileNameLength < 1) {
            alert("文件解析错误！！！");
            return;
        }
        readLength = fileNameLength;
        const fileNameArray = await cur.read(readLength);
        const fileName = new TextDecoder('utf-8').decode(fileNameArray);

        {
            document.getElementById('sysInfo').value += '文件名为' + fileName;
            document.getElementById('sysInfo').value += '\n';
        }

        readLength = 2;
        cur.seek(0, 'current');
        const remarkRecord = await cur.read(readLength);
        var remarkLengthbyte1 = remarkRecord[0];
        var remarkLengthbyte2 = remarkRecord[1];
        const remarkLength = remarkLengthbyte1 * 256 + remarkLengthbyte2;

        readLength = remarkLength;
        var remarkArray = await cur.read(readLength);
        var remarkStr = new TextDecoder('utf-8').decode(remarkArray);

        {
            document.getElementById('sysInfo').value += remarkStr;
            document.getElementById('sysInfo').value += '\n';
        }

        var privateKey = document.getElementById('privateKeyValue').value;

        if (yrqCheckPrivateKey(privateKey)) {
            var privateKeyValueInput = privateKey;
            var bitcore = yrqbitcore();
            var privateKey = bitcore.PrivateKey.fromWIF(privateKeyValueInput);
            var privateBigInteger = privateKey.bn;
            const pubAny = privateKey.toPublicKey();              // 可能是压缩的
            const point = pubAny.point;

            const pubCompressed = bitcore.PublicKey.fromPoint(point, true);

            var walletOfOwner2 = pubCompressed.toAddress().toString();
            console.log('pubCompressed address', walletOfOwner2);
            if (walletOfOwner2 == walletOfOwner) { }
            else {
                alert('请输入正确的私钥');
                return;
            }
            position = cur.pos;
            if (lengthOfFs >= position + 66 * 256 * HardValue) {
                for (let i = 0; i < 256 * HardValue; i++)//
                {
                    //var s1 = ReadBytes(bytes, 33, ref position);// br.ReadBytes(33);
                    //var s2 = ReadBytes(bytes, 33, ref position);
                    let s1 = null;
                    readLength = 33;
                    cur.seek(0, 'current');
                    s1 = await cur.read(readLength);

                    let s2 = null;
                    cur.seek(0, 'current');
                    s2 = await cur.read(readLength);

                    var infomationOfSecretText = bytesToHex(s1);
                    console.log(i, i + '压缩公钥为' + infomationOfSecretText);

                    document.getElementById('sysInfo').value += i + '压缩公钥为' + infomationOfSecretText;
                    document.getElementById('sysInfo').value += '\n';

                    var C1 = GetXYByByte33(s1);
                    if (s2[0] == 0x02) {
                        s2[0] = 0x03;
                    }
                    else if (s2[0] == 0x03) {
                        s2[0] = 0x02;
                    }
                    else {
                        alert("解压时，解析错误！");
                        return;
                    }
                    var C2_ = GetXYByByte33(s2);//这里表示-C2
                    var C2_Point = yrqsecp256k1().curve.point(C2_[0], C2_[1]);
                    //   getMulValue()
                    var C3Point = getMulValue(privateBigInteger, C2_Point);
                    var C1Point = yrqsecp256k1().curve.point(C1[0], C1[1]);
                    // var C3Point = yrqsecp256k1().curve.point(C3[0], C3[1]);

                    var C6Point = C1Point.add(C3Point);
                    //  var bitcore = yrqbitcore();
                    // bitcore.PublicKey.fromPoint(C6Point, true);
                    var M = ComPressPoint(C6Point);

                    var key = bytesToHex(M);
                    if (!(originalTextToSecretText[key] == undefined)) {
                        //Console.WriteLine($"解压时，包含！");
                        originalTextToSecretText[key] = infomationOfSecretText;
                    }
                    else {
                        alert("解压时，不包含！{" + key + "}");
                        return;
                        // return;
                    }


                    ///////////////
                    // materialForOrder.Add(infomation);
                    //  materialToIndex.Add(infomation, key);

                    // var infomation = $"{ Hex.BytesToHex(s1)}";

                    //Console.WriteLine($"{i}压缩公钥为{infomation}");

                    // msgToByte.Add(infomation, realInfo[materialToIndex[infomation]]);
                }

                for (let i = 0; i < materialForOrder.length; i++) {
                    materialForOrder[i].infomationOfSecretText = originalTextToSecretText[materialForOrder[i].infomationOfOriginalText];
                }
                for (let i = 0; i < materialForOrder.length - 1; i++) {
                    var current = materialForOrder[i];
                    var nextItem = materialForOrder[i + 1];

                    if (current.step > nextItem.step) {
                        materialForOrder.splice(i, 1);
                        materialForOrder.push(current);
                        i = -1;
                        // continue;
                    }
                    else if (current.step == nextItem.step) {
                        if (current.infomationOfSecretText > nextItem.infomationOfSecretText) {
                            materialForOrder.splice(i, 1);
                            materialForOrder.push(current);
                            i = -1;
                        }
                    }
                }
                //alert('ok');
                //alert('ok');
                alert(materialForOrder.length);
                let secretToOriginCode = {};
                for (var mIndex = 0; mIndex < materialForOrder.length; mIndex++) {
                    let secretByte = mIndex % 256;
                    materialForOrder[mIndex].secretByte = secretByte;
                    if (materialForOrder[mIndex].infomationOfSecretText == "" || materialForOrder[mIndex].infomationOfSecretText == null) {
                        alert("解析失败");
                        return;
                    }
                    if (secretToOriginCode[secretByte] != undefined) { }
                    else {
                        secretToOriginCode[secretByte] = {};
                        //secretToOriginCode.Add(secretByte, new Dictionary < int, byte > ());
                    }
                    if (secretToOriginCode[secretByte][materialForOrder[mIndex].step] == undefined) {
                        secretToOriginCode[secretByte][materialForOrder[mIndex].step] = materialForOrder[mIndex].responRealByte;
                    }
                    else {
                        alert("解析失败");
                        return;
                    }
                }

                var fileLength =
                    lengthOfFs
                    - 32//lengthOfTitle
                    - 4//lengthOfEdition
                    - 33//sFrom.Length
                    - 1////代表文件名字符串长度byte
                    - fileNameLength//文件名字长度
                    - 2////代表备注字符串长度byte
                    - remarkLength
                    - 66 * 256 * HardValue
                    - 32
                    - 32
                    - 32
                    - 33
                    ;
                var jsonStr = JSON.stringify(secretToOriginCode);
                console.log('jsonStr', jsonStr);


                const buffer = new Uint8Array(fileLength);
                var writePosition = 0;
                var addVm = 10 * 1024 * 1024;
                var sum = 0;
                for (let i = 0; i < fileLength; i += addVm) {
                    if (i + addVm < fileLength) {
                        var addV = addVm;
                        //   cur.seek(0, 'current'); //new byte[addV];

                        const bytesToRead = await cur.read(addV);

                        let bytesToWrite = new Uint8Array(addV).fill(0);

                        for (var jj = 0; jj < bytesToRead.length; jj++) {
                            var secretByte = bytesToRead[jj];
                            var sheetIndex = sum % HardValue;
                            var realByte = secretToOriginCode[secretByte][sheetIndex];
                            sum += realByte;
                            bytesToWrite[jj] = realByte;
                        }
                        buffer.set(bytesToWrite, writePosition);
                        writePosition += bytesToWrite.length;
                    }
                    else {
                        var addV = fileLength - i;
                        //   cur.seek(0, 'current');
                        const bytesToRead = await cur.read(addV);
                        let bytesToWrite = new Uint8Array(addV).fill(0);

                        for (var jj = 0; jj < bytesToRead.length; jj++) {
                            var secretByte = bytesToRead[jj];
                            var sheetIndex = sum % HardValue;
                            var realByte = secretToOriginCode[secretByte][sheetIndex];
                            sum += realByte;
                            bytesToWrite[jj] = realByte;
                        }
                        buffer.set(bytesToWrite, writePosition);
                        writePosition += bytesToWrite.length;
                    }
                }

                const blob = new Blob([buffer], { type: 'application/octet-stream' });

                if (false) {
                    const link = document.createElement('a');
                    link.href = URL.createObjectURL(blob);
                    link.download = fileName;
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                    URL.revokeObjectURL(link.href);
                }
                else
                {
                    const player = document.getElementById('player');
                    const objectURL = URL.createObjectURL(blob);

                    // 必要属性，保证 iOS/Safari 能自动播放
                    player.muted = true;                 // 自动播放策略要求
                    player.setAttribute('playsinline', '');
                    player.setAttribute('webkit-playsinline', '');

                    // 赋值并尝试播放
                    player.src = objectURL;
                }
                //using(FileStream fsWrite = new FileStream(filePathOut, FileMode.OpenOrCreate, System.IO.FileAccess.Write, FileShare.Write))
                //{
                //                    long writePosition = 0;
                //                    long addVm = 10 * 1024 * 1024;
                //    for (long i = 0; i < fileLength; i += addVm)
                //    {
                //        if (i + addVm < fileLength) {
                //            var addV = addVm;
                //            byte[] bytesToRead = new byte[addV];
                //            dealWithData(fs, ref position, bytesToRead);

                //            byte[] bytesToWrite = new byte[addV];
                //            for (var jj = 0; jj < bytesToRead.Length; jj++) {
                //                var secretByte = bytesToRead[jj];
                //                var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                //                var realByte = secretToOriginCode[secretByte][sheetIndex];
                //                sum += Convert.ToInt32(realByte);
                //                bytesToWrite[jj] = realByte;
                //            }
                //            fsWrite.Seek(writePosition, SeekOrigin.Begin);
                //            fsWrite.Write(bytesToWrite, 0, bytesToWrite.Length);
                //            writePosition += bytesToWrite.Length;
                //        }
                //        else {
                //            var addV = fileLength - i;

                //            byte[] bytesToRead = new byte[addV];
                //            dealWithData(fs, ref position, bytesToRead);

                //            byte[] bytesToWrite = new byte[addV];
                //            for (var jj = 0; jj < bytesToRead.Length; jj++) {
                //                var secretByte = bytesToRead[jj];
                //                var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                //                var realByte = secretToOriginCode[secretByte][sheetIndex];
                //                sum += Convert.ToInt32(realByte);
                //                bytesToWrite[jj] = realByte;
                //            }
                //            fsWrite.Seek(writePosition, SeekOrigin.Begin);
                //            fsWrite.Write(bytesToWrite, 0, bytesToWrite.Length);
                //            writePosition += bytesToWrite.Length;
                //        }
                //        //List<byte> bytesToWrite = new List<byte>;
                //        // dealWithData(fs, ref position)
                //        //   var secretByte = ReadByte(bytes, ref position);
                //        //  var sheetIndex = Convert.ToInt32(sum % PublicKeyComPress.HardValue);
                //        //  var realByte = secretToOriginCode[secretByte][sheetIndex];
                //        //   sum += Convert.ToInt32(realByte);
                //        //fileBinary[i] = realByte;
                //    }
                //}

            }
            else {
                alert('文件异常');
            }
        }
        else {
            alert('输入正确格式的私钥');
            return;
        }
        {
            //  int remarkLength;
            //{
            //    byte[] remarkLengthbytes = new byte[2];
            //    dealWithData(fs, ref position, remarkLengthbytes);

            //                byte remarkLengthbyte1 = remarkLengthbytes[0];
            //                byte remarkLengthbyte2 = remarkLengthbytes[1];

            //    remarkLength = Convert.ToInt32(remarkLengthbyte1) * 256 + Convert.ToInt32(remarkLengthbyte2);

            //    var remarkArray = new byte[remarkLength];
            //    dealWithData(fs, ref position, remarkArray);

            //    Console.WriteLine($"{ UTF8Encoding.UTF8.GetString(remarkArray)}");


            //}
        }
        //   dealWithData(fs, ref position, ownerArray);
        //var editionArray ;
        //dealWithData(fs, ref position, editionArray);
        //byte[] edition = new byte[] { 0x00, 0x00, 0x00, 0x01 };
        //if (Bytes32.ByteArrayEqual(edition, editionArray)) {
        //    Console.WriteLine("版本检验正确！！！");
        //}
        //else {
        //    Console.WriteLine($"版本检验错误！");
        //    return;
        //}
    }

}

const HardValue = 5;

export async function decrypt(file) {




    // public static System.Numerics.BigInteger[] GetXYByByte33(byte[] s1, out bool isRight)
    //{
    //    var data = Hex.BytesToHex(s1);
    //    var pre = data.Substring(0, 2);
    //    data = data.Substring(2, data.Length - 2);
    //    // Console.WriteLine($"{data}");
    //    var X = HexToBigInteger.inputHex(data) % Secp256k1.p;
    //    var Y = Calculate.GetYByX(X);
    //    if (pre == "02") {
    //        if (Y.IsEven) { }
    //        else {
    //            Y = ((Secp256k1.p - Y) + Secp256k1.p) % Secp256k1.p;
    //        }
    //    }
    //    else {
    //        if (Y.IsEven) {
    //            Y = ((Secp256k1.p - Y) + Secp256k1.p) % Secp256k1.p;
    //        }
    //        else { }
    //    }
    //    //  Console.WriteLine($"计算Y得为{ HexToBigInteger.bigIntergetToHex(Y) }");
    //    if (Calculate.CheckXYIsRight(X, Y)) {
    //        isRight = true;
    //        return new System.Numerics.BigInteger[] { X, Y };
    //    }
    //    else {
    //        isRight = false;
    //        return null;
    //    }
    //}


    var isRight = false;

    let originalTextToSecretText = {};

    //  let secretToOriginCode = {};
    //Dictionary < byte, Dictionary < int, byte >> secretToOriginCode = new Dictionary < byte, Dictionary < int, byte >> ();

    let materialForOrder = []
    for (var i = 0; i < 256 * HardValue; i++)//
    {
        var M = yrqGetPrivateKeyByNumber(i + 1).publicKey;
        var sM = ComPressPublic(M);

        var infomationOfOriginalText = bytesToHex(sM);
        materialForOrder.push(new InfomationClass(infomationOfOriginalText, Math.floor(i / 256), i % 256));
        originalTextToSecretText[infomationOfOriginalText] = "";
    }

    var materialToIndex = {};


    const reader = file.stream().getReader(); // 按块读 Uint8Array
    const total = file.size;
    let loaded = 0;


    console.log('文件大小=', total);

    var lengthOfFs = total;
    let hash1 = [], r = [], s = [], publicKeyS1 = [];
    let hashCode = [];
    let cur = null;
    if (lengthOfFs > 130) {
        cur = new BlobCursor(file);
        cur.seek(-129, 'end');          // 从文件末尾往回 129 字节
        const tail129 = await cur.read(32);
        hash1 = tail129.subarray(0, 32);

        cur.seek(-97, 'end');          // 从文件末尾往回 129 字节
        const tail97 = await cur.read(32);
        r = tail97.subarray(0, 32);
        //  return tail129; // Uint8Array

        cur.seek(-65, 'end');          // 从文件末尾往回 129 字节
        const tail65 = await cur.read(32);
        s = tail65.subarray(0, 32);

        cur.seek(-33, 'end');
        const tail33 = await cur.read(33);
        publicKeyS1 = tail33.subarray(0, 33);

        var intValue = [];
        for (var i = 0; i < 129; i++) {
            intValue.push(0);
        }
        let buf = Uint8Array.from(intValue.map(x => x & 0xff));
        cur.seek(-129, 'end');
        await cur.write(buf, 0, 129);

        const newBlob = cur.getBlob();
        const ab = await newBlob.arrayBuffer();
        const u8 = new Uint8Array(ab);
        hashCode = yrqHash()(u8);
    }
    else {
        alert('文件长度不够');
        return;
    }

    console.log('hashCode', hashCode);

    if (ByteArrayEqual(hashCode, hash1)) {
        // alert('文件校验成功');
        var rNum = yrqBN().fromString(bytesToHex(r), 'hex');
        var sNum = yrqBN().fromString(bytesToHex(s), 'hex');
        //var rNum = Bytes32.ConvetToBigInteger(r);
        //var sNum = Bytes32.ConvetToBigInteger(s);
        // alert('文件校验成功');

        var xyValue = GetXYByByte33(publicKeyS1);
        if (yrqsecp256k1().curve.validate(yrqsecp256k1().curve.point(xyValue[0], xyValue[1]))) {
            var publicKeyPoint = yrqsecp256k1().curve.point(xyValue[0], xyValue[1]);
            var vSuccess = VerifySignature(publicKeyPoint, hash1, rNum, sNum);
            if (vSuccess) {
                alert('检验成功');

                var addrs = p2pkhBothFromPub(xyValue)

                document.getElementById('sysInfo').value += '校验成功，签名来自' + addrs[0] + '或' + addrs[1] + '！';
                document.getElementById('sysInfo').value += '\n';
                let ccc = 0;
                ccc++;

                DealWithFile(cur, lengthOfFs, originalTextToSecretText, materialForOrder);
                //  string walletOfcompressed = PublicKeyF.GetAddressOfcompressed(publicKeyC1);
                ////Console.WriteLine($"压缩钱包地址为：{walletOfcompressed}");
                //var walletOfUncompressed = PublicKeyF.GetAddressOfUncompressed(publicKeyC1);
                ////Console.WriteLine($"非压缩钱包地址为：{walletOfUncompressed}");
                //Console.WriteLine($"校验成功，签名来自{walletOfcompressed}或{walletOfUncompressed}！");
            }
        }

    }
    else {
        alert('文件校验失败');
        return;
    }
    // 示例：统计前 100 个字节 & 计算总字节和（演示迭代）
    const first100 = [];
    let checksum = 0n; // 用 BigInt 防止溢出

    let count = 0;

    while (true) {
        const { value, done } = await reader.read();
        if (done) break;

        const chunk = value; // Uint8Array
        for (let i = 0; i < chunk.length; i++) {
            const byte = chunk[i]; // 👈 这里就是一个字节 (0~255)

            if (count < 200 || total - count < 200) {
                console.log(`第 ${count} 字节: ${byte}`);
            }
            count++;
        }



    }
}