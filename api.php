<?php
header("Content-Type: application/json");

$host = 'localhost';
$db = 'hr';
$user = 'root'; 
$pass = ''; 

$charset = 'utf8mb4';

$dsn = "mysql:host=$host;dbname=$db;charset=$charset";
$options = [
    PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
    PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
    PDO::ATTR_EMULATE_PREPARES => false,
];

try {
    $pdo = new PDO($dsn, $user, $pass, $options);

    if ($_SERVER['REQUEST_METHOD'] === 'GET') {
        $stmt = $pdo->query("SELECT a.userid, a.username, a.pass, a.email, p.bio 
                             FROM accounts a
                             LEFT JOIN profiles p ON a.userid = p.userid");
        $users = $stmt->fetchAll();
        echo json_encode($users);
    } elseif ($_SERVER['REQUEST_METHOD'] === 'POST') {
        $input = json_decode(file_get_contents('php://input'), true);
        if (isset($input['username'], $input['pass'], $input['email'], $input['bio'])) {

            $sql = "INSERT INTO accounts (username, pass, email) VALUES (?, ?, ?)";
            $stmt = $pdo->prepare($sql);
            $stmt->execute([$input['username'], $input['pass'], $input['email']]);
            
            $sql = "INSERT INTO profiles (userid, bio) VALUES (?, ?)";
            $stmt = $pdo->prepare($sql);
            $stmt->execute([$pdo->lastInsertId(), $input['bio']]);
            
            echo json_encode(['message' => 'User added successfully']);
        } else {
            echo json_encode(['error' => 'Invalid input']);
        }
    }
} catch (PDOException $e) {
    http_response_code(500);
    echo json_encode(['error' => 'Database connection failed: ' . $e->getMessage()]);
}
?>
