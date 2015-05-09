

function getChoiceList() {
    var choiceList = [];
    $('input[name=selectThis]:checked').each( function(i){
        choiceList.push(this.id.replace('checkbox',''));
    });
    return choiceList;
}

$(document).ready( function() {

    wojilu.ui.pageReturn();
    wojilu.ui.tip();
    wojilu.ui.valid();
    wojilu.ui.ajaxPostForm();
    wojilu.ui.doubleClick();
    
	// 列表管理：全选
	$('#selectAll').click( function() {    
		if( this.checked ) {
			$('#dataAdminList :checkbox').check('on');
            $('input[name=selectThis]', '#dataAdminList' ).parent().parent().css( 'background', '#FFFFE0' );
        }
		else {
			$('#dataAdminList :checkbox').check('off');
            $('input[name=selectThis]', '#dataAdminList' ).parent().parent().css( 'background', '#FFFFFF' );
        }
	});
    
    $('input[name=selectThis]').click( function() {
		var thisRow = $(this).parent().parent();
		if( this.checked )
			thisRow.css( 'background', '#FFFFE0' );
		else
			thisRow.css( 'background', '#ffffff' );
	});
	

	var getActionUrl = function() {	return $('#dataAdminList').attr('data-action').toAjax();	}

	// 数据管理
	var admindata = function(actionName) {

        var actionUrl = getActionUrl();

        var choiceList = getChoiceList();
        if( choiceList.length == 0 ) {
            alert( lang.exSelect );
            return;
        }

        // 删除的特殊处理
        if( actionName == 'deletetrue' && confirm( lang.deleteTrueTip )==false  ) return;

        $.post( actionUrl, {'choice':choiceList, 'action':actionName},  function(data) {
            if( data=='ok' ) {
                wojilu.tool.reloadPage();
            }
            else {
                var result = eval( '('+data+')' );
                if( result.IsValid ) 
                    wojilu.tool.reloadPage();
                else
                    alert( result.Msg );
            }
        });
	}    

    $('.btnCmd').click( function() {
        admindata( $(this).attr( 'cmd' ) );
    });

	$( '#adminDropCategoryList' ).change( function() {
		var actionUrl = getActionUrl();
		var choiceList = getChoiceList();
		if( choiceList.length == 0 ) {
			alert( lang.exSelect );
			return;
		}

		var categoryId = parseInt( $(this).children(':selected').val() );
		if( categoryId<=0 ) {
		    alert( '请选择分类' );
		    return;
		}

		$.post( actionUrl, {'choice':choiceList, 'action':'category', 'categoryId' : categoryId},  function(data) {
			if( data=='ok' ) {
				wojilu.tool.reloadPage();
			}
			else {
                var result = eval( '('+data+')' );
                if( !result.IsValid ) alert( result.Msg );
			}
		});


	});

	// 分类管理
	$( '#categoryList .delete' ).each( function(i) {
		$(this).click( function() { return confirm( lang.deleteCategory ); });		
	});
    
    // 排序
	function postSortData( pdata ) {
	
		var actionUrl = $('#dataAdminList').attr( 'data-sortAction' ).toAjax();
	
		$.post( actionUrl, pdata, function(data) {
			var result = eval( '('+data+')' );
			if( result.IsValid )
				wojilu.tool.reloadPage();
			else
				alert( result.Msg );
		});
	};
	
	$('.cmdUp').click( function() {	
		var id= $(this).attr( 'data-id' );
		postSortData( {'id':id, cmd:'up'} );		
	});
	
	$('.cmdDown').click( function() {	
		var id= $(this).attr( 'data-id' );		
		postSortData( {'id':id,cmd:'down'} );		
	});
	
});