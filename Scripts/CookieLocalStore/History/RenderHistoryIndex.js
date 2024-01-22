    let listHistory = "";
    if (localStorage.getItem("history") != null) {
        let history = JSON.parse(localStorage.getItem("history"));
        let count = 0;
        for (let i = history.length - 1; i >= 0; i--) {
            if (count > 4) 
                break;           
            listHistory += '<li class="history__item">' +
                '<div class="history__item-wrapper">' +
                '<a href="' + history[i].story_url + '">' +
                '<img src="' + history[i].story_image + '" alt="">' +
                '</a>' +
                '</div>' +
                '<div class="history__item-caption">' +

                '<div class="history__item-caption-title">' +
                '<a class="overflowContent" href="' + history[i].story_url + '">' + history[i].story_name + '</a>' +
                '</div>' +
                '<div class="history__item-option">' +
                '<a class="history__item-name overflowContent" href="' + history[i].chapter_url + '">' +
                history[i].chapter_name +
                '</a>' +
                '<a id="' + history[i].story_id + '" class="history__btn-delete" href="#">' +
                '<i class="fa fa-times"></i>' +
                'Xóa' +
                '</a>' +

                '</div>' +
                '</div>' +
                '</li>';
            count++;
        }
        document.getElementsByClassName('list__manga-content')[0].innerHTML = listHistory;
    }

    const btn_delete = document.getElementsByClassName('history__btn-delete');
    const list_history__item = document.getElementsByClassName('history__item');
    let history = JSON.parse(localStorage.getItem("history"));
    for (let i = btn_delete.length - 1; i >= 0; i--) {
        btn_delete[i].addEventListener('click', function (e) {
            let story_id = btn_delete[i].getAttribute('id');
            e.preventDefault();
            let index = history.findIndex(item => item.story_id == story_id);
            if (index != -1) {
                history.splice(index, 1);
            }
            list_history__item[i].style.display = 'none';
            localStorage.setItem("history", JSON.stringify(history));
        })
    }
