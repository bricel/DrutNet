<?php
/**
 * ======================================================================
 * This file contains core query rewrite rules, applied to queries just
 * before their execution.
 *
 * If you want to add custom patches for your additional modules,
 * DO NOT ADD THEM HERE, CUSTOM PATCHES this file will be overwritten when
 * you install a newer Drupal-SQLite release.
 *
 * Add your rewrite rules into a file named "database.sqlite.user-patches.inc"
 * (see "database.sqlite.user-patches.sample" for an example).
 * Your patches will be applied AFTER the core ones.
 * ======================================================================
 */
function _sqlite_core_rewrite_query(&$query)
{

  /**
   * --------------------------------------------------
   * Module: actions (core)
   * --------------------------------------------------
   * Patch for "INSERT INTO" with DEFAULT values.
   * Will change:
   * INSERT INTO {action_aid} VALUES (default)
   * to:
   * INSERT INTO {action_aid} DEFAULT VALUES
   */
  if ((substr($query, 0, 12) === 'INSERT INTO ') && stripos($query, ' VALUES (default)') > 0){
    $query = str_ireplace(' VALUES (default)', ' DEFAULT VALUES', $query);
    return;
  }


  /**
   * --------------------------------------------------
   * Module: comments (core)
   * --------------------------------------------------
   * comments module uses a query+subquery to sort comments.
   * Will change:
   * (SELECT thread FROM comments...
   * to:
   * SELECT * FROM (SELECT thread FROM comments...
   */
  if (substr($query, 0, 28) === '(SELECT thread FROM comments') {
    $query = 'SELECT * FROM ' . $query;
    return;
  }


  /**
   * --------------------------------------------------
   * Module: devel (not a core module but largely used)
   * --------------------------------------------------
   * devel module uses an extension of MySQL to insert several rows at once.
   * Will change:
   * INSERT INTO [table] VALUES (<values 1>),(<values 2>),(<values 3>)
   * to:
   * INSERT INTO [table] SELECT <values 1> UNION ALL <values 2> UNION ALL <values 3>
   */
  if(variable_get('devel_store_queries', 0)) {
    $devel_workaround_string = 'INSERT INTO devel_times (qid, time) VALUES (';
    $devel_workaround_length = strlen($devel_workaround_string);
    if (substr($query, 0, $devel_workaround_length) == $devel_workaround_string)
    {
      $query = 'INSERT INTO devel_times (qid, time) SELECT '
             . str_replace('),(', ' UNION ALL SELECT ', substr($query, $devel_workaround_length, -1));
      return;
    }
  }

  /**
   * --------------------------------------------------
   * Module: cache and update (core)
   * --------------------------------------------------
   * These two modules, starting from 6.15, use the "TRUNCATE TABLE {table}" SQL command
   * instead of the old "DELETE FROM {table}".
   * SQLite does not support TRUNCATE, so will change it back to DELETE FROM.
   * Will change:
   * TRUNCATE TABLE
   * to:
   * DELETE FROM
   */
  if (substr($query, 0, 15) === 'TRUNCATE TABLE ') {
    $query = str_replace('TRUNCATE TABLE ', 'DELETE FROM ', $query);
    return;
  }

/**
 * end of the function
 */
}
