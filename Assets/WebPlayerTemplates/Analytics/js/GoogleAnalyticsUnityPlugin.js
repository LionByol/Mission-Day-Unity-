function SendGA (host, data) {

	var postData =  $.getQueryParameters(data);

 	console.log(postData);
	$.ajax({
	    type:  "POST",
	   
	    url: host,
	    data: postData,
	    success: function(msg){
	        console.log(msg);
	    }
	});
}


jQuery.extend({
  getQueryParameters : function(str) {
	  return (str || document.location.search).replace(/(^\?)/,'').split("&").map(function(n){return n = n.split("="),this[n[0]] = n[1],this}.bind({}))[0];
  }

});