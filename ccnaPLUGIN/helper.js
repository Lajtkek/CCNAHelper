window.addEventListener("load", function(){
  var a = document.getElementsByClassName("current");
  var b = document.getElementsByClassName("answered");
  var c = document.getElementsByClassName("default");

  for (var i = 0 ; i < a.length; i++) {
   a[i].addEventListener("click", function() {CopyToClip();});
  }

  for (var i = 0 ; i < b.length; i++) {
   b[i].addEventListener("click", function() {CopyToClip();});
  }

  for (var i = 0 ; i < c.length; i++) {
   c[i].addEventListener("click", function() {CopyToClip();});
  }
});

function CopyToClip(){
  var text = document.getElementsByClassName("question");

  if(text != null){
  for(var i = 0; i < text.length; i++){
    if(!text[i].classList.contains("hidden")){
      text = text[i].getElementsByClassName("coreContent")[0];
    }
  }

  if(text != null){
      text =  text.innerHTML;
      console.log(text);

      navigator.clipboard.writeText(text)
      .then(() => {
        console.log('Text copied to clipboard');
      })
      .catch(err => {
        console.error('Could not copy text: ', err);
      });
    }
  }
}
