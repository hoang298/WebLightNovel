$(document).ready(function () {
    const user_id = $('#user').data('user_id');
    const avatar = $('#user').data('user_avatar');
    const user_name = $('#user').data('user_name');
    //thêm cmt
    let content_cmt = document.getElementById("textarea-cmt");
    const story_id = $('.detail__information-title').attr('data-story_id');
    const story_name = $('.detail__information-title a').text();
    $('#submit-cmt').on('click', function (e) {
        e.preventDefault();
        let content = content_cmt.value;
        id_parent = 0;
        const commentObj = {
            user_id: user_id,
            receiver_id: "",
            avatar: avatar,
            user_name: user_name,
            story_id: story_id,
            story_name: story_name,
            content: content,
            id_parent: id_parent,          
        };
        const jsonComment = JSON.stringify(commentObj);
        $.ajax({
            url: '/Story/AddComment',
            data: { jsonComment },
            contentType: "application/json",
            success: function (data) {
                $('.list-comment').append(data);
                content_cmt.value = "";
                $('#submit-cmt').removeClass('changeButtonCmt');
                window.requestAnimationFrame(rep_cmt);
                window.requestAnimationFrame(updateComment);
            }
        });
    });
    let list_cmt = document.getElementsByClassName('ln-comment-group');//list các comment
    let array_rep = new Array();
    let map_cmt_inputRepCmt = new Map();// gắn mỗi cmt parent với ô input rep cmt tương ứng
    let array_html_input = new Array(); // quản lý các ô input có trên trang
    let array_html_button = new Array();// quản lý các button gửi rep cmt có trên trang
    let array_userIdRec = new Array();
    function insertInputCmt(k, toUser, toUserId) {
        if (array_rep[k] == undefined) {// ktra để không cho người dùng bật nhiều ô input trong cùng 1 cmt  
            let id_parent = list_cmt[k].getAttribute('data-id-parent');
            $(list_cmt[k]).append('<form id = "' + id_parent + '" class="form-reply"><textarea class= "form-comment"></textarea><button class = "btn-reply"type="button"/>Trả lời</button></form>');
            let input_cmt = list_cmt[k].getElementsByClassName("form-comment");// lấy input vừa đc thêm ở trên
            let button_cmt = list_cmt[k].getElementsByClassName("btn-reply");// lấy button vừa đc thêm ở trên
            if (user_id != toUserId)
                input_cmt[0].value = "@" + toUser + ": ";
            input_cmt[0].focus();
            input_cmt[0].selectionStart = input_cmt[0].selectionEnd = input_cmt[0].value.length;
            array_html_input.push(input_cmt[0]);
            array_html_button.push(button_cmt[0]);
            array_userIdRec.push(toUserId);
            array_rep[k] = 1;
            map_cmt_inputRepCmt.set(button_cmt[0], k);// gắn kết cmt parent vs form input vừa đc tạo ở trên
            console.log(array_userIdRec);
        }

    }
    function insertCmt() {
        for (let i = 0; i < array_html_button.length; i++) {
            array_html_button[i].onclick = function () {
                let k = map_cmt_inputRepCmt.get(array_html_button[i]); // lấy ra cmt parent với key tương ứng                  
                let content_reply = array_html_input[i].value;
                let id_parent = list_cmt[k].getAttribute('data-id-parent');
                const commentObj = {
                    user_id: user_id,
                    receiver_id: array_userIdRec[i],
                    avatar: avatar,
                    user_name: user_name,
                    story_id: story_id,
                    story_name: story_name,
                    content: content_reply,
                    id_parent: id_parent,
                };
                const jsonComment = JSON.stringify(commentObj);
                $.ajax({
                    url: "/Story/AddComment",
                    data: { jsonComment },
                    contentType: "application/json",
                    data: {jsonComment},
                    success: function (data) {
                        $(list_cmt[k]).append(data);
                        let form = list_cmt[k].getElementsByClassName("form-reply");// lấy ra form hiện tại đang đc thao tác
                        $(form[0]).remove();
                        array_rep[k] = undefined;
                        map_cmt_inputRepCmt.delete(array_html_button[i]);// xóa các liên kết tương ứng và cập nhật mảng input và button
                        let index = array_html_button.indexOf(array_html_button[i]);
                        array_html_input.splice(index, 1);
                        array_html_button.splice(index, 1);
                        window.requestAnimationFrame(rep_cmt);
                        window.requestAnimationFrame(updateComment);
                    },

                });

            }
        }
    }
    function rep_cmt() {
        let count_cmt_parent = list_cmt.length;
        for (let k = 0; k < count_cmt_parent; k++) {// duyệt các cmt cha
            let list_repcmt = list_cmt[k].getElementsByClassName('ln-comment-text-reply');
            for (let i = 0; i < list_repcmt.length; i++) {//duyệt các cmt con của từng cha
                list_repcmt[i].onclick = function () {
                    let user_nameRec = list_repcmt[i].getAttribute('name');
                    let user_IdRec = list_repcmt[i].getAttribute('data-user_id')
                    insertInputCmt(k, user_nameRec, user_IdRec);
                    window.requestAnimationFrame(insertCmt);
                }
            }
        }
        insertCmt();
    }
    function updateComment() {
        let checkEditCmt = document.getElementsByClassName('ln-comment-edit');// bảng chỉnh sửa/xóa
        let clickIconEdit = document.getElementsByClassName('ln-comment-icon');// các nút để bật bảng chỉnh sửa/xóa
        let buttonEdit = document.getElementsByClassName('ln-comment-edit-span');// nút edit
        let edit_text = document.getElementsByClassName('ln-comment-text');
        let buttonDeleteCmt = document.getElementsByClassName('ln-comment-delete');
        let userName = document.getElementsByClassName('ln-comment-user-name');
        let visible = document.getElementsByClassName('visible-toolkit');
        for (let i = 0; i < clickIconEdit.length; i++) {
            clickIconEdit[i].onclick = function () {
                let id_cmt = checkEditCmt[i].getAttribute('data-id-comment');
                $(document).on('click', function (e) {//tìm checkEditCmt[i] gần vị trí click chuột nhất
                    if ($(e.target).closest(checkEditCmt[i]).length == 0 && $(e.target).closest(clickIconEdit[i]).length == 0) {
                        $(checkEditCmt[i]).css('display', 'none');
                    }
                });
                var us_id = checkEditCmt[i].getAttribute('data-user_id');
                if (user_id == us_id) {
                    $(checkEditCmt[i]).toggle();
                }
                buttonEdit[i].onclick = function () {
                    edit_text[i].setAttribute('contenteditable', 'true');
                    $(edit_text[i]).css('outline', '0px solid transparent');
                    $(edit_text[i]).focus();
                    $(userName[i]).css('display', 'none');
                    $(visible[i]).css('display', 'none');
                }
                buttonDeleteCmt[i].onclick = function () {
                    $.ajax({
                        url: "/Story/DeleteComment",
                        data: { id_cmt: id_cmt },
                        success: function () {
                            let cmt = document.getElementById(id_cmt);
                            cmt.remove();
                            window.requestAnimationFrame(updateComment);
                            window.requestAnimationFrame(rep_cmt);
                        }

                    });
                }
            }
        }
    }

    rep_cmt();
    updateComment();
    const list_more_reply = document.getElementsByClassName('more-comment-reply');
    const list_img_loading = document.getElementsByClassName('img-loading');
    for (let i = 0; i < list_more_reply.length; i++) {
        list_more_reply[i].addEventListener('click', function () {
            let comment_id = list_more_reply[i].getAttribute('id');
            list_img_loading[i].style.display = 'block';
            $.ajax({
                url: '/Story/MoreCommentReply',
                data: { comment_id: comment_id },
                success: function (data) {
                    data.forEach(function (item1) {
                        let element_parent = document.getElementById(comment_id);
                        let element = '<div data-id-parent="' + comment_id + '" id="' + item1.comment_id + '" class="ln-comment-reply">' +
                            '<div class="ln-comment-user-avt">' +
                            '<img src="/avatar/' + item1.avatar + '" alt="Avatar" />' +
                            '</div>' +
                            '<div class="ln-comment-info">' +
                            '<div class="ln-comment-wrapper">' +
                            '<div class="ln-comment-user-name">' +
                            '<a>' + item1.user_name + '</a>' +
                            '</div>' +
                            '<div contenteditable="false" class="ln-comment-text">' + item1.content + '</div>' +
                            '</div>' +
                            '<div class="visible-toolkit">' +
                            '<span style="color:#9191c5" class="ln-comment-time">' + item1.time + '</span>' +
                            '<a data-user_id="' + item1.user_id + '" name="' + item1.user_name + '" class="ln-comment-text-reply">Trả lời</a>' +
                            '</div>' +
                            '</div>' +
                            '<div class="ln-comment-menu">' +
                            '<div class="ln-comment-icon">' +
                            '<i class="fas fa-angle-down"></i>' +
                            '</div>' +
                            '<div data-id-comment="' + item1.comment_id + '" data-user_id="' + item1.user_id + '" class="ln-comment-edit">' +
                            '<span class="ln-comment-edit-span">' +
                            '<i class="fas fa-edit"> </i>' +
                            'Chỉnh sửa' +
                            '</span>' +
                            '<span class="ln-comment-delete">' +
                            ' <i class="fas fa-times"> </i>' +
                            'Xóa' +
                            '</span>' +
                            '</div>' +
                            '</div>' +
                            '</div>';
                        let elementChil = document.createElement('div');
                        elementChil.innerHTML = element;
                        element_parent.appendChild(elementChil);
                        rep_cmt();
                        updateComment();
                        list_more_reply[i].style.display = 'none';
                        list_img_loading[i].style.display = 'none';
                    });
                },
                error: function (error) {
                    alert('Đã xảy ra lỗi:', error);
                }
            });
        })
    }
})