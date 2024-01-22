const chapter_info = document.getElementById('chapter_info');
const chapter_id = chapter_info.getAttribute('data-chapter_id');
const chapter_url = chapter_info.getAttribute('data-chapter_url');
const story_url = chapter_info.getAttribute('data-story_url');
const chapter_name = chapter_info.getAttribute('data-chapter_name').trim();
const story_name = chapter_info.getAttribute('data-story_name').trim();
const story_id = chapter_info.getAttribute('data-story_id').trim();
const story_image = chapter_info.getAttribute('data-story_image');
const story = {
    story_id: story_id,
    chapter_url: chapter_url,
    story_url: story_url,
    chapter_name: chapter_name,
    story_name: story_name,
    story_image: story_image,
    time: new Date()
};
function getCookie(cookieName) {
    let cookies = document.cookie.split(';');
    // Duyệt qua từng cặp key=value và tìm giá trị của cookie cần lấy
    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i].trim(); // Xóa khoảng trắng ở đầu và cuối chuỗi
        let cookieParts = cookie.split('=');

        // Kiểm tra xem cookie có phải là cookie cần lấy không
        if (cookieParts[0] === cookieName) {
            return decodeURIComponent(cookieParts[1]); // Trả về giá trị của cookie
        }
    }
    return "";
}
function addStory() {
    // Lấy danh sách truyện đã đọc từ localStorage
    let history = new Array();
    let exist = false;
    if (localStorage.getItem("history") != null) {
        history = JSON.parse(localStorage.getItem("history"));
        // Kiểm tra xem truyện đã có trong lịch sử chưa
        for (let i = 0; i < history.length; i++) {
            if (history[i].story_id == story_id) {
                history.splice(i, 1);
                history.push(story);
                localStorage.setItem("history", JSON.stringify(history));
                exist = true;
                break;
            }
        }
    }
    console.log(document.cookie)
    // Nếu truyện chưa có trong lịch sử thì thêm mới
    if (!exist && story != null) {
        // Giới hạn số lượng truyện lưu trong lịch sử là 10
        /*if (history.length === 10) {
            history.shift();
        }*/
        history.push(story);
        // Lưu danh sách truyện đã đọc vào Cookie
        localStorage.setItem("history", JSON.stringify(history));
    }
    if (getCookie('login') != "") {
        $.ajax({
            url: "/Home/AddHistory",
            data: { story_id: story_id, chapter_id: chapter_id },
            success: function () {
            }
        })
    }
}
addStory();
//getCookie("history");
/*function getCookie1(cname) {
    let name = cname + "=";
    let listCookie1 = document.cookie.split(name);
    const result = listCookie1[1].split(";");
    return result[0];
}
if (getCookie1("login") == "true") {
    $.ajax({
        url: "/Home/AddHistory",
        data: { story_id: story_id, chapter_id: chapter_id },
        success: function () {
        }
    })
}*/