// tao hieu ung truot cho slider
const banner__list = document.getElementById('banner__list');
const time_waitting = 2000;
for (let i = 0; i < document.getElementsByClassName('banner__item').length; i++) {
    document.getElementsByClassName('banner__item')[i].style.width = document.getElementsByClassName('banner__wrapper')[0].offsetWidth / 5 + 'px';

}
const banner__item_width = document.getElementsByClassName('banner__item')[0].offsetWidth;
const banner__item_length = document.getElementsByClassName('banner__item').length;
let index = 0;
let pos = 0;
let intervalID = setInterval(slider_bar, 2000);
function slider_bar() {
    if (index >= 5) {
        pos = 0;
        index = 0;
        banner__list.style = 'transform: translateX(' + pos + 'px' + ')';
    }
    else {
        pos -= banner__item_width;
        index++;
        banner__list.style = 'transform: translateX(' + pos + 'px' + ')';
    }
}
// button tren slider
const banner__wrapper_height = document.getElementsByClassName('banner__wrapper')[0].offsetHeight;
const banner__btn_next = document.getElementById('btn_next');
const banner__btn_prev = document.getElementById('btn_prev');
const btn_next_height = banner__btn_next.offsetHeight;
banner__btn_next.style.top = banner__wrapper_height / 2 - btn_next_height / 2 + 'px';
banner__btn_prev.style.top = banner__wrapper_height / 2 - btn_next_height / 2 + 'px';
document.cookie = "username=framgia; expires=Thu, 18 Dec 2016 12:00:00 UTC";
banner__btn_next.onclick = function () {
    clearInterval(intervalID);
    if (index >= 5) {
        pos = 0;
        index = 0;
        banner__list.style = 'transform: translateX(' + pos + 'px' + ')';
    }
    else {
        pos -= banner__item_width;
        index++;
        banner__list.style = 'transform: translateX(' + pos + 'px' + ')';
    }

    console.log(document.cookie.length)

    intervalID = setInterval(slider_bar, time_waitting);
}
banner__btn_prev.onclick = function () {
    clearInterval(intervalID);
    if (index <= 0) {
        index = 5;
        pos = -index * banner__item_width;
        banner__list.style = 'transform: translateX(' + pos + 'px' + ')';
    }
    else {
        pos += banner__item_width;
        index--;
        banner__list.style = 'transform: translateX(' + pos + 'px' + ')';
    }
    intervalID = setInterval(slider_bar, time_waitting);
}


