'use strict';

const readline = require('readline');

const imputPrivateKeyAndLockTime=function(){
  const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
  });

  const r2 = readline.createInterface({
    input: process.stdin,
    output: process.stdout
  });
  rl.question('请输入您的私钥: ', (Wif) => {
   // console.log(`您好, ${name}!`);
   r2.question('请输入您的私钥: ', (locktime) => {
      console.log(`您好, ${Wif},${locktime}`);
   
     // 不要忘记关闭接口！
     r2.close();
   });
    // 不要忘记关闭接口！
    rl.close();
  });
} 

exports.imputPrivateKeyAndLockTime =  imputPrivateKeyAndLockTime; 