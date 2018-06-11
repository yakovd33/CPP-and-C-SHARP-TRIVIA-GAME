<?php
    $target_dir = "uploads/";
    
    if(!isset($_FILES["file"])) {
        die('please send a file');
    }
    
    $uploadOk = 1;
    $imageFileType = strtolower(pathinfo($target_dir . basename($_FILES["file"]["name"]),PATHINFO_EXTENSION));
    $filename = md5(time() + rand(0, 10000)) . '.' . $imageFileType;
    $target_file = $target_dir . $filename;

    // Check if image file is a actual image or fake image
    if(isset($_POST["file"])) {
        $check = getimagesize($_FILES["file"]["tmp_name"]);
        if($check !== false) {
            $uploadOk = 1;
        } else {
            die();
            $uploadOk = 0;
        }
    }

    // Check file size
    if ($_FILES["file"]["size"] > 500000) {
        $uploadOk = 0;
    }

    // Allow certain file formats
    if($imageFileType != "jpg" && $imageFileType != "png" && $imageFileType != "jpeg"
    && $imageFileType != "gif" ) {
        $uploadOk = 0;
    }
    // Check if $uploadOk is set to 0 by an error
    if ($uploadOk == 0) {
        //echo "Sorry, your file was not uploaded.";
    // if everything is ok, try to upload file
    } else {
        if (move_uploaded_file($_FILES["file"]["tmp_name"], $target_file)) {
            echo 'uploads/' . $filename;
        } else {
        }
    }
?>