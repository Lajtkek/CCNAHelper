var array = new Array();

$("ol > li").each(function(){
  var question = {};
  question.Question = $(this).find("strong").first().text();

  var answers = new Array();

  $(this).find("strong").each(function() {
    if($(this).css("color") == "rgb(255, 0, 0)"){
    answers.push($(this).text());
    }
  })
  question.Answers = answers;
  array.push(question);
})

console.log(JSON.stringify(array));
