//瀑布流
function waterfallFlow(parent, oLoader, aWarpper) {
	var iCells = 0; //列数
	var iWidth = 250; //每个数据块的宽
	var iSpace = 10; //数据块间的横向间隔
	var iOuterWidth = iWidth + iSpace;
	var arrT = []; //存储瀑布流每一列距离顶部距离的数组
	var arrL = []; //存储瀑布流每一列距离最左边距离的数组
	var onOff = true; //用来记录瀑布流数据是否加载完了的开关
	var descriptionMargin = 10; //瀑布流数据块里对图片的文字描述的上下margin
	var labelMargin = 10; //瀑布流数据块里标签块的上下margin
	var waterTop = 80; //瀑布流距离页面顶部的距离

	var num = 20; //每次获取20个json
	var ciShu = 0; //用来记录第几次获取

	setCell();

	for (var i = 0; i < iCells; i++) {
		arrT[i] = 0;
		arrL[i] = iOuterWidth * i;
	}

	getList();

	addEvent(window, 'scroll', scrollEventForWater);
	addEvent(window, 'resize', resizeEventForWater);

	//获取数据
	function getList() {
		oLoader.style.display = 'block';
		ajax('post', 'handler/GetPictureListAsy.ashx', 'ciShu=' + ciShu, function(dataForString) {
			var data = JSON.parse(dataForString);
			//循环添加20个数据块
			for (var i = 0; i < 20; i++) {

				if (!data[i]) break;

				var _index = getMin();

				var oDiv = document.createElement('div');
				oDiv.className = 'picDiv';

				var oA = document.createElement('a');
				oA.href = '#';
				var iHeight = data[i].height * (iWidth / data[i].width);
				oA.style.height = iHeight + 'px';
				oA.innerHTML = '<img src="/assets/img/white.png" class="pic lazy" width="' + iWidth + '" height="' + iHeight + '" data-original="' + data[i].imgUrl + '"></a>';
				oDiv.appendChild(oA);

				var oP = document.createElement('p');
				oP.className = 'description';
				oP.innerHTML = data[i].title;
				oDiv.appendChild(oP);

				var oUl = document.createElement('ul');
				oUl.className = 'label clear';
				var oUlStr = '';
				for (var j = 0; j < data[i].label.length; j++) {
					oUlStr += '<li><a href="#">#' + data[i].label[j] + '</a></li>';
				}
				oUl.innerHTML = oUlStr;
				oDiv.appendChild(oUl);

				var userDiv = document.createElement('div');
				userDiv.className = 'users clear';

				var userA = document.createElement('a');
				userA.href = '#';
				userA.className = 'face';
				userA.innerHTML = '<img src="' + data[i].userFace + '" alt="">';
				userDiv.appendChild(userA);

				var userSubDiv = document.createElement('div');
				userSubDiv.className = 'text';
				userSubDiv.innerHTML = '<div class="line"><a href="#">' + data[i].userName + '</a></div><div class="line">' + (eval(data[0].uploadDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-MM-dd") + '</div>';
				userDiv.appendChild(userSubDiv);

				oDiv.appendChild(userDiv);

				oDiv.style.left = arrL[_index] + 'px';
				oDiv.style.top = arrT[_index] + 'px';

				parent.appendChild(oDiv);

				oDiv.style.height = iHeight + oP.offsetHeight + oUl.offsetHeight + userDiv.offsetHeight + 2 * descriptionMargin + 1 * labelMargin + 'px';
				arrT[_index] += parseInt(oDiv.style.height) + 10;
			}

			setTimeout(function() {
				oLoader.style.display = 'none';
			}, 1000);

			//懒加载
			$('img.lazy').lazyload();

			onOff = true;
		});
	}

	function scrollEventForWater() {
		var _index = getMin();
		var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
		var iH = scrollTop + document.documentElement.clientHeight;
		if (arrT[_index] + waterTop < iH) {
			if (onOff) {
				onOff = false; //开始加载数据了，开关关上，保证只加载一次
				ciShu++; //次数加1
				getList();
			}
		}
	}

	function resizeEventForWater() {
		var iLen = iCells;
		setCell(); //重新获取列数
		if (iLen == iCells) { //如果新的列数和以前的列数相同的话直接返回
			return;
		}
		//距左和距顶部距离的数组清空，因为要重新进行排列了
		arrT = [];
		arrL = [];

		for (var i = 0; i < iCells; i++) {
			arrT[i] = 0;
			arrL[i] = iOuterWidth * i;
		}

		//为每个picDiv添加运动特效
		$('.picDiv').each(function() {
			var _index = getMin();
			$(this).animate({
				left: arrL[_index],
				top: arrT[_index]
			}, 1000);
			arrT[_index] += $(this).height() + 10;
		});
	}

	//返回最短li的序号
	function getMin() {
		var v = arrT[0];
		var _index = 0;

		for (var i = 1; i < arrT.length; i++) {
			if (arrT[i] < v) {
				v = arrT[i];
				_index = i;
			}
		}
		return _index;
	}

	//设置列数和warrper的宽度
	function setCell() {
		iCells = Math.floor($(window).innerWidth() / iOuterWidth);
		if (iCells < 3) {
			iCells = 3;
		} else if (iCells > 6) {
			iCells = 6;
		}
		for (var i = 0; i < aWarpper.length; i++) {
			aWarpper[i].style.width = iCells * iOuterWidth - iSpace + 'px';
		}
	}
}