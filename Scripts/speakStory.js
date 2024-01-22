const btn__speak = document.getElementById('btn__speak');
const listP = document.getElementsByTagName('p');
let content = "";
const charSpecial = ['~', '`', '@', '$', '%', '[', ']', '{', '}', '?', '/', '^', '&', '*', ','];
for (let i = 0; i < listP.length; i++) {
    content += listP[i].innerText + '\n';
}
/*for (let i = 0; i < charSpecial.length; i++) {
    content = content.replace(charSpecial[i], "");
}*/
/*content = content.replace(/[^A-Za-z0-9\sàáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ\.]/g, "");
content = content.replace("  ", "");*/
let synth = window.speechSynthesis;
let utterance = new SpeechSynthesisUtterance(content);
utterance.lang = "vi-VN";
utterance.volume = 1;
utterance.rate = 0.8;
utterance.pitch = 1;
let i = 1;
let temp = "<mark>" + listP[0].innerText + "</mark";
listP[0].outerHTML = listP[0].outerHTML.replace(listP[0].innerText, temp);
btn__speak.addEventListener('click', function () {
    if (btn__speak.innerText == "Play") {
        btn__speak.innerText = "Pause";
        btn__speak.style = "background-color:blue";
        //responsiveVoice.speak(content, 'Vietnamese Female');
        synth.speak(utterance);
        synth.resume();
        utterance.addEventListener('boundary', event => {
            const startIndex = event.charIndex;
            const nextSpace = content.indexOf('\n', startIndex);
            const result = content.substring(startIndex, nextSpace);
            if (result == listP[i].innerText) {
                //doi mau dong hien tai
                temp = "<mark>" + result + "</mark>";
                listP[i].outerHTML = listP[i].outerHTML.replace(result, temp);
                // xoa mau cua dong trc do
                temp = "<mark>" + listP[i - 1].innerText + "</mark>";
                listP[i - 1].outerHTML = listP[i - 1].outerHTML.replace(temp, listP[i - 1].innerText);
                i++;
            }         
        });
    }
    else {
        btn__speak.innerText = "Play";
        btn__speak.style = "background-color:#36a189";
        synth.pause();
    }

})
document.getElementById('btn__replay').addEventListener('click', function () {
    synth.cancel();
    temp = "<mark>" + listP[0].innerText + "</mark";
    listP[0].outerHTML = listP[0].outerHTML.replace(listP[0].innerText, temp);
    temp = "<mark>" + listP[i - 1].innerText + "</mark>";
    listP[i - 1].outerHTML = listP[i - 1].outerHTML.replace(temp, listP[i - 1].innerText);
    i = 1;
    synth.speak(utterance);  
    btn__speak.innerText = "Pause";
})
const speak_volume = document.getElementById('speak_volume');
const speak_range = document.getElementById('speak_range');
const speak_pitch = document.getElementById('speak_pitch');
speak_volume.oninput = function () {
    document.getElementById('volume_text').innerText = speak_volume.value;
    utterance.volume = speak_volume.value;
}
speak_range.oninput = function () {
    document.getElementById('range_text').innerText = speak_range.value;
    utterance.rate = speak_range.value;
}
speak_pitch.oninput = function () {
    document.getElementById('pitch_text').innerText = speak_pitch.value;
    utterance.pitch = speak_pitch.value;
}

window.addEventListener('beforeunload', function (event) {
    synth.cancel();
});


