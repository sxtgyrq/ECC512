



let test=false;
if(test){
    const LockTimeScriptHashAddr= require('./LockTimeScriptHashAddrv2.js')
    var privateKey='cMmmWc5ZptX5TwsZjw32F7KTBkLFFXJZRVfzeXLq9eoNyuSiQLY5';//args[1];
    //cMmmWc5ZptX5TwsZjw32F7KTBkLFFXJZRVfzeXLq9eoNyuSiQLY5
    const redeemScriptHex= '0320830cb17576a914fd7ac2296b8600dcbc864e54d6497f8e30940cd088ac';
    LockTimeScriptHashAddr.LockTimeScriptHashAppr.generateTranstraction(
        privateKey,
        redeemScriptHex)   
        console.log('redeemScript',redeemScript);
       

}
else
{
    const args = process.argv.slice(2);

    console.log(args);
    const LockTimeScriptHashApprMainNet= require('./LockTimeScriptHashAddrv3.js')
    const privateKey=args[0];//args[1];
    const redeemScriptHex=args[1];//args[1]; 
    LockTimeScriptHashApprMainNet.LockTimeScriptHashApprMainNet.generateTranstraction(
        privateKey,
        redeemScriptHex);   
    console.log('redeemScript',redeemScript);
}







// for(i=0;i<args.length;i++)
// {


// }


// switch(args[0])
// {
//     case "test":
//         {
//             const LockTimeScriptHashAddr= require('./LockTimeScriptHashAddr.js');
//             LockTimeScriptHashAddr.LockTimeScriptHashAppr.generateRandomPrivake();

//         };break;
//     case "product": 
//      { 
        

//      };break;
// }

