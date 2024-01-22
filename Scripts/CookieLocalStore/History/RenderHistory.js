function RenderStoryBrowser() {
    let listHistory = "";
    if (localStorage.getItem("history") != null) {
        let history = JSON.parse(localStorage.getItem("history"));
        for (let i = history.length - 1; i >= 0; i--) {
            listHistory += '<div class="manga__item">' +
                '<div class="manga__item-img">' +
                '<a href="' + history[i].story_url + '">' +
                '<img src="' + history[i].story_image + '"/>' +
                '</a>' +
                '<div class="manga__item-view2">' +
                '<a id="' + history[i].story_id + '" class="history__btn-delete_browser history__btn-delete2" href="#">' +
                '<i class="fa fa-times"></i>' +
                ' Xóa' +
                '</a>' +
                '</div>' +
                '</div>' +
                '<div class="manga__item-caption">' +
                '<h3 class=\"manga__item-title\">' +
                '<a href="' + history[i].story_url + '">' + history[i].story_name + '</a>' +
                '</h3>' +
                '<ul class="manga__item-history">' +
                '<li class="item__history-chapter ">' +
                '<a class="overflowContent" href="' + history[i].chapter_url + '">' + history[i].chapter_name + '</a>' +
                '</li>' +
                '</ul>' +
                '</div>' +
                '</div>';
        }
        document.getElementsByClassName('list__manga-wrapper')[0].innerHTML = listHistory;
    }
}
RenderStoryBrowser();
let btn_delete_browser = document.getElementsByClassName('history__btn-delete_browser');
let list_history__item = document.getElementsByClassName('manga__item');
let btn_delete_account;
let history = JSON.parse(localStorage.getItem("history"));
for (let i = btn_delete_browser.length - 1; i >= 0; i--) {
    let story_id = btn_delete_browser[i].getAttribute('id');
    btn_delete_browser[i].addEventListener('click', function () {
        let index = history.findIndex(item => item.story_id == story_id);
        if (index != -1) {
            history.splice(index, 1);
        }
        list_history__item[i].style.display = 'none';
        localStorage.setItem("history", JSON.stringify(history));
    })
}
const browser = document.getElementById('browser');
const account = document.getElementById('account');
browser.addEventListener('change', function () {
    if (browser.checked == true) {
        RenderStoryBrowser();
    }
});
account.addEventListener('change', function () {
    if (account.checked == true) {
        $.ajax({
            url: "/Home/RenderHistory",
            success: function (data) {
                document.getElementsByClassName('list__manga-wrapper')[0].innerHTML = data;
                btn_delete_account = document.getElementsByClassName('history__btn-delete_account');
                list_history__item = document.getElementsByClassName('manga__item');
                for (let i = 0; i < btn_delete_account.length; i++) {
                    btn_delete_account[i].addEventListener('click', function () {
                        let id = btn_delete_account[i].getAttribute('id');
                        $.ajax({
                            url: "/Home/DeleteHistory",
                            data: { id: id },
                            success: function () {
                                list_history__item[i].style.display = 'none';
                            }
                        })
                    })
                }
            }
        });
    }

});