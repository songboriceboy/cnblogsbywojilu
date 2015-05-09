jQuery.ajaxSettings.traditional = true;

var wojilu = new Object();

wojilu.path = {};
wojilu.path.app = '';
wojilu.path.st = wojilu.path.app + '/static';
wojilu.path.js = wojilu.path.st + '/js';
wojilu.path.img = wojilu.path.st + '/img';
wojilu.path.siteAndApp = 'http://' + window.location.host + wojilu.path.app;
wojilu.path.flash = wojilu.path.st + '/flash';'';

wojilu.ctx = {
    isSubmit : true,
    isValid : true,

    box : {
    	sizeCache : null,
    	isShowing : false,
    	param : null,
    	title : '对话窗口',
    	pages : [],
    	isBack : false
    }
};

var logger = {
	info : function(msg) {
		if( !document.all ) console.log( msg );		
	}
};

$.fn.check = function(mode) {
	var mode = mode || 'on';
	return this.each(function() {
		switch(mode) {
		case 'on':
			this.checked = true;
			break;
		case 'off':
			this.checked = false;
			break;
		case 'toggle':
			this.checked = !this.checked;
			break;
		}
	});
};

wojilu.str = {

    isNull : function( txt ) {
        return txt=='undefined' || txt==null || txt.length<1;
    },

    hasText : function( txt ) {
        return !wojilu.str.isNull( txt );
    },

    startsWith : function( txt, value ) {
        return ( txt.substr( 0, value.length ) == value );
    },

    endsWith : function( txt, value ) {
        return ( txt.substr( txt.length-value.length, txt.length ) == value ) ;
    },

    isJson : function( txt ) {
        if( wojilu.str.isNull( txt ) ) {return false;};
        var rtxt = $.trim(txt);
        if( wojilu.str.startsWith( rtxt, '{' ) && wojilu.str.endsWith( rtxt, '}' ) ) {return true;};
        return false;
    },
    
    isInt : function( text ) {
        var i = parseInt(text);
        if (isNaN(i)) {return false;};
        if (i.toString() == text) {return true;};
        return false;
    },

    replaceAll : function( strText, strTarget,  strSubString ){
        var intIndexOfMatch = strText.indexOf( strTarget );
        while (intIndexOfMatch != -1) {
            strText = strText.replace( strTarget, strSubString );
            intIndexOfMatch = strText.indexOf( strTarget );
        };
        return strText;
    },

    trimStart : function( txt, val ) {
        if( wojilu.str.startsWith( txt, val ) ==false ) {return txt;};
        return txt.substring( val.length, txt.length );
    },

    trimEnd : function( txt, val ) {
        if( wojilu.str.isNull( txt ) ) return txt;
        if( !wojilu.str.endsWith( txt, val ) ) return txt;
        if( txt == val ) return '';
        return txt.substr( 0, txt.length-val.length );
    },

    trimExt : function( txt ){
        var extPosition = txt.search( /\.[^\.]*$/i );
        if( extPosition>=0 ) {return txt.substring(0, extPosition );};
        return txt;
    },

    trimHost : function( txt ) {
        if( wojilu.str.startsWith( txt, 'http://' ) ==false ) {return txt;};
        var result = wojilu.str.trimStart( txt, 'http://' );
        var slashIndex = result.indexOf( '/' );
        return result.substring( slashIndex, result.length );
    },

    getExt : function( txt ){
        return txt.replace( wojilu.str.trimExt(txt), '' );
    }

};

wojilu.tool = {
    
    getPageHead : function() {
        return document.getElementsByTagName('HEAD').item(0);
    },

    refreshImg : function( vimg ) {
        vimg.attr( 'src', vimg.attr('src').toAjax() );
    },
    
    resizeBox : function(x,y) {
    	var width = x;
    	var height = y;
    	wojilu.ui.box.setWidthHeight( width, height );
    	$('#boxFrm').css( 'width', width+'px');
    	$('#boxFrm').css( 'height', height+'px');
    	$('#box').css( 'width', width+'px');
    	$('#box').css( 'height', (height+26)+'px');
    },
    
    addBoxHeight : function(y) {
        var t = $('#boxFrm');
        t.height( t.height()+y );
        var b = $('#box');
        b.height( b.height()+y );
   },
    
    resizeParentBox : function() {
        var width = $(document).width();
        var height = parseInt( $(document).height() );
        window.parent.wojilu.tool.resizeBox(width,height);
    },
    
    resizeImg : function( target, width, height ) {
        if( width && target.width>width){target.width=width;};
        if( height && target.height>height){target.height=height;};
    },
    
    htmlReplace : function( targetId, val ) {
        $('#'+targetId).html( val );
    },
    
    htmlAppend : function( targetId, val ) {
        $('#'+targetId).append( val );
    },
    
    hideLoading : function() {
        $('#loadingDiv').hide();
    },
    
    reloadPage : function() {
        window.location = window.location;
    },

    forward : function( url, time ) {
        if( !time ) {time = 500;};
        setTimeout( function(){
            if( wojilu.str.startsWith( url, '/' ) ) url = 'http://'+window.location.host+url;
            var currentUrlPath = window.location.href;
            var currentHash = currentUrlPath.indexOf( '#' );
            if( currentHash>0 ) currentUrlPath = currentUrlPath.substring( 0, currentHash );
            
            if( wojilu.str.startsWith( url, currentUrlPath+'#'  ) ) {
                var hashIndex = url.indexOf( '#' );
                var urlPath = url.substring( 0, hashIndex );
                var urlFragment = url.substring( hashIndex, url.length );
                window.location.href=urlPath+'?reload=true'+urlFragment;
            }
            else {
                window.location.href=url;
            }
        }, time );
    },
    
    loadJs : function( filePath ) {
        loadJsFull( wojilu.path.js + '/' + filePath );
    },

    loadJsFull : function( filePath ) {
        var script = document.createElement('script');
        script.src = filePath;
        script.type = 'text/javascript';
        wojilu.tool.getPageHead().appendChild(script);
    },
    
    loadCss : function( filePath ) {
        var style = document.createElement('link');
        style.href = filePath;
        style.rel = 'stylesheet';
        style.type = 'text/css';
        wojilu.tool.getPageHead().appendChild(style);
    },
    
    getByteLength : function ( str ) {
        if( wojilu.str.isNull( str ) ) return 0;
        var pattern = /[\u4e00-\u9fa5]/g;
        var arrChineseChar = str.match(pattern);
        if( arrChineseChar ) return str.length + arrChineseChar.length;
        return str.length;
    },

    getTimePrivate : function(seperator, isMilliseconds){
       var result = '';
       var d = new Date();
       result += d.getHours() + seperator;
       result += d.getMinutes() + seperator;
       result += d.getSeconds();
       if( isMilliseconds ) {result += seperator + d.getMilliseconds();};
       return result;
    },

    getDayPrivate : function(seperator){
       var d = new Date();
       var result = '';
       result += d.getFullYear() + seperator;
       result += (d.getMonth()+1) + seperator;
       result += d.getDate();
       return result; 
    },
    
    getRandom : function() {
        return Math.random()+ wojilu.tool.getDayPrivate('')+ wojilu.tool.getTimePrivate('', true);
    },

    getRandomInt : function() {
        return wojilu.tool.getDayPrivate('')+ wojilu.tool.getTimePrivate('', true);
    },

    getTime : function() {
        return wojilu.tool.getTimePrivate(':', false);
    },

    getDay : function() {
        return wojilu.tool.getDayPrivate('-');
    },

    getFileName : function(fileFullName) {
        var index = fileFullName.lastIndexOf( '\\' );
        if( index==-1 ) {index = fileFullName.lastIndexOf( '/' );};
        return fileFullName.substring(index+1);
    },

    getParams : function( scriptName ) {
        var sc=document.getElementsByTagName('script');
        
        var scriptSrcQuery = '';
        for( var i=0;i< sc.length;i++ ) {
            if( sc[i].src.indexOf( scriptName )>=0 ) {
                scriptSrcQuery = sc[i].src.split( '?' )[1];
                continue;
            };
        };
        
        var arrRawItem = scriptSrcQuery.split( '&' );
        var results = new Array();
        for( i=0;i<arrRawItem.length;i++) {
            var itemString = arrRawItem[i].split( '=' );
            results[ itemString[0] ] = itemString[1];
        };
        return results;
    },

    getQuery : function( qName ) {
        var url = window.location.href;
        var arrPart = url.split( '?');
        if( arrPart.length <=1 ) {return '';};
        var arrItem = arrPart[1].split( '&' );
        for( var i=0;i<arrItem.length;i++ ) {
            if( wojilu.str.startsWith( arrItem[i], qName+'=' ) ) {				
                return wojilu.str.trimStart( arrItem[i], qName+'=' );
            };
        };
        return '';
    },
    
    cancelBubble : function(e) {
        if ( e && e.stopPropagation ) {
            e.stopPropagation();
        }
        else {
            window.event.cancelBubble = true;
        };
        return false;
    }    
};

String.prototype.toAjax = function() {
	var indexQuery = this.indexOf( '?' );
	if( indexQuery<0 ) {
		return this + '?rd=' + wojilu.tool.getRandom() + '&ajax=true';
	}
	else {
		var queryString = this.substring( indexQuery+1, this.length );
		var url = this.substring( 0, indexQuery );
		var newQueryString = '';
		var arrQueryItem = queryString.split( '&' );
		for( i=0;i<arrQueryItem.length;i++ ) {
			var item = arrQueryItem[i];
			if( wojilu.str.startsWith( item, 'rd=' ) || wojilu.str.startsWith( item, 'ajax=' ) ) {continue;};
			newQueryString += item;
			if( i<arrQueryItem.length-1 ) {newQueryString += '&';};
		};
		return url + '?rd=' + wojilu.tool.getRandom() + '&ajax=true&' + newQueryString;
	}
};

String.prototype.cssVal = function() {
    return parseInt( wojilu.str.trimEnd( this, 'px' ) );
};

String.prototype.toAjaxFrame = function() {
    var url = this;
    if( url.indexOf( 'frm=true' )>0 ) return url;
	var indexQuery = url.indexOf( '?' );
    var rd = '&rd='+wojilu.tool.getRandom();
	if( indexQuery<0 ) {
		return url + '?frm=true'+rd;
	}
	else {
        if( wojilu.str.endsWith( url, '?' ) ) return url + 'frm=true'+rd;
        if( wojilu.str.endsWith( url, '&' ) )  return url + 'frm=true'+rd;
        return url + '&frm=true'+rd;
    };
};

wojilu.position = {

    getMouse : function(ev){
    	ev = ev || window.event;
    	if(ev.pageX || ev.pageY) return {x:ev.pageX, y:ev.pageY};
    	return {		
    		x:ev.clientX + document.documentElement.scrollLeft - document.body.clientLeft,
    		y:ev.clientY + document.documentElement.scrollTop - document.body.clientTop
    	};
    },

    getTarget : function(target){
    	var left = 0;
    	var top  = 0;

    	while (target.offsetParent){
    		left += target.offsetLeft;
    		top += target.offsetTop;
    		target = target.offsetParent;
    	};

    	left += target.offsetLeft;
    	top += target.offsetTop;

    	return {x:left, y:top};
    },

    getMouseOffset : function(target, ev){
    	ev = ev || window.event;
    	var targetPosition = wojilu.position.getTarget(target);
    	var mousePosition  = wojilu.position.getMouse(ev);
    	return {x:mousePosition.x - targetPosition.x, y:mousePosition.y - targetPosition.y};
    }
};

wojilu.ui = new Object();

var shouldHide = function( e, menuList, menuMore, isMore ) {    
    var pi = wojilu.position.getMouse(e);
    var piMenu = wojilu.position.getTarget( menuList[0] );
    var piMore = wojilu.position.getTarget( menuMore[0] );
    
    var minX = piMenu.x;
    var minY = piMore.y;        
    var maxX = piMenu.y+menuList.height();
    if( isMore ) maxX = piMore.x + menuMore.width();        
    var maxY = piMenu.y+menuList.height();
    
    if( pi.x< minX ) return true;
    if( pi.y< minY ) return true;
    if( pi.x> maxX ) return true;
    if( pi.y> maxY ) return true;
    return false;
};

wojilu.ui.clickMenu = function() {
    $( '.clickMenu' ).click( function() {
 		var tp = wojilu.position.getTarget(this);
        var item = $( '#'+$(this).attr('list') ).appendTo( 'body' );
        item.css( 'position', 'absolute' ).css( 'zIndex', 999 ).css( 'left' , tp.x ).css( 'top', tp.y + this.offsetHeight );
        item.show().mouseout(function(){ $(this).hide(); }).mouseover( function() {$(this).show();});
    }).mouseout( function(e) {
        var menuList = $( '#'+$(this).attr('list') );
        if( shouldHide( e, menuList, $(this), true ) ) {
            menuList.hide();
        }
    });  
};

wojilu.ui.menu = function() {
    $( '.menuMore' ).mouseover( function() {
 		var tp = wojilu.position.getTarget(this);
        var item = $( '#'+$(this).attr('list') ).appendTo( 'body' );
        item.css( 'position', 'absolute' ).css( 'zIndex', 999 ).css( 'left' , tp.x ).css( 'top', tp.y + this.offsetHeight );
        item.show().mouseout(function(){ $(this).hide(); }).mouseover( function() {$(this).show();});
    }).mouseout( function(e) {
        var menuList = $( '#'+$(this).attr('list') );
        if( shouldHide( e, menuList, $(this), true ) ) {
            menuList.hide();
        }
    });    
};

wojilu.ui.tab = function() {
	var currentUrl = wojilu.str.trimHost( window.location.href );
    currentUrl = wojilu.str.trimExt( currentUrl );
	$('.otherTab a' ).each( function(i) {
        var link = $(this).attr( 'href' );
        link = wojilu.str.trimExt( link );
        if( currentUrl.indexOf( link )>=0 ) {
            $(this).parent().removeClass( 'otherTab' ).addClass( 'currentTab' );
        };
	});
};

wojilu.ui.pageReturn = function() {
	$( '.btnReturn' ).click( function() {history.back();} );
};

wojilu.ui.boxCancel = function() {
    $( '.btnCancel' ).click( function() {window.parent.wojilu.ui.box.hideBox();} );
};

wojilu.ui.tip = function() {
	var tipInputs = $( '.tipInput' );
	if( tipInputs.length>0 ) {
		function chkInputTip() {
			if( $(this).val() !='' && $(this).val() != $(this).attr('tip') ) return;
			$(this).val( $(this).attr('tip') );
			$(this).addClass('inputTip');
		};
		tipInputs.each( chkInputTip );
		tipInputs.blur( chkInputTip);
		tipInputs.click( function() {
			if( $(this).val() == $(this).attr('tip') ) $(this).val('');
			$(this).removeClass('inputTip');
		});
	};
};

wojilu.ui.tree = function() {
    $('.parentNode').click( function() {
        $(this).toggleClass( 'expandNode' );
        $(this).toggleClass( 'collapseNode' );
        $(this).next().slideToggle(100);
    });
};

wojilu.ui.postBack = function(control, httpMethod) {
    var href = $(control).attr( 'href' );
    var formId = 'hiddenForm' + wojilu.tool.getRandomInt();
    var postForm = '<form method="POST" action="'+href +'" id="'+formId+'" style="display:none;">'+
        '<input name="Submit1" type="submit" value="__hiddenForm">'+
        '<input name="_httpmethod" type="hidden" value="'+httpMethod+'" /></form>';
    
    $(control).append( postForm );		
    $( '#' +formId ).submit();    
};

wojilu.ui.httpMethod = function() {
	$('.postCmd').unbind('click').click( function() { wojilu.ui.postBack(this, 'POST');return false;});
	$('.deleteCmd').unbind('click').click( function() { if( confirm( lang.deleteTip ) ) wojilu.ui.postBack(this, 'DELETE'); return false; });    
    $('.putCmd').unbind('click').click( function() { wojilu.ui.postBack(this, 'PUT');return false;});
};

wojilu.ui.valid = function() {

	var arrRule = new Array();
	arrRule['name'] = /^[a-zA-Z]{1}([a-zA-Z0-9]|[_]){2,19}$/;          //英文开头，可数字、下划线，3-20个字符
	arrRule['name_cn'] = /^[a-zA-Z\u4E00-\u9FA5]{1}([0-9a-zA-Z \u4E00-\u9FA5]|[_]){1,19}$/;   //可中英文，长度2-19
	arrRule['password'] = /^.{4,20}$/;
	arrRule['int'] = /^[0-9]{1,10}$/;
	arrRule['email'] = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	arrRule['tel'] = /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/;
	arrRule['mobile'] = /^((\(\d{2,3}\))|(\d{3}\-))?13\d{9}$/;
	arrRule['url'] = /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/;
	arrRule['money'] = /^\d+(\.\d+)?$/;
	arrRule['zip'] = /^[1-9]\d{5}$/;
	arrRule['qq'] = /^[1-9]\d{4,8}$/;
    
	validPage();

	function validPage() {
		var validator = $( '.valid' );
		if( validator.length>0 ) {			
			$( '.valid' ).each( addValid );
			var form = $( '.valid' ).parents( 'form' );
			form.submit( function() {
				wojilu.ctx.isValid = true;
				$( '.valid' ).each( validOne );
				return wojilu.ctx.isValid;
			});
		};
	};

	function addValid() {
		var validSpan = $(this);
		var target = getTarget(validSpan);
		var isShow = validSpan.attr( 'show' );
		if( 'true'==isShow ) validSpan.html( validSpan.attr('msg') );
        if( target.attr( 'type' ) == 'hidden' ) {
            editorBlur( target, validSpan );
        }
        else if( target.attr( 'type' ) == 'checkbox' ) {
            var chks = $('input:checkbox[name="'+target.attr('name')+'"]');
            chks.click( function() {validInput(target, validSpan);});
        }
        else if( target.attr( 'type' ) == 'radio' ) {
            var rdos = $('input:radio[name="'+target.attr('name')+'"]');
            rdos.click( function() {validInput(target, validSpan);});
        }
        else {
            target.blur( function() {validInput(target, validSpan);});
        };
	};
    
    function editorBlur(target,validSpan) {
        var editor = geteditor(target);
        if( editor == null ) {
            target.blur( function() {validInput(target, validSpan);});
            return;
        };
        
        if (document.all) {
            editor.attachEvent('onblur', function() { validInput(target, validSpan); });
        }
        else {
             editor.addEventListener('blur', function() { validInput(target, validSpan); }, false);
        };
    };
    
    function geteditor(target) {
        var frm = target.next().children('.wojiluEditorFrame');
        if( frm.size()==0 ) return null;
        if( document.all ) { return frm[0]; } else { return frm[0].contentWindow; };
    };

	function validOne() {
		var validSpan = $(this);
		var target = getTarget(validSpan);
		validInput(target, validSpan);
	};

	function setMsg( result, validSpan, msg ) {
		var target = getTarget(validSpan);
		var mode = validSpan.attr( 'mode' );
		if( result==-1 ) {
			if( 'border' == mode ) 
				setErrorMsgSimple( validSpan, msg );
			else
				setErrorMsg( validSpan, msg );
		}
		else {
			if( 'border' == mode ) 
				setOkMsgSimple(validSpan);
			else
				setOkMsg( validSpan );
        };
	};

	function setErrorMsg( validSpan, msg ) {
        if( !msg ) msg = lang.exFill;
		validSpan.html( '<span class="validError">'+msg+'</span>' );
		validSpan.css( 'border', '1px #fed22f solid' );
		validSpan.css( 'background', '#ffe45c' );
		validSpan.css( 'color', '#666' );
		wojilu.ctx.isValid = false;
	};
	
	function setErrorMsgSimple( validSpan, msg ){
		var target = getTarget(validSpan);
        if( target.attr( 'type' )=='hidden' ) {
            target.next().addClass( 'inputWarning' );
        }
        else if( target.attr( 'type' )=='checkbox' ) {
            target.parent().addClass( 'inputWarning' );
        }
        else if( target.attr( 'type' )=='radio' ) {
            target.parent().addClass( 'inputWarning' );
        }
        else {
            target.addClass( 'inputWarning' );
        };
		wojilu.ctx.isValid = false;
	};

	function setOkMsg( validSpan ) {
		validSpan.html( '<span class="validOk">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>' );
		validSpan.css( 'border', '0px #ffd324 dotted' );
		validSpan.css( 'background', '#fff' );
	};
	
	function setOkMsgSimple(validSpan) {
		var target = getTarget(validSpan);		
        if( target.attr( 'type' )=='hidden' ) {
            target.next().removeClass( 'inputWarning' );
        }
        else if( target.attr( 'type' )=='checkbox' ) {
            target.parent().removeClass( 'inputWarning' );
        }
        else if( target.attr( 'type' )=='radio' ) {
            target.parent().removeClass( 'inputWarning' );
        }
        else {
            target.removeClass( 'inputWarning' );
        };
	};
    
    function isValNull( target ) {

        var inputValue = target.val();
        if( target.attr( 'type' )=='checkbox' ) {
            inputValue = $('input:checkbox[name="'+target.attr('name')+'"]:checked').val();
        }
        else if( target.attr( 'type' )=='radio' ) {
            inputValue = $('input:radio[name="'+target.attr('name')+'"]:checked').val();
        }
        
        if( inputValue=='undefined' || inputValue==null || inputValue.length<1 ) return true;
        var stip = target.attr( 'tip' );        
        if( stip && inputValue == stip  ) return true;
        return false;
    };

	function validInput(target, validSpan) {
        var inputValue = target.val();
		var rule = validSpan.attr( 'rule' );
		var msg = validSpan.attr( 'msg' );
		var mode = validSpan.attr( 'mode' );
        var ajaxAction = validSpan.attr( 'ajaxAction' );        

		if( isValNull(target) ) {
			if( 'border' == mode )
				setErrorMsgSimple(validSpan, msg);
			else
				setErrorMsg( validSpan, msg );
			return;
		};

		if( rule==undefined || rule==null || rule.length==0 ) {
        
            if( wojilu.str.hasText( ajaxAction ) ) {
                checkAjaxResult(target, inputValue, validSpan, ajaxAction);
                return;
            }
            else {
    			if( 'border' == mode )
    				setOkMsgSimple(validSpan);
    			else
    				setOkMsg( validSpan );
    			return;
            };
		};

		for( ruleKey in arrRule ) {
			if( rule==ruleKey ) {
                checkResult( target, inputValue, arrRule[ruleKey], validSpan, msg, ajaxAction );
				return;
			}
		};
        
        if( rule=='same' ) {
            var type = validSpan.attr( 'type' );
            var targetStr = validSpan.attr( 'target' );
            var target = $( getTargetSelector( targetStr, type ) );
            var isSame = ( inputValue==target.val() ? 0:-1);
            setMsg( isSame, validSpan, msg );
            return;
        };

		if( rule=='password2' ) {
			var result = inputValue.search( arrRule['password'] );
			var form = validSpan.parents( 'form' );
			var isSame = ( inputValue==$( ':password', form[0] ).not(getTarget(validSpan)).val() );
			var pwdResult = (result==-1 || isSame==false)?-1:0;
			setMsg( pwdResult, validSpan, msg );
			return;
		};

        checkResult( target, inputValue, rule, validSpan, msg, ajaxAction );        
            
		return;
	};
    
    function checkResult( target, inputValue, rule, validSpan, msg, ajaxAction ) {
		var result = inputValue.search( rule );
        if( result == -1 || wojilu.str.isNull(ajaxAction) )
            setMsg( result, validSpan, msg );
        else {
            checkAjaxResult(target, inputValue, validSpan, ajaxAction);
        };
    };
    
    function checkAjaxResult(target, inputValue, validSpan, ajaxAction) {
        var cname = target.attr( 'name' );
        var pdata = new Object();
        pdata[ cname ] = inputValue;
        $.post( ajaxAction.toAjax(), pdata, function(data) {
            var aResult = eval( '('+data+')' );
            var aMsg = aResult.Msg;
            result = aResult.IsValid?1:-1;
            setMsg( result, validSpan, aMsg );
        });
    };

	function getTarget(validSpan) {
		var target = validSpan.attr( 'to' );
		if( target=='undefined' || target==null ) {
			return validSpan.prev();
		}
		else {
			var type = validSpan.attr( 'type' );
			return $( getTargetSelector( target, type ) );
		};
	};

	function getTargetSelector( target, type ) {
		if( type=='undefined' || type==null ) type='input';
		if( type=='input' || type=='password' || type=='hidden' || type=='textarea' ) return 'input[name=\''+target+'\']';
		if( type=='radio' || type=='checkbox' ) return 'input[name=\''+target+'\']:checked';
		if( type=='select' ) return 'select[name=\''+target+'\']:selected';
        return 'input[name=\''+target+'\']';
	};
};

wojilu.ui.ajaxFormCallback = function(thisForm,validFunc) {

    if( wojilu.ctx.isValid==false ) return false;
        
    var actionUrl = $(thisForm).attr( 'action' ).toAjax();
    var loadingInfo = $(thisForm).attr( 'loading' );
    loadingInfo = loadingInfo ? loadingInfo : "loading...";
    
    var formValues = $(thisForm).serializeArray();
    
    var btnSubmit = $( ':submit', thisForm );
    btnSubmit.attr( 'disabled', 'disabled' );
    btnSubmit.after( ' <span class="loadingInfo"><img src="'+wojilu.path.img+'/ajax/loading.gif"/>' + loadingInfo + '</span>' );
    
    $.post( actionUrl, formValues, function( data ) {
    
        var customCallbackName = $(thisForm).attr( 'callback' );
        if( customCallbackName ) {
            var isContinue = eval( customCallbackName+'(thisForm,data)' );
            if( isContinue==false ) {
                btnSubmit.attr( 'disabled', '' ); 
                return false;
            }
        };
        
        if( wojilu.str.isJson( data )==false ) {
            $( '.loadingInfo', thisForm ).html('');
            btnSubmit.attr( 'disabled', '' ); 
            alert( data );
            return false;
        };
            
        var result = eval( '(' +data+')' );

        $( '.loadingInfo' ).html('');        

        if(result.IsValid){
            validFunc(thisForm,btnSubmit,result);
        }else{
            alert( result.Msg );
            btnSubmit.attr( 'disabled', '' );
        };
        
        return false;                   
    });
    
    return false;
};

wojilu.ui.ajaxForward = function( result ) {
    var ftime = result.Time ? result.Time : 800;
    if( result.IsParent ) {
        if( result.ForwardUrl=='#' ) {
            window.parent.wojilu.tool.reloadPage();
        }
        else {
            window.parent.wojilu.tool.forward( result.ForwardUrl, ftime );
        }
    }
    else {
        wojilu.tool.forward( result.ForwardUrl, ftime );
    };
};

wojilu.ui.ajaxUpdateForm = function() {

    var validFunc = function(thisForm,btnSubmit,result) {
    
        if( result.ForwardUrl ) {
            wojilu.ui.ajaxForward( result );
        };
    
        var insertType = $(thisForm).attr( 'insertType' );
        
        if( 'prepend' == insertType ) {
            $('#'+result.Info).prepend( result.Msg );
        } else {
            $('#'+result.Info).append( result.Msg );
        };
        thisForm.reset();
        btnSubmit.attr( 'disabled', '' );
    };

    $( '.ajaxUpdateForm' ).submit( function() {
        return wojilu.ui.ajaxFormCallback(this, validFunc);
    });
};

wojilu.ui.ajaxPostForm = function() {

    var validFunc = function(thisForm,btnSubmit,result) {    
    
        if( wojilu.str.hasText(result.Msg) ) {        
            var waringInfo = $( '.warning', thisForm );
            if( waringInfo && waringInfo.length>0 ) {
                waringInfo.show();
                waringInfo.html( '<div class="opok">'+result.Msg+'</div>' );
            }
            else
                $(thisForm).prepend( '<div class="warning strong" style="margin:5px 5px 10px 5px;padding:5px 15px;"><div class="opok">' + result.Msg + '</div></div>' );       
        };
            
        if( result.ForwardUrl ) {
            wojilu.ui.ajaxForward( result );
            return;
        };        
        
        if( wojilu.str.hasText(result.Msg) ) {
            setTimeout( function() {
                var newTip = $('.warning', thisForm );
                newTip.hide();
                btnSubmit.attr( 'disabled', '' );
            }, 1500 );
            return;
        }

        btnSubmit.attr( 'disabled', '' );        
    };

    $( '.ajaxPostForm' ).submit( function() {
        return wojilu.ui.ajaxFormCallback(this, validFunc);
    });
};

wojilu.ui.box = {

// 使用方法：
//    wojilu.ui.box.init();
//    wojilu.ui.box.showBoxString( topModeHtml, 200, 100 );

	mouseOffset : null,

	init : function() {    
		
		var boxLayer = document.getElementById( 'boxContents' );
		if( boxLayer ) {
			$( '#boxTitleText' ).html( wojilu.ctx.box.title );
			return;
		};
        
		var boxtext = '<div id="overlay" style="display:none"></div>';
		boxtext += '<div id="box" style="display:none;"><div id="boxInner">';
        boxtext += '<div id="boxTitle">';
		boxtext += '<div id="boxTitleText">'+ wojilu.ctx.box.title +'</div>';
		boxtext += '<div id="boxClose" onClick="wojilu.ui.box.hideBox()" title="'+lang.closeBox+'"></div>';
        boxtext += '<div style="clear:both;"></div></div>';
		boxtext += '<div id="boxContents"></div>';
		boxtext += '</div></div>';
        
		$( 'body' ).append( boxtext );
		$( '#boxTitle' ).mousedown( wojilu.ui.box.startMove );
	},

	loading : function() {
		if( !wojilu.ctx.box.isShowing ) {
			$('#box').css('width', '400px');
            $('#box').css('height', '200px');
			this.showBox();
		};        
	},
    
    hideBg : function() {
        $('#overlay').css( 'opacity', 0 );
    },

	//---------------------------------- show/hide ----------------------------------

	showBoxString : function(content, boxWidth, boxHeight){
		this.setWidthHeight(boxWidth, boxHeight);
		$('#boxContents').html( content );
		var contentWidth = $(content).css( 'width' );
		
		if( $(content).length==1 && contentWidth != 'auto'  ) {
			$('#box').css('width', contentWidth);
		};
        
		this.showBox();
		return false;
	},

	setWidthHeight : function(width, height) {
		if(width) {
			if(width < $(window).width()) {
				$('#box').css('width',width + 'px');
			} else {
				$('#box').css('width',($(window).width() - 50) + 'px');
			}
		};
		if(height) {
			if(height < $(window).height()) {
                $('#box').css('height',height + 'px');                
			} else {
				$('#box').css('height', ($(window).height() - 50) + 'px');
			}
		};
        this.resetBoxPosition();
	},

	showBox : function() {
        if( wojilu.ctx.box.isShowing ) return;
        
		var box = $('#box');
        
		var oeverlay = $('#overlay');
        oeverlay.css('width', '100%' );
        oeverlay.css('height', $(document).height() + 'px' );
		oeverlay.show();

		box.css( 'position', 'absolute' );
		box.css( 'z-index', 199);
		
		var box_x = ( $(window).width()  - box.width()  ) / 2;
		var box_y = ( $(window).height() - box.height() ) / 2 + $(document).scrollTop() -30;
		box.css( 'left', box_x + 'px' );
		box.css( 'top', box_y + 'px' );
		box.show();
		wojilu.ctx.box.isShowing = true;
		return false;
	},
    
    resetBoxPosition : function() {
        var box = $('#box');
		var box_x = ( $(window).width()  - box.width()  ) / 2;
		var box_y = ( $(window).height() - box.height() ) / 2 + $(document).scrollTop() -30;
		box.css( 'left', box_x + 'px' );
		box.css( 'top', box_y + 'px' );    
    },

	hideBox : function(){
		$('#boxContents').html( '' );
		$('#box').css('width',null);
		$('#box').css('height',null);
		$('#box').hide(); 
		$('#overlay').hide();
        $('#loadingDiv').hide();
		wojilu.ctx.box.isShowing = false;
		wojilu.ctx.box.param = null;
		wojilu.ctx.box.title = lang.boxTitle;
		return false;
	},

	//---------------------------------- drag ----------------------------------
    
	startMove : function(e) {
		if( document.all ) {
			document.onmousemove=wojilu.ui.box.move;
			document.onmouseup=wojilu.ui.box.endMove;
			$( '#boxTitle' )[0].setCapture();
		}
		else {
			$(document).mousemove( wojilu.ui.box.move );
			$(document).mouseup( wojilu.ui.box.endMove );
		}
		var target = $( '#box' );
		wojilu.ui.box.mouseOffset = wojilu.position.getMouseOffset( target[0], e);
	},

	move : function(e) {
		var target = $('#box');
		e = e || window.event;
		var newMousePosition = wojilu.position.getMouse(e);
		target.css( 'left', newMousePosition.x-wojilu.ui.box.mouseOffset.x );
		target.css( 'top', newMousePosition.y-wojilu.ui.box.mouseOffset.y );
	},

	endMove : function(e) {		
		if( document.all ) {
			document.onmousemove=null;
			document.onmouseup=null;
			$('#boxTitle')[0].releaseCapture();
		}
		else {
			$(document).unbind( 'mousemove' );
			$(document).unbind( 'mouseup' );
		}
	}
};

wojilu.flashUpload = {};
wojilu.flashUpload.initJs = function() {
    var spath = wojilu.path.flash + '/swfupload/';
    function loadjs( sjs ) {wojilu.tool.loadJsFull( spath+sjs );};
    wojilu.tool.loadCss( spath+'default.css' );
    loadjs( 'swfupload.js' );
    loadjs( 'plugins/swfupload.queue.js' );
    loadjs( 'plugins/swfupload.cookies.js' );
    loadjs( 'fileprogress.js' );
    loadjs( 'handlers.js' );
};

wojilu.flashUpload.make = function(uploadDiv, uploadLink, s, parmas) {

    var spath = wojilu.path.flash + '/swfupload/';
    var cdomainPath = '/static/flash/swfupload/';
    var max = s ? s.maxQueue : 0;

    var uploader = '<div style="margin:10px;">';
    uploader += '<div>';
    uploader += '<span id="btnSelect"></span> <span class="note left5">你一次最多可以选择的图片个数：'+max+'</span>';
    uploader += '<input id="btnCancel" type="button" value="取消所有上传" disabled="disabled" class="left10" />';
    uploader += '</div>';
    uploader += '<div id="totalStatus"></div>    ';
    uploader += '<div id="uploadProgress"></div>';
    uploader += '</div>';

    $('#'+uploadDiv).html( uploader );        
    $('#btnCancel').hide();

    var settings = {
        flash_url : cdomainPath+'swfupload.swf',
        upload_url: uploadLink,
        post_params: parmas,
        file_size_limit : s ? s.maxSize : '800 MB',
        file_types : s ? s.fileType : '*.*',
        file_types_description : s ? s.fileDesc : 'All Files',
        file_upload_limit : 0, // 0表示不限制大小
        file_queue_limit : max,
        custom_settings : {
            progressTarget : 'uploadProgress',
            cancelButtonId : 'btnCancel',
            errors : 0
        },
        debug: false,

        // Button settings
        button_image_url: spath+'images/btnSelect160-24.png',
        button_width: '160',
        button_height: '24',
        button_placeholder_id: 'btnSelect',
        button_text: '<span class="button">请选择图片...</span>',
        button_text_style : '.button {font-size: 14pt; color:#ffffff;margin-left:40px;}',
        button_text_top_padding: 2,
        button_text_left_padding: 5,
        button_cursor : SWFUpload.CURSOR.HAND,

        // The event handler functions are defined in handlers.js
        file_queued_handler : fileQueued,
        file_queue_error_handler : fileQueueError,
        file_dialog_complete_handler : fileDialogComplete,
        upload_start_handler : uploadStart,
        upload_progress_handler : uploadProgress,
        upload_error_handler : uploadError,
        upload_success_handler : uploadSuccess,
        upload_complete_handler : uploadComplete,
        queue_complete_handler : queueComplete	// Queue plugin event
    };

    var swfu = new SWFUpload(settings);
    
    $('#btnCancel').click( function() {
        swfu.cancelQueue();
    });
};

wojilu.ui.frmBox = function() {

    $('.frmBox' ).click( frmBoxCallback );
    
    function writeFrm( frmId, htmlContent ) {
        var frmDoc;
		if( document.all ) {
            frmDoc = frames[ frmId ].document;
        } else { 
            frmDoc = $( '#'+frmId )[0].contentWindow.document; 
        };
		frmDoc.open();
		frmDoc.write( htmlContent );
		frmDoc.close();
    };

    function frmBoxCallback() {
    
		var boxTitle = $(this).attr( 'title' );
		if( boxTitle ) wojilu.ctx.box.title = boxTitle;

		wojilu.ui.box.init();
		wojilu.ui.box.loading();        

		var actionUrl = $(this).attr( 'href' ).toAjaxFrame();    

        var boxWidth = $(this).attr( 'xwidth' );
        var boxHeight = $(this).attr( 'xheight' );
        if( !boxWidth ) boxWidth=500;
        if( !boxHeight ) boxHeight = 200;
        
        wojilu.ui.box.setWidthHeight(boxWidth, boxHeight);        

        var ptop = $('#box').css( 'top' ).cssVal()+26+8;
        var pleft = $('#box').css( 'left' ).cssVal()+8;
        var pwidth = $('#box').css( 'width' ).cssVal();
        
        var loadingHeight = boxHeight-26;
        
        var loadingDiv = $('#loadingDiv');
        if( loadingDiv.length>0 ) {
            loadingDiv.css( 'top', ptop+'px');
            loadingDiv.css( 'left', pleft+'px');
            loadingDiv.css( 'width', pwidth+'px');
            loadingDiv.show();
        }
        else {
            var lp = 'position:absolute;z-index:900;top:'+ptop+'px;left:'+pleft+'px;width:'+pwidth+'px;height:'+loadingHeight+'px;background:#fff;border:0px blue solid;';
            var lstyle = 'width:200px;margin:30px auto;';
            var loadingDiv = '<div id="loadingDiv" style="'+lp+'"><iframe id="loadingFrm" frameborder="0" width="100%;height:'+loadingHeight+'px;"></iframe></div>';
            var loadingBody = '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml"><body><table style="'+lstyle+'"><tr><td><img src="'+wojilu.path.img+'/ajax/big.gif"/></td><td style="font-size:18px;font-weight:bold;font-family:verdana;padding-left:5px;">loading...</td></tr></table></body></html>';
            $('body').append( loadingDiv );
            writeFrm( 'loadingFrm', loadingBody );
        }
        
        var frmHtml = '<iframe id="boxFrm" src="'+actionUrl+'" frameborder="0" width="'+pwidth+'" scrolling="no" style="padding:0px;margin:0px;border:0px red solid;height:'+loadingHeight+'px;"></iframe>';
        wojilu.ui.box.showBoxString( frmHtml, boxWidth, boxHeight );        
       
        return false;
    };
};

wojilu.ui.ajaxUpdate = function(callback) {

	$( '.ajaxUpdate' ).click( function() {
		var actionUrl = $(this).attr( 'href' ).toAjax();
		var targetId = $(this).attr( 'update' );
		var target = $( '#' + targetId );
		$.get( actionUrl, function(data){
			target.html( data );
            if( callback ) callback();
		});
		return false;
	});
};

wojilu.ui.frmUpdate = function() {
    $('.frmUpdate').click( function() {

		var frmUrl = $(this).attr( 'href' ).toAjaxFrame();
		var loadTo = $(this).attr( 'loadTo' );
		var divLoad = $( '#' + loadTo );		
        var isHidden = ( divLoad.css( 'display' ) == 'none' );
        
        var txt;
        if( isHidden ) {
            txt = $(this).text();
		    $(this).attr( 'txt', txt );
        }
        else {
            txt = $(this).attr( 'txt' );
        }
        
        var txtHidden = $(this).attr( 'txtHidden' );
        if( !txtHidden ) txtHidden = txt;
		
		if( isHidden==false  ) {
		    divLoad.hide();
		    $(this).text( txt );
		    return;
		}
		
		$(this).text( txtHidden );
		
		var xwidth = divLoad.parent().width();
        var frmId = 'updateFrm'+loadTo;		    
        var frmHtml = '<iframe id="'+frmId+'" src="'+frmUrl+'" frameborder="0" width="'+xwidth+'" height="100" scrolling="no" style="padding:0px;margin:0px;"></iframe>';
		divLoad.show();
		divLoad.html( frmHtml );
		
		$('#'+frmId).load( function() {
		    var commentsWrap = $('#'+frmId).contents().find('div');
		    $('#'+frmId).height( commentsWrap.height()+20 );
		});

		return false;
    });
};

wojilu.ui.editFontSize = function() {

    var getFontSize = function( target ) {
        var fontSize = target.css( 'font-size' );
        var intFontSize = parseInt( wojilu.str.trimEnd( fontSize, 'px' ) );
        return intFontSize;
    };
    
    var getContent = function(target) {
        return $( '#'+ $(target).attr( 'fontTarget' ));
    };
    
    $('.fontBig').click( function() {
        var content = getContent(this);
        var fontSize = getFontSize(content);
        var maxSize = 20;
        var newSize = fontSize>=maxSize ? maxSize:(fontSize+2);
        content.css( 'font-size', newSize+'px');
    });
    
    $('.fontSmall').click( function() {
        var content = getContent(this);  
        var fontSize = getFontSize(content);
        var minSize = 12;
        var newSize = fontSize<=minSize ? minSize:(fontSize-2);
        content.css( 'font-size', newSize+'px');
    });
};

wojilu.ui.editor = function() {
    $('.wEditor').each( function() {
    
        var eName = $(this).attr( 'name' );
        var eHeight = $(this).css( 'height' );
        var ePath = $(this).attr( 'path' );
        var eContent = $(this).attr( 'content' );
        
        var editorPanel = eName.replace( '.', '_' )+'Editor';    
        $(this).after( '<div id=\"'+editorPanel+'\"></div>' );
        $(this).remove();
        
        $.getScript( ePath+'editor.js', function() {
            new wojilu.editor( {editorPath:ePath, height:eHeight, name:eName, content:eContent, toolbarType:'full', uploadUrl:'', mypicsUrl:'' } ).render();
        });
        
    });
};

wojilu.ui.doubleClick = function() {
    $('.click2').dblclick( function() {
        window.location.href = $(this).attr( 'href');
    });
};

$(document).ready( function() {
    wojilu.ui.menu();
    wojilu.ui.clickMenu();
    wojilu.ui.tab();
    wojilu.ui.httpMethod();
    wojilu.ui.frmBox();
    wojilu.ui.boxCancel();
    wojilu.ui.editor();
});
