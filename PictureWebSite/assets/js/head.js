//改变顶部搜索框的宽度及其下拉ul宽度、位置
function setSearchWidth() {

    var oHead = document.getElementById('head');
    var oTopBar = getByClass(oHead, 'div', 'topBar')[0];
    var oLeftPart = getByClass(oTopBar, 'div', 'leftPart')[0];
    var oRightPart = getByClass(oTopBar, 'div', 'rightPart')[0];
    var oSearch = getByClass(oTopBar, 'div', 'search')[0];
    var oSearchInput = oSearch.getElementsByTagName('input')[0];
    var oSearchHint = getByClass(oTopBar, 'div', 'searchHint')[0];
    var oSearchHintUl = oSearchHint.getElementsByTagName('ul')[0];

    //此处减30因为为input有左右padding（加起来18px），剩余12px为输入框与rightPart之间的距离
    var iWidth = oTopBar.offsetWidth - oLeftPart.offsetWidth - oRightPart.offsetWidth - 30;
    oSearchInput.style.width = iWidth + 'px';

    //搜搜框下面的ul的宽度设置及位置设置
    oSearchHintUl.style.width = iWidth + 20 + 'px'; //此处加20是为了保持搜索框与ul宽度保持一致（20为：左右边框2px+左右padding 12px 6px ）
    oSearchHintUl.style.left = oSearch.offsetLeft + 'px';
}

//登入按钮，用户头像相关事件
function accountEventBind() {

    var obj = document.getElementsByClassName("rightPart")[0];

    var oLoginNav = getByClass(obj, 'ul', 'loginNav')[0];
    //判断登入,注册按钮是否存在
    if (oLoginNav) {
        var aLiLogin = oLoginNav.getElementsByTagName('li');

        //加判断是登录还是注册的自定义属性
        aLiLogin[0].isLogin = false;
        aLiLogin[1].isLogin = true;

        //登录、注册点击事件
        for (var i = 0; i < aLiLogin.length; i++) {
            aLiLogin[i].onclick = function() {
                newMask(this.isLogin, obj);
            }
        }
    } else {
        var oRightPartMenu = getByClass(obj, 'div', 'rightPartMenu')[0];
        obj.onmouseover = function() {
            clearTimeout(obj.timer);
            oRightPartMenu.style.display = 'block';
        }
        obj.onmouseout = function() {
            clearTimeout(obj.timer);
            obj.timer = setTimeout(function() {
                oRightPartMenu.style.display = 'none';
            }, 500);
        }
    }
}



function classify() {
    var oClassify = getByClass(document, 'div', 'classify')[0];
    var oClassifyDiv = getByClass(oClassify, 'div', 'classifyDiv')[0];

    oClassify.onmouseover = function() {
        clearTimeout(oClassify.timer);
        oClassifyDiv.style.display = 'block';
    }
    oClassify.onmouseout = function() {
        clearTimeout(oClassify.timer);
        oClassify.timer = setTimeout(function() {
            oClassifyDiv.style.display = 'none';
        }, 500);
    }
}



window.onload = function() {
    var oWaterfallFlow = document.getElementById('waterfallFlow');
    var oLoader = document.getElementById('loader');
    var aWarpper = getByClass(document, 'div', 'warpper');

    var oHead = document.getElementById('head');
    var oTopBar = getByClass(oHead, 'div', 'topBar')[0];
    var oLeftPart = getByClass(oTopBar, 'div', 'leftPart')[0];
    var oRightPart = getByClass(oTopBar, 'div', 'rightPart')[0];
    var oSearch = getByClass(oTopBar, 'div', 'search')[0];
    var oSearchInput = oSearch.getElementsByTagName('input')[0];
    var oSearchImg = oSearch.getElementsByTagName('a')[0];
    var oSearchHint = getByClass(oTopBar, 'div', 'searchHint')[0];
    var oSearchHintUl = oSearchHint.getElementsByTagName('ul')[0];

    //顶部右边的登录、注册、用户名、头像
    //updateRightPart(false, oRightPart);
    accountEventBind();
    //改变顶部搜索框的宽度
    setSearchWidth();
    window.onresize = function() {
        setSearchWidth();
    }

    //搜索框聚焦、失焦事件
    oSearchInput.onfocus = function() {
        this.style.backgroundColor = 'white';

        document.onkeydown = function(ev) { //按下回车键进行搜索
            var ev = ev || event;
            if (ev.keyCode == 13) {
                alert('搜索功能实现中...');
            }
        }
        document.onkeyup = function() { //键盘弹起出现下拉提示框
            /* 测试用 */
            //if (oSearchInput.value != '') {
            //    var oScript = document.createElement('script');
            //    oScript.src = 'http://suggestion.baidu.com/su?wd=' + oSearchInput.value + '&cb=xgy';
            //    oScript.src = 'http://localhost:9855/handler/SearchPreview.ashx?wd=' + oSearchInput.value + '&cb=xgy';
            //    document.body.appendChild(oScript);
            //} else {
            //    oSearchHintUl.style.display = 'none';
            //}

            //实际用
            if (oSearchInput.value != '') {
                console.log('ss')
                ajax('post', 'http://localhost:9855/handler/SearchPreview.ashx', 'value=' + oSearchInput.value, function (data) {
                    console.log(data);
                });
            } else {
                oSearchHintUl.style.display = 'none';
            }
         
        }
    }
    oSearchInput.onblur = function(ev) {
        this.style.backgroundColor = '#fafafa';

        document.onkeydown = null;
    }
    oSearchImg.onclick = function() {
        alert('搜索功能实现中...');
    }
    classify();
}

//顶部导航栏宽度确定
function headWidth(obj, oHead) {
    var iWidth = obj.offsetWidth;
    console.log(obj, oHead, iWidth);
    oHead.style.width = iWidth + 'px';
}