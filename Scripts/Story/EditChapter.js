var editor = CKEDITOR.replace('content');
const form = document.getElementsByTagName('form')[0];
let input_content = document.getElementById('content').value;
input_content = input_content.replaceAll("@LineBreak", "\n");
let list_content = input_content.split('\n');
let result = "";
for (let i = 0; i < list_content.length; i++) {
    list_content[i] = "<p>" + list_content[i] + "</p>";
    result += list_content[i];
}
editor.setData(result);
let input_content1 = document.getElementById('content');
let temp = "";
form.addEventListener('submit', function (event) {
    for (var instanceName in CKEDITOR.instances)
        CKEDITOR.instances[instanceName].updateElement();
    const list_p = input_content1.querySelectorAll('p');
    for (let i = 0; i < list_p.length; i++) {
        if (list_p[i].textContent.trim() != "") {
            let text = list_p[i].outerHTML.replace('<p>', '');
            text = text.replaceAll('</p>', "#LineBreak");
            temp += text;
        }
       
    }
    input_content1.value = temp;
   
});
document.getElementById('click_me').addEventListener('click', function () {
    for (var instanceName in CKEDITOR.instances)
        CKEDITOR.instances[instanceName].updateElement();
    const list_p = input_content1.querySelectorAll('p');
    let r = "";
    for (let i = 0; i < list_p.length; i++) {
        if (list_p[i].textContent.trim() != "") {
            let text = list_p[i].outerHTML.replace('<p>', '');
            text = text.replaceAll('</p>', "#LineBreak");
            r += text;
        }

    }
    input_content1.value = r;
    console.log(input_content1)
})

