<?php    //error_reporting (0);
class database{
    private $servername = 'localhost';
    private $username = 'root';
    private $password = '';
    private $database  = 'CCNAdatabase';

    private $conn;
    private $url = "http://localhost";

    function __construct() {
        $this->conn = new mysqli($this->servername, $this->username, $this->password,$this->database);
        $this->conn->set_charset('utf8');

    }

    function GetAnswer($question){
      $sql = "SELECT a.answer FROM QUESTIONS q
              JOIN answers a on (a.id_q = q.id_q)
              WHERE q.body LIKE '%$question%'";

      $result = $this->conn->query($sql);

      $answers = array();

      while($row = $result->fetch_assoc()) {
          array_push($answers,$row["answer"]);
      }

      echo json_encode($answers)  ;
    }

    function IsApiKeyValid($key){
      $sql = "SELECT * FROM api_keys WHERE api_key = '$key'";

      $result = $this->conn->query($sql);

      if ($result->num_rows > 0) {
          return true;
      } else {
          return false;
      }
    }
}

?>
