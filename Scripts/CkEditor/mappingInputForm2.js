CKEDITOR.replace('content');
const form = document.getElementById('myForm');
const input_content = document.getElementById('content');
form.addEventListener('submit', function (event) {
    for (var instanceName in CKEDITOR.instances)
        CKEDITOR.instances[instanceName].updateElement();
    const list_p = input_content.querySelectorAll('p');
    for (let i = 0; i < list_p.length; i++) {
        let text = list_p[i].outerHTML.replace('<p>', '');
        text = text.replace('</p>', "#LineBreak");
        input_content.value += text;
    }
});
