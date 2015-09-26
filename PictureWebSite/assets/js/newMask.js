//创建弹出层
function newMask(isLogin, obj) {
	var oLoginHead, oSpanLogin, oH1LoginHead, oSpanRegister, oALoginHead;
	var oLabel, oForget;
	var oPassword2, oEmail, oVerfiyDiv, oVerfiy, oAVerfiy, oSpanVerfiy, oASpanVerfiy;
	var oButton;
	var oLoginBody, oLoginForm, oUserName, oPassword;
	var oMask, oSignUp;
	var oPForget, oDivForget;
	var oPVerfiy;
	var oPEmail, oPPassword;
	var oPPassword2, oPUserName;

	var canSubmit = true;

	blackBackground(); //黑色半透明背景
	loginHead(); //顶部
	publicArea(); //公共区域
	//根据是登录还是注册产生不同的DOM节点，独特区域
	if (isLogin) {
		newLoginDom();
	} else {
		newRegister();
	}
	newButton(); //按钮

	oLoginBody.appendChild(oLoginForm);
	oSignUp.appendChild(oLoginBody);
	document.body.appendChild(oSignUp);

	oSignUp.style.marginTop = -oSignUp.offsetHeight / 2 + 'px'; //使弹出层保持上下居中

	//点击右上角关闭
	oALoginHead.onclick = function() {
		document.body.removeChild(oMask);
		document.body.removeChild(oSignUp);

		document.onkeydown = null;
	}

	//点击上面的登陆按钮
	oSpanLogin.onclick = function() {
		if (isLogin == true) return;
		isLogin = true;

		this.className = 'active';
		oSpanRegister.className = '';

		oEmail.value = '';
		oPEmail.innerHTML = '';
		oPassword.value = '';
		oPPassword.innerHTML = '';

		oLoginForm.removeChild(oPassword2);
		oLoginForm.removeChild(oPPassword2);
		oLoginForm.removeChild(oUserName);
		oLoginForm.removeChild(oPUserName);
		oLoginForm.removeChild(oVerfiyDiv);
		oLoginForm.removeChild(oPVerfiy);
		oLoginForm.removeChild(oButton);

		//oLoginForm.action = '/account/Login.ashx';

		newLoginDom();
		newButton();

		oSignUp.style.marginTop = -oSignUp.offsetHeight / 2 + 'px';
	}

	//点击上面的注册按钮
	oSpanRegister.onclick = function() {
		if (isLogin == false) return;
		isLogin = false;

		this.className = 'active';
		oSpanLogin.className = '';

		oEmail.value = '';
		oPEmail.innerHTML = '';
		oPassword.value = '';
		oPPassword.innerHTML = '';

		oLoginForm.removeChild(oVerfiyDiv);
		oLoginForm.removeChild(oPVerfiy);
		oLoginForm.removeChild(oDivForget);
		oLoginForm.removeChild(oPForget);
		oLoginForm.removeChild(oButton);

		//oLoginForm.action = '/account/Register.ashx';

		newRegister();
		newButton();

		oSignUp.style.marginTop = -oSignUp.offsetHeight / 2 + 'px';
	}

	//生成登录块下的节点
	function newLoginDom() {
		//验证码
		newVerfiy();

		oDivForget = document.createElement('div');
		oDivForget.className = 'clear';

		//自动登录
		oLabel = document.createElement('label');
		oLabel.for = 'autoLogin';
		oLabel.className = 'autoLogin';
		oLabel.innerHTML = '<input type="checkbox" id="autoLogin" name="autoLogin">自动登录';
		oDivForget.appendChild(oLabel);

		//忘记密码
		oForget = document.createElement('a');
		oForget.href = '#';
		oForget.className = 'forgetPassword';
		oForget.innerHTML = '忘记密码';
		oDivForget.appendChild(oForget);

		oLoginForm.appendChild(oDivForget);
		oPForget = productP(oPForget);
	}

	//生成注册块下的节点
	function newRegister() {
		//再次输入密码
		oPassword2 = document.createElement('input');
		oPassword2.type = 'password';
		oPassword2.name = 'password2';
		oPassword2.className = 'password';
		oPassword2.placeholder = '请再次输入密码';
		oLoginForm.appendChild(oPassword2);
		oPPassword2 = productP(oPPassword2);

		//用户用户名
		oUserName = document.createElement('input');
		oUserName.type = 'text';
		oUserName.name = 'username';
		oUserName.className = 'username';
		oUserName.placeholder = '请输入用户用户名';
		oUserName.autocomplete = 'off';
		oLoginForm.appendChild(oUserName);
		oPUserName = productP(oPUserName);

		//验证码
		newVerfiy();

		//输入框聚焦失焦事件
		focusBlurEvent(oPassword2, oPPassword2);
		focusBlurEvent(oUserName, oPUserName);

	}

	//输入框添加聚焦、失焦事件
	function focusBlurEvent(obj, obj1) {
		obj.onfocus = function() {

			//进入之前先清空
			obj1.innerHTML = '';
			obj1.style.color = 'black';

			if (!isLogin) { //注册
				var str = '';

				switch (obj.className) {
					case 'verify':
						{
							str = '请输入右侧图片内容';
							break;
						}
					case 'email':
						{
							str = '请输入你有效的邮箱';
							break;
						}
					case 'password':
						{
							str = '请输入6-20位密码，区分大小写，不能使用空格';
							break;
						}
					case 'username':
						{
							str = '请输入用户名，2-20位中英文、数字或下划线';
							break;
						}
				}
				obj1.innerHTML = str; //验证码：请输入右侧图片内容
			} else { //登录
				obj1.innerHTML = '';
			}
		}

		obj.onblur = function() {
			var str = '';
			switch (obj.className) {
				case 'password': //注册区的密码输入框的特殊处理
					{
						var arr = obj.value.split('<script>');
						if (arr.length != 1) { //长度不等于1，说明里面含有<script>标签
							obj1.innerHTML = '本输入框不支持嵌入脚本！'
							obj1.style.color = 'red';
							canSubmit = false;
							return;
						} else {
							canSubmit = true;
						}
                        //先看长度，长度合适了在检验两次密码是否一致
						if (obj.value && (obj.value.length > 20 || obj.value.length < 6)) {//长度不合适的情况
							obj1.innerHTML = '密码长度不合适';
							obj1.style.color = 'red';
						} else {//为空或长度合适的情况
						    if (!isLogin) {//注册的情况
						        if (oPassword.value && oPassword2.value && (oPassword.value != oPassword2.value)) {//长度合适
						            oPPassword.innerHTML = '两次输入密码不一致';
						            oPPassword2.innerHTML = '两次输入密码不一致';
						            oPPassword.style.color = 'red';
						            oPPassword2.style.color = 'red';
						        } else {//为空，登陆的情况
						            oPPassword.innerHTML = '';
						            oPPassword2.innerHTML = '';
						            oPPassword.style.color = 'black';
						            oPPassword2.style.color = 'black';
						        }
						    }
                            console.log(isLogin)

						}
						break;
					}
				case 'username':
					{
						var len = 0;
						var arr = obj.value.split('');
						for (var i = 0; i < arr.length; i++) {
							isChineseChar(arr[i]) ? len += 2 : len++; //汉字的话长度加2，字符的话长度加1
						}
						if ((len > 20 || len < 2) && obj.value) {
							obj1.innerHTML = '用户名长度不合适';
							obj1.style.color = 'red';
						} else {
							obj1.innerHTML = str;
							obj1.style.color = 'black';
						}
						break;
					}
				case 'email':
					{
						if (!isEmail(obj.value) && obj.value) { //如果不是邮箱
							obj1.innerHTML = '邮箱格式不正确';
							obj1.style.color = 'red';
						} else {
							obj1.innerHTML = str;
							obj1.style.color = 'black';
						}
						break;
					}
				default:
					{
						obj1.innerHTML = str;
						obj1.style.color = 'black';
					}
			}

		}
	}

	function functionCanSubmit() {
		var aP = getByClass(document, 'p', 'loginP');
		for (var i = 0; i < aP.length; i++) {
			if (aP[i].innerHTML != '') { //有不为空的
				return false;
			}
		}
		return true;
	}

	//点击下面的登录/注册按钮
	function newButton() {
		var data = null;
		oButton = document.createElement('input'); //提交表单按钮
		oButton.type = 'button';
		oButton.value = isLogin == true ? '登录' : '注册';
		oButton.name = 'loginButton';
		oButton.className = 'loginButton';
		oLoginForm.appendChild(oButton);

		//按钮的点击事件，根据登录或是注册不同处理
		if (isLogin == true) { //登录
			oButton.onclick = function() { //点击按钮提交登录信息的情况
				if (canSubmit && functionCanSubmit()) submitLoginDate();
			}
			document.onkeydown = function(ev) { //点击回车键提交的情况
				var ev = ev || event;
				if (ev.keyCode == 13) { //按下回车键
					if (canSubmit && functionCanSubmit()) submitLoginDate();
				}
			}
		} else { //注册
			oButton.onclick = function() { //点击按钮提交注册信息的情况
				if (canSubmit && functionCanSubmit()) submitRegisterDate();
			}
			document.onkeydown = function(ev) { //点击回车键提交的情况
				var ev = ev || event;
				if (ev.keyCode == 13) { //按下回车键
					if (canSubmit && functionCanSubmit()) submitRegisterDate();
				}
			}
		}
	}

	//提交登录信息，接收返回数据并进行登录后相关操作
	function submitLoginDate() {
		data = 'email=' + oEmail.value + '&password=' + oPassword.value + '&verify=' + oVerfiy.value;
		ajax('post', '/account/Login.ashx', data, function(dataForString) {
			console.log(dataForString);
			var data = JSON.parse(dataForString);
			if (data.isLogined == true) { //登录成功
				location.reload();
			} else { //登录失败

				//登录失败在相应区域填写错误信息
				switch (data.place) {
					case 0:
						{
							oPVerfiy.innerHTML = data.errorMessage;
							oPVerfiy.style.color = 'red';
							break;
						}
					case 1:
						{
							oPEmail.innerHTML = data.errorMessage;
							oPEmail.style.color = 'red';
							break;
						}
					case 2:
						{
							oPPassword.innerHTML = data.errorMessage;
							oPPassword.style.color = 'red';
							break;
						}
					case 3:
						{
							oPVerfiy.innerHTML = data.errorMessage;
							oPVerfiy.style.color = 'red';
							break;
						}
				}

				oAVerfiy.innerHTML = '<img src="handler/ValidateCodeHandler.ashx?' + new Date().getTime() + '" />'; //更新验证码图片
			}

		});
	}

	//提交注册信息，接收返回数据并进行注册后登录操作（注册成功即登录成功）
	function submitRegisterDate() {
		data = 'email=' + oEmail.value + '&password=' + oPassword.value + '&username=' + oUserName.value + '&verify=' + oVerfiy.value;
		ajax('post', '/account/Register.ashx', data, function(dataForString) {
			console.log(dataForString);
			var data = JSON.parse(dataForString);
			if (data.isRegister) { //注册成功
				location.reload();
			} else { //注册失败

				//注册失败在相应区域填写错误信息
				switch (data.place) {
					case 0:
						{
							oPVerfiy.innerHTML = data.errorMessage;
							oPVerfiy.style.color = 'red';
							break;
						}
					case 1:
						{
							oPEmail.innerHTML = data.errorMessage;
							oPEmail.style.color = 'red';
							break;
						}
					case 2:
						{
							oPPassword.innerHTML = data.errorMessage;
							oPPassword.style.color = 'red';
							break;
						}
					case 3:
						{
							oPUserName.innerHTML = data.errorMessage;
							oPUserName.style.color = 'red';
							break;
						}
					case 4:
						{
							oPVerfiy.innerHTML = data.errorMessage;
							oPVerfiy.style.color = 'red';
							break;
						}
				}

				oAVerfiy.innerHTML = '<img src="handler/ValidateCodeHandler.ashx?' + new Date().getTime() + '" />'; //更新验证码图片
			}
		});
	}

	//验证码区域
	function newVerfiy() {
		oVerfiyDiv = document.createElement('div'); //验证码div
		oVerfiyDiv.className = 'verifyDiv clear';
		oVerfiy = document.createElement('input'); //验证码输入框
		oVerfiy.type = 'text';
		oVerfiy.name = 'verify';
		oVerfiy.className = 'verify';
		oVerfiy.placeholder = '请输入验证码';
		oVerfiyDiv.appendChild(oVerfiy);
		oAVerfiy = document.createElement('a'); //验证码图片
		oAVerfiy.href = 'javascript:;';
		oAVerfiy.className = 'verifyImg';
		oAVerfiy.innerHTML = '<img src="handler/ValidateCodeHandler.ashx" />'; //验证码图片路径
		oVerfiyDiv.appendChild(oAVerfiy);
		oSpanVerfiy = document.createElement('span'); //看不清，换一张
		oSpanVerfiy.innerHTML = '看不清<br />';
		oASpanVerfiy = document.createElement('a');
		oASpanVerfiy.href = 'javascript:;';
		oASpanVerfiy.innerHTML = '换一张';
		oSpanVerfiy.appendChild(oASpanVerfiy);
		oVerfiyDiv.appendChild(oSpanVerfiy);
		oLoginForm.appendChild(oVerfiyDiv);
		oPVerfiy = productP(oPVerfiy);

		//更换验证码图片
		oAVerfiy.onclick = oASpanVerfiy.onclick = function() {
			oAVerfiy.innerHTML = '<img src="handler/ValidateCodeHandler.ashx?' + new Date().getTime() + '" />';
		}

		//聚焦，失焦
		focusBlurEvent(oVerfiy, oPVerfiy);

	}

	//弹出层顶部区域
	function loginHead() {
		oLoginHead = document.createElement('div');
		oLoginHead.className = 'loginHead';
		oH1LoginHead = document.createElement('h1');
		oSpanLogin = document.createElement('span');
		if (isLogin) oSpanLogin.className = 'active';
		oSpanLogin.innerHTML = '登录';
		oSpanRegister = document.createElement('span');
		if (!isLogin) oSpanRegister.className = 'active';
		oSpanRegister.innerHTML = '注册';
		oH1LoginHead.appendChild(oSpanLogin);
		oH1LoginHead.appendChild(oSpanRegister);
		oLoginHead.appendChild(oH1LoginHead);
		oALoginHead = document.createElement('a');
		oALoginHead.href = 'javascript:;';
		oLoginHead.appendChild(oALoginHead);
		oSignUp.appendChild(oLoginHead);
	}

	//登录、注册的公共部分
	function publicArea() {
		oLoginBody = document.createElement('div');
		oLoginBody.className = 'loginBody';

		oLoginForm = document.createElement('form');
		oLoginForm.className = 'loginForm';
		oLoginForm.name = 'loginform';
		oLoginForm.method = 'post'; //method
		oLoginForm.action = isLogin ? '/account/Login.ashx' : '/account/Register.ashx'; //根据登陆还是注册改变

		//邮箱
		oEmail = document.createElement('input');
		oEmail.type = 'text';
		oEmail.name = 'email';
		oEmail.className = 'email';
		oEmail.placeholder = '请输入邮箱';
		oEmail.autocomplete = 'off';
		oLoginForm.appendChild(oEmail);
		oPEmail = productP(oPEmail);

		//密码
		oPassword = document.createElement('input');
		oPassword.type = 'password';
		oPassword.name = 'password';
		oPassword.className = 'password';
		oPassword.placeholder = '请输入密码';
		oLoginForm.appendChild(oPassword);
		oPPassword = productP(oPPassword);


		focusBlurEvent(oEmail, oPEmail);
		focusBlurEvent(oPassword, oPPassword);
	}


	//产生input下文字段落
	function productP(obj) {
		obj = document.createElement('p');
		obj.className = 'loginP';
		oLoginForm.appendChild(obj);
		return obj;
	}

	//黑色半透明背景
	function blackBackground() {
		oMask = document.createElement('div');
		oMask.id = 'mask';
		oMask.style.height = $(window).innerHeight() + 'px';
		document.body.appendChild(oMask);

		oSignUp = document.createElement('div');
		oSignUp.id = 'signin';
	}

}