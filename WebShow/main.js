import { yrqGetRandomPrivateKey } from './signandverify_simple.js';
import { decrypt } from './Deciphering.js';


let selectedFile;
$(document).ready(function () {
    // alert('11');

    const generatePrivateKeyBtn = document.getElementById('generatePrivateKeyBtn');

    const privateKeyTextarea = document.getElementById('privateKeyValue');

    function handleCanvasClick(event) {
        var privateKey = yrqGetRandomPrivateKey();
        alert(privateKey);
        privateKeyTextarea.value = privateKey;
    }

    generatePrivateKeyBtn.addEventListener('click', handleCanvasClick);



    const decryptBtn = document.getElementById('decryptBtn');

    // const privateKeyTextarea = document.getElementById('privateKeyTextarea');

    function decryptBtnClick(event) {
        var privateKey = yrqGetRandomPrivateKey();
        alert(privateKey);
    }

    decryptBtn.addEventListener('click', decryptBtnClick);



    document.getElementById("fileInput").addEventListener("change", async (event) => {
        selectedFile = event.target.files[0]; // 保存用户选择的文件
        if (selectedFile) {
            console.log("已选择文件:", selectedFile.name);
            decrypt(selectedFile);
        }
    });


    async function downloadFile(url, fileName) {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`下载失败：${response.status} ${response.statusText}`);
        }
        const blob = await response.blob();
        return new File([blob], fileName || "downloaded.mp4", { type: blob.type });
    }
    const downloadBtn = document.getElementById('downloadBtn');

    async function downloadBtnClick(event) {
        var url = "https://fuxitp.oss-cn-beijing.aliyuncs.com/secVedio/N2bak.secr";
        selectedFile = await downloadFile(url, "aa.mp4");
        decrypt(selectedFile);
    }
    downloadBtn.addEventListener('click', downloadBtnClick);

});