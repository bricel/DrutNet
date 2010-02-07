<?php
// $Id:

/**
 * SQLite specific install functions
 *
 * This file is part of Drupal-SQLite by CoolSoft,
 * a patched Drupal 6 version that works with a SQLite database layer.
 *
 * @author Claudio Nicora (coolsoft@altervista.org)
 * @see http://coolsoft.altervista.org/drupal-sqlite
 */

/**
 * Check if SQLite is available.
 *
 * @return
 *  TRUE/FALSE
 */
function sqlite_is_available() {
  return class_exists('PDO') && extension_loaded('pdo_sqlite');
}

/**
 * Check if we can connect to SQLite.
 *
 * @return boolean
 *  TRUE/FALSE
 */
function drupal_test_sqlite($url, &$success) {

  if (!sqlite_is_available()) {
    drupal_set_message(st('PHP SQLite support not enabled.'), 'error');
    return FALSE;
  }

  // we are interested only to path component, removing the leading "/"
 	$url = parse_url(urldecode($url));
 	if (!isset($url['path'])) {
 	  drupal_set_message(st('Failed to parse SQLite database path.'));
 	  return FALSE;
 	} else {
 	  $url = substr($url['path'], 1);
 	}

 	// test if user set an absolute path.
 	// in this case we have another leading "/"
 	if (substr($url['path'], 0, 1) === '/') {
 	  drupal_set_message('Database path must be relative (not starting with /), like "sites/default/db.s3db"');
 	  return FALSE;
 	}

 	// now $url contains the relative path to the db file
 	// let's test if it exists
 	$path = dirname($url);
  if(!is_dir($path)) {
    // create the folder
    mkdir($path, 0777, true);
    // create an .htaccess file to prevent DB file download
    $path_info = pathinfo($url);
    $file_ext = $path_info['extension'];
    $htfile = <<<EOF
<FilesMatch "\.${file_ext}$">
  Order allow,deny
</FilesMatch>
EOF;
    file_put_contents("$path/.htaccess", $htfile);
 	}

  // create .htaccess file
  $path = dirname($url);
  if(!is_dir($path)) {
    // create the folder
    mkdir($path, 0777, true);
  }

 	// test access permission to database folder
 	if(!is_writable($path)) {
    drupal_set_message('Drupal-SQLite needs read/write access both to database
    file and the folder containing it.
    <br/>Folder '.$path.' is not writable.
    <br/>Please fix permissions and retry.',
    'error');
    return FALSE;
 	}

 	// Build pdo_sqlite connection string.
 	$dsn = 'sqlite:' . $url;

 	$driver_options = array(
 	// Throw a PDOException and set its properties to reflect the error code
 	// and error information.
 	PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
 	//  Leave column names as returned by the database driver.
 	PDO::ATTR_CASE => PDO::CASE_NATURAL,
 	);

 	// Test connecting to the database.
 	// This will create the DB if it doesn't exist.
 	$connection = new PDO($dsn, '', '', $driver_options);
 	if (!$connection) {
 	  drupal_set_message(st('Failed to connect to your SQLite database. SQLite reports the following message: %error.', array('%error' => sqlite_install_db_error())), 'error');
 	  return FALSE;
 	}
 	$success = array('CONNECT');

 	// Since SQLite has no permissions, once we successfully connected to the DB
 	// we could skip SELECT/CREATE/INSERT/UPDATE/DROP tests
 	$success[] = 'SELECT';
 	$success[] = 'CREATE';
 	$success[] = 'INSERT';
 	$success[] = 'UPDATE';
 	$success[] = 'DELETE';
 	$success[] = 'DROP';

 	// return True
 	return TRUE;
}


/**
 * Returns a formatted error message
 */
function sqlite_install_db_error($stmt) {
  $error = $stmt->errorInfo();
  if (is_array($error) && isset($error[2])) {
    return $error[2];
  }
  return false;
}
