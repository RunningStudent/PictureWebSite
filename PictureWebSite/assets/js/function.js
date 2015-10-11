/// <reference path="function.js" />
//根据类名获取元素
function getByClass(parent, tagName, className) {
	var aEls = parent.getElementsByTagName(tagName);
	var arr = [];

	for (var i = 0; i < aEls.length; i++) {
		var aClassName = aEls[i].className.split(' ');
		for (var j = 0; j < aClassName.length; j++) {
			if (aClassName[j] == className) {
				arr.push(aEls[i]);
				break;
			}
		}
	}

	return arr;
}

//增加类名
function addClass(obj, className) {
	//原来没有类名
	if (obj.className == '') {
		obj.className = className;
	} else { //原来有类名
		var arrClassName = obj.className.split(' ');
		var _index = arrIndexOf(arrClassName, className);
		if (_index == -1) { //要添加的类名在原来的class中不存在
			obj.className += ' ' + className;
		} //要添加的类名在原来的class中存在的话什么都不要做了
	}
}

//移除类名
function removeClass(obj, className) {
	//原来有类名
	if (obj.className != '') {
		var arrClassName = obj.className.split(' ');
		var _index = arrIndexOf(arrClassName, className);
		if (_index != -1) { //要删除的类名存在
			arrClassName.splice(_index, 1);
			obj.className = arrClassName.join(' ');
		}
	}
}

//按值搜索，返回序号
function arrIndexOf(arr, v) {
	for (var i = 0; i < arr.length; i++) {
		if (arr[i] == v) {
			return i;
		}
	}
	return -1;
}

//绑定事件
function addEvent(obj, evname, fn) {
	if (obj.addEventListener) {
		obj.addEventListener(evname, fn, false);
	} else {
		obj.attachEvent('on' + evname, function() {
			fn.call(obj);
		});
	}
}

//获取元素的样式
function getStyle(obj, attr) {
	return obj.currentStyle ? obj.currentStyle[attr] : getComputedStyle(obj)[attr];
}

//ajax函数，data为要传给后端的数据，endFn为回调函数
function ajax(method, url, data, endFn) {

	//创建ajax对象
	var xhr = null;
	try {
		xhr = new XMLHttpRequest();
	} catch (e) {
		xhr = new ActiveXObject('Microsoft.XMLHTTP');
	}

	if (method == 'get' && data) {
		url += '?' + data;
	}

	//get,post区别处理
	xhr.open(method, url, true);
	if (method == 'get') {
		xhr.send();
	} else {
		xhr.setRequestHeader('content-type', 'application/x-www-form-urlencoded');
		xhr.send(data);
	}
	xhr.onreadystatechange = function() {

		if (xhr.readyState == 4) {
			if (xhr.status == 200) {
				endFn && endFn(xhr.responseText);
			} else {
				alert('出错了,Err：' + xhr.status);
			}
		}

	}
}

//获取字符串中的数字
function getNum(text) {
	var value = text.replace(/[^0-9]/ig, "");
	return value;
}

//验证一个字符串是否为邮箱
function isEmail(str) {
	var reg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
	return reg.test(str);
}

//验证一个字符串是否包含中文
function isChineseChar(str) {
	var reg = /[\u4E00-\u9FA5\uF900-\uFA2D]/;
	return reg.test(str);
}

//打字机效果
function typeWriterMain(parent, tagName, className) {
	var aTypeWriter = getByClass(parent, tagName, className);
	console.log(aTypeWriter)
	var arrText = [];
	for (var i = 0; i < aTypeWriter.length; i++) {
		arrText[i] = aTypeWriter[i].innerHTML;
		aTypeWriter[i].innerHTML = '';
	}
	var str = '';
	for (var i = 0; i < aTypeWriter.length; i++) {
		if (i < aTypeWriter.length - 1) {
			str += 'typeWriter(aTypeWriter[' + i + '],arrText[' + i + '],function(){';
		} else {
			str += 'typeWriter(aTypeWriter[' + i + '],arrText[' + i + ']);';
		}
	}
	for (var j = 0; j < aTypeWriter.length - 1; j++) {
		str += '});'
	}
	eval(str);
}

function typeWriter(obj, text, endFn) {

	var arr = text.split('');
	var i = 0;

	clearInterval(obj.timer);

	obj.timer = setInterval(function() {
		obj.innerHTML += arr[i++];
		if (i >= arr.length) {
			clearInterval(obj.timer);
			endFn && endFn();
		}
	}, 200);
}

//格式化日期的插件
/**
 * 对Date的扩展，将 Date 转化为指定格式的String
 * 月(M)、日(d)、12小时(h)、24小时(H)、分(m)、秒(s)、周(E)、季度(q) 可以用 1-2 个占位符
 * 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)
 * eg:
 * (new Date()).pattern("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423
 * (new Date()).pattern("yyyy-MM-dd E HH:mm:ss") ==> 2009-03-10 二 20:09:04
 * (new Date()).pattern("yyyy-MM-dd EE hh:mm:ss") ==> 2009-03-10 周二 08:09:04
 * (new Date()).pattern("yyyy-MM-dd EEE hh:mm:ss") ==> 2009-03-10 星期二 08:09:04
 * (new Date()).pattern("yyyy-M-d h:m:s.S") ==> 2006-7-2 8:9:4.18
 */
Date.prototype.pattern = function(fmt) {
	var o = {
		"M+": this.getMonth() + 1, //月份
		"d+": this.getDate(), //日
		"h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12, //小时
		"H+": this.getHours(), //小时
		"m+": this.getMinutes(), //分
		"s+": this.getSeconds(), //秒
		"q+": Math.floor((this.getMonth() + 3) / 3), //季度
		"S": this.getMilliseconds() //毫秒
	};
	var week = {
		"0": "/u65e5",
		"1": "/u4e00",
		"2": "/u4e8c",
		"3": "/u4e09",
		"4": "/u56db",
		"5": "/u4e94",
		"6": "/u516d"
	};
	if (/(y+)/.test(fmt)) {
		fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
	}
	if (/(E+)/.test(fmt)) {
		fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[this.getDay() + ""]);
	}
	for (var k in o) {
		if (new RegExp("(" + k + ")").test(fmt)) {
			fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
		}
	}
	return fmt;
}