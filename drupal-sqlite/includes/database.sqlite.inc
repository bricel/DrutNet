<?php
// $Id:

/**
 * Database interface code for SQLite
 * Based on the original database.mysql.inc and database.mysql-common.inc
 *
 * This file is part of Drupal-SQLite by CoolSoft,
 * a patched Drupal 6 version that works with a SQLite database layer.
 *
 * @author Claudio Nicora (coolsoft@altervista.org)
 * @see http://coolsoft.altervista.org/drupal-sqlite
 */

/**
 * Minimum supported version of SQLite.
 * It is defined here instead of 'modules/system/system.module'
 * to prevent it being overwritten by future Drupal updates.
 */
define('DRUPAL_MINIMUM_SQLITE',  '3.0.0');

/**
 * Suffix for index names.
 */
define('SQLITE_INDEX_NAME_SUFFIX', '_IDX');

/**
 * Include code to to patch core queries before they are executed.
 * This is done because I want to keep _db_query() as clean as possible.
 */
require('database.sqlite.core-patches.inc');

/**
 * Optional user file where user can add patches for external module queries.
 * I don't want Drupal-SQLite to include query patches for each
 * existing module, so put yours here
 * (see "database.sqlite.user-patches.sample" as a sample).
 */
@include('database.sqlite.user-patches.inc'); // @ for "no warning"

/**
 * Report database status.
 */
function db_status_report($phase) {
  $t = get_t();

  $version = db_version();

  $form['sqlite'] = array(
    'title' => $t('SQLite database'),
    'value' => ($phase == 'runtime') ? l($version, 'admin/reports/status/sql') : $version,
  );

  if (version_compare($version, DRUPAL_MINIMUM_SQLITE) < 0) {
    $form['sqlite']['severity'] = REQUIREMENT_ERROR;
    $form['sqlite']['description'] = $t('Your SQLite library is too old. Drupal requires at least version %version.', array('%version' => DRUPAL_MINIMUM_SQLITE));
  }

  return $form;
}


/**
 * Returns the version of the database server currently in use.
 *
 * @return string Database server version
 */
function db_version() {
  return db_result(db_query("SELECT SQLITE_VERSION()"));
}


/**
 * Initialize a database connection.
 */
function db_connect($url) {

  // parse the DB url
  $url = parse_url(urldecode($url));

  // Check if PDO SQLite support is present in PHP.
  if (!(class_exists('PDO') && extension_loaded('pdo_sqlite'))) {
    _db_error_page('Unable to use the SQLite database because the pdo_sqlite extension for PHP is not installed. Check your php.ini to see how you can enable it.');
  }

  // Build pdo_sqlite connection string.
  if (!isset($url['path'])) {
    throw new Exception("db_url not set in settings.php");
  } else {
    // removes the leading "/"
    $dsn = 'sqlite:' . substr($url['path'],1);
  }

  $driver_options = array(
    // Throw a PDOException and set its properties to reflect the error code
    // and error information (will change after connection)
    PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,    // will change later
    //  Leave column names as returned by the database driver.
    PDO::ATTR_CASE => PDO::CASE_NATURAL,
    // Timeout
    PDO::ATTR_TIMEOUT => 20,
  );

  try {
    $connection = new PDO($dsn, '', '', $driver_options);
  } catch (PDOExecption $e) {
    _db_error_page($e->getMessage());
    return false;
  }

  // reset PDO SQLite error mode
  $connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_WARNING);

  // Modify the operation of the SQLite library
  $connection->exec('PRAGMA synchronous=OFF');
  $connection->exec('PRAGMA short_column_names=1');
  $connection->exec('PRAGMA temp_store=MEMORY');
  $connection->exec('PRAGMA cache_size=4000');
  $connection->exec('PRAGMA count_changes=0');
  $connection->exec('PRAGMA encoding="UTF-8"');


  /*
   * From PHP documentation:
   * This function is EXPERIMENTAL. The behaviour of this function,
   * its name, and surrounding documentation may change without notice
   * in a future release of PHP. This function should be used at your own risk.
   * http://it2.php.net/manual/en/function.pdo-sqlitecreatefunction.php
   * so it's better to test it.
   */
  if(!method_exists($connection, 'sqliteCreateFunction')){
    ${DB_ERROR} = TRUE;
    trigger_error('Error while adding custom SQLite functions.<br />PDO::sqliteCreateFunction method is missing.', E_USER_WARNING);
  } else {
    // register functions which are not built-in in SQLite
    $connection->sqliteCreateFunction('concat', '_sqlite_concat');
    $connection->sqliteCreateFunction('substring', '_sqlite_substring');
    $connection->sqliteCreateFunction('greatest', '_sqlite_greatest');
    $connection->sqliteCreateFunction('rand', '_sqlite_rand');
    $connection->sqliteCreateFunction('if', '_sqlite_if', 3);
    $connection->sqliteCreateFunction('year', '_sqlite_year', 1);
    $connection->sqliteCreateFunction('month', '_sqlite_month', 1);
    $connection->sqliteCreateFunction('FROM_UNIXTIME', '_sqlite_from_unix_timestamp', 1);
    $connection->sqliteCreateFunction('pow', '_sqlite_pow', 2);
    $connection->sqlitecreateFunction('MD5', '_sqlite_md5', 1);
    $connection->sqlitecreateFunction('DATE_FORMAT', '_sqlite_dateformat', 2);
  }

  if(!method_exists($connection, 'sqliteCreateAggregate')){
    ${DB_ERROR} = TRUE;
    trigger_error('Error while adding custom SQLite functions.<br />PDO::sqliteCreateAggregate method is missing.', E_USER_WARNING);
  } else {
    // register functions which are not built-in in SQLite
    $connection->sqliteCreateAggregate('COALESCE', '_sqlite_coalesce_step', '_sqlite_coalesce_finalize');
    $connection->sqliteCreateAggregate('STDDEV', '_sqlite_stddev_step', '_sqlite_stddev_finalize');
  }

  return $connection;
}


/**
 * Helper function for db_query().
 */
function _db_query($query, $debug = 0) {

  global $active_db, $last_result, $queries, $user;

  // Expand $query if it is in array format.
  $args = array();
  if (is_array($query)) {
    $args = $query['args'];
    $query = $query['query'];

    // Check if locator exists in query, or else unset it.
    foreach ($args as $key => $value) {
      if (!is_array($args[$key]) || !isset($args[$key]['locator']) || !strstr($query, $args[$key]['locator'])) {
        unset($args[$key]);
      }
    }
  }

  /**
   * Run core query rewrite rules, from file
   * 'database.sqlite.core-patches.inc'.
   * This is done because I want to keep _db_query() as clean as possible.
   */
  _sqlite_core_rewrite_query($query);

  /**
   * Optional user function with custom rewrite rules, from
   * 'database.sqlite.user-patches.inc' file.
   * I don't want Drupal-SQLite to include query patches for each
   * existing module, so put yours there.
   * Use 'database.sqlite.core-patches.sample' as example.
   * $query is a String parameter containing the SQL query ready to be executed.
   */
  if (function_exists('_sqlite_user_rewrite_query')) {
     _sqlite_user_rewrite_query($query);
  }

  // developer module
  if (variable_get('dev_query', 0)) {
    list($usec, $sec) = explode(' ', microtime());
    $timer = (float)$usec + (float)$sec;
    // If devel.module query logging is enabled, prepend a comment with the username and calling function
    // to the SQL string. This is useful when running mysql's SHOW PROCESSLIST to learn what exact
    // code is issueing the slow query.
    $bt = debug_backtrace();
    // t() may not be available yet so we don't wrap 'Anonymous'.
    $name = $user->uid ? $user->name : variable_get('anonymous', 'Anonymous');
    // str_replace() to prevent SQL injection via username or anonymous name.
    $name = str_replace(array('*', '/'), '', $name);
    $query = '/* '. $name .' : '. $bt[2]['function'] .' */ '. $query;
  }

  // prepare a statement
  $last_result = $stmt = $active_db->prepare($query);
  if(!$stmt){
    $error = $active_db->errorInfo();
    if (is_array($error) && isset($error[2])) {
      // Indicate to drupal_error_handler that this is a database error.
      ${DB_ERROR} = TRUE;
      trigger_error(check_plain($error[2]) . '<br />query: ' . check_plain($query), E_USER_WARNING);
    }
    return FALSE;
  }

  if (count($args)) {
    foreach ($args as $key => $value) {
      if (isset($args[$key]['type'])) {
        $stmt->bindValue($args[$key]['locator'], $args[$key]['data'], $args[$key]['type']);
      }
      else {
        $stmt->bindValue($args[$key]['locator'], $args[$key]['data']);
      }
    }
  }

  // execute the statement.
  $result = $stmt->execute();
  if ($stmt->errorCode() !== '00000') {
    ${DB_ERROR} = TRUE;
    trigger_error(check_plain($error[2]) . '<br />query: ' . check_plain($query), E_USER_WARNING);
    return FALSE;
  }

  if (variable_get('dev_query', 0)) {
    $query = $bt[2]['function'] ."\n". $query;
    list($usec, $sec) = explode(' ', microtime());
    $stop = (float)$usec + (float)$sec;
    $diff = $stop - $timer;
    $queries[] = array($query, $diff);
  }

  if ($debug) {
    $error = $active_db->errorInfo();
    print '<p>query: '. $query .'<br />error:'. $error[2] .'</p>';
  }

  return $last_result;
}


/**
 * Fetch one result row from the previous query as an object.
 *
 * @param $result
 *   A database query result resource, as returned from db_query().
 * @return
 *   An object representing the next row of the result, or FALSE. The attributes
 *   of this object are the table fields selected by the query.
 */
function db_fetch_object($result) {
  if ($result) {
    $object = $result->fetch(PDO::FETCH_OBJ);
    if (!is_object($object) || !$object) return FALSE;

    /**
     * Fix for short_column_names not returned
     * by SQLite when runnung JOINed queries.
     * Will retrieve object properties (record columns) and,
     * if some of them contains a '.' in its name,
     * will create an "alias" property without the dot.
     **/
    $query = $result->queryString;
    if (! property_exists($result, 'fix_short_column_names')) {
      $result->fix_short_column_names =
        (strpos($query, 'JOIN')     !== FALSE) &&
        (strpos($query, 'GROUP BY') !== FALSE);
    }

    if ($result->fix_short_column_names) {
	  $objectProps = get_object_vars($object);
	  foreach ($objectProps as $propName => $value) {
	    $pos = strpos($propName, '.');
	    if($pos !== FALSE) {
	      // will create alias only if a property
	      // with the same name does not already exist
	      $aliasProp = substr($propName, $pos+1);
	      if(!array_key_exists($aliasProp, $objectProps)) {
	        $object->$aliasProp = $value;
	      }
	    }
	  }
	}
    return $object;
  }
  return FALSE;
}


/**
 * Fetch one result row from the previous query as an array.
 *
 * @param $result
 *   A database query result resource, as returned from db_query().
 * @return
 *   An associative array representing the next row of the result, or FALSE.
 *   The keys of this object are the names of the table fields selected by the
 *   query, and the values are the field values for this result row.
 */
function db_fetch_array($result) {
  if ($result) {
    $array = $result->fetch(PDO::FETCH_ASSOC);
    return isset($array) ? $array : FALSE;
  }
  return FALSE;
}


/**
 * Return an individual result field from the previous query.
 *
 * Only use this function if exactly one field is being selected; otherwise,
 * use db_fetch_object() or db_fetch_array().
 *
 * @param $result
 *   A database query result resource, as returned from db_query().
 * @return
 *   The resulting field or FALSE.
 */
function db_result($result) {
  if ($result) {
    $array = $result->fetch(PDO::FETCH_NUM);
    return isset($array[0]) ? $array[0] : FALSE;
  }
  return FALSE;
}


/**
 * Determine whether the previous query caused an error.
 */
function db_error() {
  global $last_result;
  if($last_result) {
    $error = $last_result->errorInfo();
    if (is_array($error) && isset($error[2])) {
      return $error[2];
    }
  }
}


/**
 * Determine the number of rows changed by the preceding query.
 */
function db_affected_rows() {
  global $last_result;
  if ($last_result)
    return $last_result->rowCount();
   else
    return 0;
}


/**
 * Runs a limited-range query in the active database.
 *
 * Use this as a substitute for db_query() when a subset of the query is to be
 * returned.
 * User-supplied arguments to the query should be passed in as separate parameters
 * so that they can be properly escaped to avoid SQL injection attacks.
 *
 * @param $query
 *   A string containing an SQL query.
 * @param ...
 *   A variable number of arguments which are substituted into the query
 *   using printf() syntax. The query arguments can be enclosed in one
 *   array instead.
 *   Valid %-modifiers are: %s, %d, %f, %b (binary data, do not enclose
 *   in '') and %%.
 *
 *   NOTE: using this syntax will cast NULL and FALSE values to decimal 0,
 *   and TRUE values to decimal 1.
 *
 * @param $from
 *   The first result row to return.
 * @param $count
 *   The maximum number of result rows to return.
 * @return
 *   A database query result resource, or FALSE if the query was not executed
 *   correctly.
 */
function db_query_range($query) {
  $args = func_get_args();
  $count = array_pop($args);
  $from = array_pop($args);
  array_shift($args);

  $query .= ' LIMIT '. (int) $count .' OFFSET '. (int) $from;
  if (isset($args[0]) and is_array($args[0])) { // 'All arguments in one array' syntax
    $args = $args[0];
  }
  return db_query($query, $args);
}


/**
 * Runs a SELECT query and stores its results in a temporary table.
 *
 * Use this as a substitute for db_query() when the results need to stored
 * in a temporary table. Temporary tables exist for the duration of the page
 * request.
 * User-supplied arguments to the query should be passed in as separate parameters
 * so that they can be properly escaped to avoid SQL injection attacks.
 *
 * Note that if you need to know how many results were returned, you should do
 * a SELECT COUNT(*) on the temporary table afterwards. db_affected_rows() does
 * not give consistent result across different database types in this case.
 *
 * @param $query
 *   A string containing a normal SELECT SQL query.
 * @param ...
 *   A variable number of arguments which are substituted into the query
 *   using printf() syntax. The query arguments can be enclosed in one
 *   array instead.
 *   Valid %-modifiers are: %s, %d, %f, %b (binary data, do not enclose
 *   in '') and %%.
 *
 *   NOTE: using this syntax will cast NULL and FALSE values to decimal 0,
 *   and TRUE values to decimal 1.
 *
 * @param $table
 *   The name of the temporary table to select into. This name will not be
 *   prefixed as there is no risk of collision.
 * @return
 *   A database query result resource, or FALSE if the query was not executed
 *   correctly.
 */
function db_query_temporary($query) {
  $args = func_get_args();
  $tablename = array_pop($args);
  array_shift($args);

  $query = preg_replace('/^SELECT/i', 'CREATE TEMPORARY TABLE '. $tablename .' AS SELECT', $query);
  if (isset($args[0]) and is_array($args[0])) { // 'All arguments in one array' syntax
    $args = $args[0];
  }
  return db_query($query, $args);
}


/**
 * Returns a properly formatted Binary Large Object value.
 *
 * @param $data
 *   Data to encode.
 * @return
 *  Encoded data.
 */
function db_encode_blob($data) {
  // base64_encode is the better choice to store a BLOB in a SQLite field
  // PDO->quote() is not working well since it stops at the first \0 byte
  $res = "'" . base64_encode($data) . "'";
  return $res;
}


/**
 * Returns text from a Binary Large Object value.
 *
 * @param $data
 *   Data to decode.
 * @return
 *  Decoded data.
 */
function db_decode_blob($data) {
  return base64_decode($data);
}


/**
 * Prepare user input for use in a database query, preventing SQL injection attacks.
 */
function db_escape_string($text) {
  global $active_db;
  $res = $active_db->quote($text, PDO::PARAM_STR);
  // removes ' as prefix/suffix
  if(strlen($res) >= 2)
  return substr($res, 1, strlen($res)-2);
  else
  return $res;
}


/**
 * Lock a table.
 */
function db_lock_table($table) {
  global $active_db;
  $active_db->beginTransaction();
}


/**
 * Unlock all locked tables.
 */
function db_unlock_tables() {
  global $active_db;
  $active_db->commit();
}


/**
 * Check if a table exists.
 */
function db_table_exists($table) {
  return (bool) db_result(db_query("SELECT COUNT(*) FROM SQLITE_MASTER WHERE TBL_NAME LIKE '{". db_escape_table($table) ."}'"));
}


/**
 * Check if a column exists in the given table.
 */
function db_column_exists($table, $column) {
  $result = db_query("PRAGMA TABLE_INFO('{". db_escape_table($table) ."}')");
  while ($row = db_fetch_array($result)) {
    if ($row['name'] == $column) {
      return TRUE;
    }
  }
  return FALSE;
}


/**
 * Wraps the given table.field entry with a DISTINCT(). The wrapper is added to
 * the SELECT list entry of the given query and the resulting query is returned.
 * This function only applies the wrapper if a DISTINCT doesn't already exist in
 * the query.
 *
 * @param $table Table containing the field to set as DISTINCT
 * @param $field Field to set as DISTINCT
 * @param $query Query to apply the wrapper to
 * @return SQL query with the DISTINCT wrapper surrounding the given table.field.
 */
function db_distinct_field($table, $field, $query) {
  $field_to_select = 'DISTINCT('. $table .'.'. $field .')';
  $matches = array();
  if (preg_match('/^SELECT(.*?)FROM(.*)/is', $query, $matches)) {
    $select = preg_replace(
      '/((?:^|,)\s*)(?<!DISTINCT\()(?:'. $table .'\.)?'. $field .'(\s*(?:,|$))/is',
      '\1'. $field_to_select .'\2', $matches[1], 1
    );
    return 'SELECT'. $select .'FROM'. $matches[2];
  }
  return $query;
}


/**
 * Runs a basic query in the active database.
 *
 * User-supplied arguments to the query should be passed in as separate
 * parameters so that they can be properly escaped to avoid SQL injection
 * attacks.
 *
 * @param $query
 *   A string containing an SQL query.
 * @param ...
 *   A variable number of arguments which are substituted into the query
 *   using printf() syntax. Instead of a variable number of query arguments,
 *   you may also pass a single array containing the query arguments.
 *
 *   Valid %-modifiers are: %s, %d, %f, %b (binary data, do not enclose
 *   in '') and %%.
 *
 *   NOTE: using this syntax will cast NULL and FALSE values to decimal 0,
 *   and TRUE values to decimal 1.
 *
 * @return
 *   A database query result resource, or FALSE if the query was not
 *   executed correctly.
 */
function db_query($query) {

  $args = func_get_args();
  array_shift($args);
  $query = db_prefix_tables($query);
  if (isset($args[0]) and is_array($args[0])) { // 'All arguments in one array' syntax
    $args = $args[0];
  }
  _db_query_callback($args, TRUE);
  $query = preg_replace_callback(DB_QUERY_REGEXP, '_db_query_callback', $query);
  return _db_query($query);
}


/**
 * Generate SQL to create a new table from a Drupal schema definition.
 *
 * @param $name
 *   The name of the table to create.
 * @param $table
 *   A Schema API table definition array.
 * @return
 *   An array of SQL statements to create the table.
 */
function db_create_table_sql($name, $table) {
  $sql_fields = array();
  foreach ($table['fields'] as $field_name => $field) {
    if ($field['type'] == 'serial' && isset($table['primary key']) && in_array($field_name, $table['primary key'])) {
      unset($table['primary key']);
    }
    $sql_fields[] = _db_create_field_sql($field_name, _db_process_field($field));
  }

  $sql_keys = array();
  if (isset($table['primary key']) && is_array($table['primary key'])) {
    $sql_keys[] = 'PRIMARY KEY (['. implode('], [', $table['primary key']) .'])';
  }
  if (isset($table['unique keys']) && is_array($table['unique keys'])) {
    foreach ($table['unique keys'] as $key_name => $key) {
      $sql_keys[] = 'UNIQUE (['. implode('], [', $key) .'])';
    }
  }

  $sql = "CREATE TABLE [{". $name ."}] (\n\t";
  $sql .= implode(",\n\t", $sql_fields);
  if (count($sql_keys) > 0) {
    $sql .= ",\n\t";
  }
  $sql .= implode(",\n\t", $sql_keys);
  $sql .= "\n)";
  $statements[] = $sql;

  if (isset($table['indexes']) && is_array($table['indexes'])) {
    foreach ($table['indexes'] as $key_name => $key) {
      $statements[] = _db_create_index_sql($name, $key_name, $key);
    }
  }

  // In case of SQLite, use transaction to ensure atomic CREATE TABLE.
  return array_merge(
    array('BEGIN'),
    $statements,
    array('COMMIT')
  );
}


/**
 *
 * @param $table
 * Table name
 * @param $name
 * Name of the index (will be composed as TABLENAME_INDEXNAME_IDX
 * @param $fields
 * Fields of this index
 * @return unknown_type
 */
function _db_create_index_sql($table, $name, $fields, $unique = false) {
  if($unique) {
    $res = 'CREATE UNIQUE INDEX ';
  } else {
    $res = 'CREATE INDEX ';
  }
  $res .= $table . '_'
        . $name . SQLITE_INDEX_NAME_SUFFIX . ' ON '
        . $table .' (' . _db_create_key_sql($fields) .')';
  return $res;
}


/**
 * @param $table
 * Table name
 * @param $name
 * Name of the index (will be composed as TABLENAME_INDEXNAME_IDX
 * @return unknown_type
 */
function _db_drop_index_sql($table, $name) {
  return 'DROP INDEX IF EXISTS '
        . $table . '_' . $name . SQLITE_INDEX_NAME_SUFFIX;
}


/**
 * Set database-engine specific properties for a field.
 *
 * @param $field
 *   A field description array, as specified in the schema documentation.
 */
function _db_process_field($field) {
  if (!isset($field['size'])) {
    $field['size'] = 'normal';
  }

  // Set the correct database-engine specific datatype.
  if (!isset($field['sqlite_type'])) {
    $map = db_type_map();
    $field['sqlite_type'] = $map[$field['type'] .':'. $field['size']];
  }

  if ($field['type'] == 'serial') {
    $field['auto_increment'] = TRUE;
  }

  if ($field['type'] == 'varchar' || $field['type'] == 'char') {
    unset($field['length']);
  }

  return $field;
}


/**
 * Create an SQL string for a field to be used in table creation or alteration.
 *
 * Before passing a field out of a schema definition into this function it has
 * to be processed by _db_process_field().
 *
 * @param $name
 *    Name of the field.
 * @param $spec
 *    The field specification, as per the schema data structure format.
 */
function _db_create_field_sql($name, $spec) {
  $sql = '['. $name .'] '. $spec['sqlite_type'];

  if (!empty($spec['auto_increment'])) {
    $sql .= ' PRIMARY KEY AUTOINCREMENT';
    return $sql;
  }

  if (!empty($spec['unsigned'])) {
    $sql .= " CHECK ([$name] >= 0)";
  }

  if (!empty($spec['length'])) {
    $sql .= '('. $spec['length'] .')';
  }
  elseif (isset($spec['precision']) && isset($spec['scale'])) {
    $sql .= '('. $spec['precision'] .', '. $spec['scale'] .')';
  }

  if (isset($spec['not null']) && $spec['not null']) {
    $sql .= ' NOT NULL';
  }

  if (isset($spec['default'])) {
    $default = is_string($spec['default']) ? "'". $spec['default'] ."'" : $spec['default'];
    $sql .= " DEFAULT $default";
  }

  return $sql;
}


/**
 * Returns the SQL needed to add a primary key
 */
function _db_create_keys_sql($spec) {
  $keys = array();

  if (!empty($spec['primary key'])) {
    $keys[] = 'PRIMARY KEY ('. _db_create_key_sql($spec['primary key']) .')';
  }

  return $keys;
}


/**
 * Returns the SQL needed to add a single key
 */
function _db_create_key_sql($fields) {
  $ret = array();
  foreach ($fields as $field) {
    if (is_array($field)) {
      $ret[] = $field[0]; // .'('. $field[1] .')';
    }
    else {
      $ret[] = $field;
    }
  }
  return implode(', ', $ret);
}


/**
 * This maps a generic data type in combination with its data size
 * to the engine-specific data type.
 */
function db_type_map() {
  // Put :normal last so it gets preserved by array_flip.  This makes
  // it much easier for modules (such as schema.module) to map
  // database types back into schema types.
  $map = array(
    'varchar:normal'  => 'TEXT',
    'char:normal'     => 'TEXT',

    'text:tiny'       => 'TEXT',
    'text:small'      => 'TEXT',
    'text:medium'     => 'TEXT',
    'text:big'        => 'TEXT',
    'text:normal'     => 'TEXT',

    'serial:tiny'     => 'INTEGER',
    'serial:small'    => 'INTEGER',
    'serial:medium'   => 'INTEGER',
    'serial:big'      => 'INTEGER',
    'serial:normal'   => 'INTEGER',

    'int:tiny'        => 'INTEGER',
    'int:small'       => 'INTEGER',
    'int:medium'      => 'INTEGER',
    'int:big'         => 'INTEGER',
    'int:normal'      => 'INTEGER',

    'float:tiny'      => 'REAL',
    'float:small'     => 'REAL',
    'float:medium'    => 'REAL',
    'float:big'       => 'REAL',
    'float:normal'    => 'REAL',

    'numeric:normal'  => 'NUMERIC',

    'blob:big'        => 'BLOB',
    'blob:normal'     => 'BLOB',

    'datetime:normal' => 'TEXT',
  );
  return $map;
}


/**
 * Rename a table.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be renamed.
 * @param $new_name
 *   The new name for the table.
 */
function db_rename_table(&$ret, $table, $new_name) {
  $ret[] = update_sql('ALTER TABLE {'. $table .'} RENAME TO {'. $new_name .'}');
}


/**
 * Drop a table.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be dropped.
 */
function db_drop_table(&$ret, $table) {
  $ret[] = update_sql('DROP TABLE {'. $table .'}');
}


/**
 * Add a new field to a table.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   Name of the table to be altered.
 * @param $field
 *   Name of the field to be added.
 * @param $spec
 *   The field specification array, as taken from a schema definition.
 *   The specification may also contain the key 'initial', the newly
 *   created field will be set to the value of the key in all rows.
 *   This is most useful for creating NOT NULL columns with no default
 *   value in existing tables.
 * @param $keys_new
 *   Optional keys and indexes specification to be created on the
 *   table along with adding the field. The format is the same as a
 *   table specification but without the 'fields' element.  If you are
 *   adding a type 'serial' field, you MUST specify at least one key
 *   or index including it in this array. @see db_change_field for more
 *   explanation why.
 */
function db_add_field(&$ret, $table, $field, $spec, $keys_new = array()) {
  $fixnull = FALSE;
  if (!empty($spec['not null']) && !isset($spec['default'])) {
    $fixnull = TRUE;
    $spec['not null'] = FALSE;
  }
  $query = 'ALTER TABLE {'. $table .'} ADD ';
  $query .= _db_create_field_sql($field, _db_process_field($spec));
  if (count($keys_new)) {
    $query .= ', ADD '. implode(', ADD ', _db_create_keys_sql($keys_new));
  }
  $ret[] = update_sql($query);
  if (isset($spec['initial'])) {
    // All this because update_sql does not support %-placeholders.
    $sql = 'UPDATE {'. $table .'} SET '. $field .' = '. db_type_placeholder($spec['type']);
    $result = db_query($sql, $spec['initial']);
    $ret[] = array('success' => $result !== FALSE, 'query' => check_plain($sql .' ('. $spec['initial'] .')'));
  }
  if ($fixnull) {
    $spec['not null'] = TRUE;
    db_change_field($ret, $table, $field, $field, $spec);
  }
}


/**
 * Drop a field.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be altered.
 * @param $field
 *   The field to be dropped.
 */
function db_drop_field(&$ret, $table, $field) {
  $new_schema = _db_introspectSchema($table);
  unset($new_schema['fields'][$field]);
  foreach ($new_schema['indexes'] as $index => $fields) {
    foreach ($fields as $key => $field_name) {
      if ($field_name == $field) {
        unset($new_schema['indexes'][$index][$key]);
      }
    }
    // If this index has no more fields then remove it.
    if (empty($new_schema['indexes'][$index])) {
      unset($new_schema['indexes'][$index]);
    }
  }
  _db_alterTable($ret, $table, $new_schema);
}


/**
 * Set the default value for a field.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be altered.
 * @param $field
 *   The field to be altered.
 * @param $default
 *   Default value to be set. NULL for 'default NULL'.
 */
function db_field_set_default(&$ret, $table, $field, $default) {
  throw ("db_field_set_default Unsupported");
}


/**
 * Set a field to have no default value.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be altered.
 * @param $field
 *   The field to be altered.
 */
function db_field_set_no_default(&$ret, $table, $field) {
  throw ("db_field_set_no_default Unsupported");
}


/**
 * Add a primary key.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be altered.
 * @param $fields
 *   Fields for the primary key.
 */
function db_add_primary_key(&$ret, $table, $fields) {
  $schema = _db_introspectSchema($table);
  $schema['primary key'] = $fields;
  _db_alterTable($ret, $table, $schema);
}


/**
 * Drop the primary key.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be altered.
 */
function db_drop_primary_key(&$ret, $table) {
  $schema = _db_introspectSchema($table);
  unset($schema['primary key']);
  _db_alterTable($ret, $table, $schema);
}


/**
 * Add a unique key.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be altered.
 * @param $name
 *   The name of the key.
 * @param $fields
 *   An array of field names.
 */
function db_add_unique_key(&$ret, $table, $name, $fields) {
  $sql = _db_create_index_sql($table, $name, $fields, true);
  $ret[] = update_sql($sql);
}


/**
 * Drop a unique key.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be altered.
 * @param $name
 *   The name of the key.
 */
function db_drop_unique_key(&$ret, $table, $name) {
  $sql = _db_drop_index_sql($table, $name);
  $ret[] = update_sql($sql);
}


/**
 * Add an index.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be altered.
 * @param $name
 *   The name of the index.
 * @param $fields
 *   An array of field names.
 */
function db_add_index(&$ret, $table, $name, $fields) {
  $sql = _db_create_index_sql($table, $name, $fields, false);
  $ret[] = update_sql($sql);
}


/**
 * Drop an index.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   The table to be altered.
 * @param $name
 *   The name of the index.
 */
function db_drop_index(&$ret, $table, $name) {
  $sql = _db_drop_index_sql($table, $name);
  $ret[] = update_sql($sql);
}


/**
 * Change a field definition.
 *
 * IMPORTANT NOTE: To maintain database portability, you have to explicitly
 * recreate all indices and primary keys that are using the changed field.
 *
 * That means that you have to drop all affected keys and indexes with
 * db_drop_{primary_key,unique_key,index}() before calling db_change_field().
 * To recreate the keys and indices, pass the key definitions as the
 * optional $keys_new argument directly to db_change_field().
 *
 * For example, suppose you have:
 * @code
 * $schema['foo'] = array(
 *   'fields' => array(
 *     'bar' => array('type' => 'int', 'not null' => TRUE)
 *   ),
 *   'primary key' => array('bar')
 * );
 * @endcode
 * and you want to change foo.bar to be type serial, leaving it as the
 * primary key.  The correct sequence is:
 * @code
 * db_drop_primary_key($ret, 'foo');
 * db_change_field($ret, 'foo', 'bar', 'bar',
 *   array('type' => 'serial', 'not null' => TRUE),
 *   array('primary key' => array('bar')));
 * @endcode
 *
 * The reasons for this are due to the different database engines:
 *
 * On PostgreSQL, changing a field definition involves adding a new field
 * and dropping an old one which* causes any indices, primary keys and
 * sequences (from serial-type fields) that use the changed field to be dropped.
 *
 * On MySQL, all type 'serial' fields must be part of at least one key
 * or index as soon as they are created.  You cannot use
 * db_add_{primary_key,unique_key,index}() for this purpose because
 * the ALTER TABLE command will fail to add the column without a key
 * or index specification.  The solution is to use the optional
 * $keys_new argument to create the key or index at the same time as
 * field.
 *
 * You could use db_add_{primary_key,unique_key,index}() in all cases
 * unless you are converting a field to be type serial. You can use
 * the $keys_new argument in all cases.
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   Name of the table.
 * @param $field
 *   Name of the field to change.
 * @param $field_new
 *   New name for the field (set to the same as $field if you don't want to change the name).
 * @param $spec
 *   The field specification for the new field.
 * @param $keys_new
 *   Optional keys and indexes specification to be created on the
 *   table along with changing the field. The format is the same as a
 *   table specification but without the 'fields' element.
 */
function db_change_field(&$ret, $table, $field, $field_new, $spec, $keys_new = array()) {
  $schema = _db_introspectSchema($table);
  if(isset($schema['fields'][$field])){
    if ($field !== $field_new){
      unset($schema['fields'][$field]);
      $schema['fields'][$field_new] = $spec;
    } else {
      $schema['fields'][$field] = $spec;
    }
    _db_alterTable($ret, $table, $schema);
  }
}


/**
 * Returns the last insert id.
 *
 * @param $table
 *   The name of the table you inserted into.
 * @param $field
 *   The name of the autoincrement field.
 */
function db_last_insert_id($table, $field) {
  return db_result(db_query('SELECT last_insert_rowid()'));
}


/**
 * Create a table with a new schema containing the old content.
 *
 * As SQLite does not support ALTER TABLE (with a few exceptions) it is
 * necessary to create a new table and copy over the old content, then
 * swap them and delete the original.
 *
 * Thanks to Dmitri Schamschurko for his precious feedback and help!
 *
 * @param $ret
 *   Array to which query results will be added.
 * @param $table
 *   Name of the table to be altered.
 * @param $new_schema
 *   The new schema array for the table.
 */
function _db_alterTable(&$ret, $table, $new_schema) {

  global $active_db;

  // begin transaction
  $active_db->beginTransaction();

  // new table name
  $i = 0;
  do {
    $new_table = $table . '_' . $i++;
  } while (db_table_exists($new_table));

  // we have an unique table name, let's retrieve SQL commands
  // to create the new table.
  $create_statements = db_create_table_sql($new_table, $new_schema);

  /**
   * the array $sql_statements is filled like this:
   * [0]        BEGIN
   * [1]        CREATE TABLE ...
   * [.]        CREATE INDEX 1 ...
   * [.]        CREATE INDEX 2 ...
   * [.]        ...
   * [count-1]  COMMIT
   *
   * now we need to create the table only and not all of its indexes
   * otherwise they will be created with wrong names:
   * suppose we are altering table 'my_table' with an index on field 'nid',
   * the temporary table will be 'my_table_0' with index 'my_table_0_nid_IDX'.
   *
   * Indexes will then be recreated at the end of
   * CREATE/SWAP/DELETE procedure.
   */

  // the 2nd statement is 'CREATE TABLE', we run it alone
  $ret[] = update_sql($create_statements[1]);

  // copy the original data into the new (temp) table
  $fields = implode(', ', array_keys($new_schema['fields']));
  $ret[] = update_sql('INSERT INTO {' . $new_table . "} ($fields) SELECT $fields FROM {" . $table . '}');

  // test if table copy suceeded for all the source rows
  $old_count = db_query('SELECT COUNT(*) FROM {' . $table . '}')->fetchColumn();
  $new_count = db_query('SELECT COUNT(*) FROM {' . $new_table . '}')->fetchColumn();
  if ($old_count !== false && $old_count == $new_count) {
    // swap the two tables: original -> temp, new->original, drop temp
    do {
      $temp_table = $table . '_' . $i++;
    } while (db_table_exists($temp_table));
    db_rename_table($ret, $table, $temp_table);
    db_rename_table($ret, $new_table, $table);
    db_drop_table($ret, $temp_table);

    // recreate table indexes
    if (isset($new_schema['indexes']) && is_array($new_schema['indexes'])) {
      foreach ($new_schema['indexes'] as $key_name => $key) {
        db_add_index($ret, $table, $key_name, $key);
      }
    }
  } else {
    // ask to rollback transaction
    $ret[] = array('success' => FALSE,
                   'query' => 'ALTER TABLE: Failed to copy data.'
                  );
  }

  // test for success before committing
  foreach($ret as $i) {
    if(!$i['success']) {
      // rollback transaction
      $active_db->rollback();
      return;
    }
  }

  // commit transaction
  $active_db->commit();

}

/**
 * Find out the schema of a table.
 *
 * This function uses introspection methods provided by the database to
 * create a schema array. This is useful, for example, during update when
 * the old schema is not available.
 *
 * @param $table
 *   Name of the table.
 * @return
 *   An array representing the schema, @see drupal_get_schema.
 */
function _db_introspectSchema($table) {
  $mapped_fields = array_flip(db_type_map());
  $schema = array();
  $stmt = db_query("PRAGMA table_info('{" . $table . "}')");
  while ($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
    if (preg_match('/^([^(]+)\((.*)\)$/', $row['type'], $matches)) {
      $type = $matches[1];
      $length = $matches[2];
    }
    else {
      $type = $row['type'];
      $length = NULL;
    }
    if (isset($mapped_fields[$type])) {
      list($type, $size) = explode(':', $mapped_fields[$type]);
      $schema['fields'][$row['name']] = array(
          'type' => $type,
          'size' => $size,
          'not null' => !empty($row['notnull']),
          'default' => trim($row['dflt_value'], "'"),
      );
      if ($length) {
        $schema['fields'][$row['name']]['length'] = $length;
      }
      if ($row['pk']) {
        $schema['primary key'][] = $row['name'];
      }
    }
    else {
      new Exception("Unable to parse the column type " . $row['type']);
    }
  }

  $indexes = array();
  $stmt = db_query("PRAGMA index_list('{" . $table . "}')");
  while ($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
    if (strpos($row['name'], 'sqlite_autoindex_') !== 0) {
      $indexes[] = array(
          'schema_key' => $row['unique'] ? 'unique keys' : 'indexes',
          'name' => $row['name'],
      );
    }
  }

  $n = strlen($table) + 1;
  foreach ($indexes as $index) {
    $name = $index['name'];
    // drop out index name prefix ("TableName_") and suffix ("_IDX")
    $index_name = substr($name, $n, - strlen(SQLITE_INDEX_NAME_SUFFIX));
    $stmt = db_query("PRAGMA index_info('$name')");
    while ($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
      $schema[$index['schema_key']][$index_name][] = $row['name'];
    }
  }
  return $schema;
}



/**
 * User defined functions to replace the ones missing in SQLite.
 */
function _sqlite_concat() {
  $a = func_get_args();
  return implode('', $a);
}

function _sqlite_substring() {
  $a = func_get_args();
  return substr($a[0], $a[1] - 1, $a[2]);
}

function _sqlite_year($date) {
  return date("Y", strtotime($date));
}

function _sqlite_month($date) {
  return date("m", strtotime($date));
}

function _sqlite_from_unix_timestamp($timestamp) {
  //YYYY-MM-DD HH:MM:SS
  return date("Y-m-d G:i:s", $timestamp);
}

function _sqlite_pow($num, $exp) {
  return pow($num, $exp);
}

function _sqlite_greatest() {
  $a = func_get_args();
  return max($a);
}

/* COALESCE (MySQL) */
function _sqlite_coalesce_step(&$context, $rownumber, $value) {
  if($value !== null)
  return $value;
  return null;
}
function _sqlite_coalesce_finalize(&$context, $rownumber) {
  return $context;
}

/* Standard deviation STDDEV (MySQL) */
function _sqlite_stddev_step(&$context, $rownumber, $value) {
  if (!$context)
    return array(
                'mean' => (float) $value,
                'quad' => (float) $value * (float) $value,
                'cnt' => 1,
                );

  $context['mean'] += (float) $value;
  $context['quad'] += (float) $value * (float) $value;
  $context['cnt']  += 1;

  return $context;
}

function _sqlite_stddev_finalize(&$context, $rownumber) {
  if ($context['cnt'])
  {
    return sqrt(($context['quad'] - $context['mean'] * $context['mean'] / (float) $context['cnt']) / (float) $context['cnt']);
  }

  return 0.0;
}


function _sqlite_rand() {
  return rand();
}

function _sqlite_if($expr, $true_part, $false_part) {
  if ($expr)
  return ($true_part);
  else
  return ($false_part);
}

function _sqlite_md5($text) {
  return md5($text);
}

function _sqlite_dateformat($date, $format) {
  if($date) {
    list($year, $month, $day) = explode('-', $date) ;
    return(strftime($format,mktime(0,0,0,(int)$month,(int)$day,(int)$year))) ;
  } else {
    return null;
  }
}