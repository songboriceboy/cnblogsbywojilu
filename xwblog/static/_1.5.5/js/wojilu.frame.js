
window.parent.wojilu.tool.hideLoading();

$(document).ready( function() {
    function toAjaxFrame(url) {
    
        function endsWith( txt, value ) {
            return ( txt.substr( txt.length-value.length, txt.length ) == value ) ;
        };
        
        if( url.indexOf( 'frm=true' )>0 ) return url;
    	var indexQuery = url.indexOf( '?' );
    	if( indexQuery<0 ) {
    		return url + '?frm=true';
    	}
    	else {
            if( endsWith( url, '?' ) ) return url + 'frm=true';
            if( endsWith( url, '&' ) )  return url + 'frm=true';
            return url + '&frm=true';
        };
    };
    
    var frmlnk = function() {
        var url = toAjaxFrame($(this).attr( 'href' ));
        $(this).attr( 'href', url );
    };
    
    $('a').click(frmlnk);
    $('.deleteCmd').click(frmlnk);

    $('form').each( function() {
        $(this).append( '<input name="frm" type="hidden" value="true" />' );
    });
 
});
