<?php
  include_once("../php/database.php");
  $db = new database();
  if ($_SERVER['REQUEST_METHOD'] === 'POST') {

  }
  else if ($_SERVER['REQUEST_METHOD'] === 'GET'){
    if(isset($_GET["question"]) && isset($_GET["key"])){
      if($db->IsApiKeyValid($_GET["key"])){
        $db->GetAnswer($_GET["question"]);
      }else{
        echo "Error: Api key is not valid.";
      }
    }else{
      echo "Error: Wrong request check you api key and question.";
    }
  }
?>
