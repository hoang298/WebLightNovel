function setCookie(cname, cvalue) {
        let d = new Date();
        d.setTime(d.getTime() + (30 * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + "; " + expires + ";path =/";
    }

    function getCookie(cname) {
        let name = cname + "=";
        let cookie = document.cookie.split(name);
        const r = cookie[1].split(";");
        if (r != "")
            return JSON.parse(r);
        return "";
    }
    function checkCookie(name) {
        let username = getCookie(name);
        if (username == "") {
            setCookie(name, "");
     }
}