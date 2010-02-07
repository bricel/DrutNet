<?php
// $Id:

/**
 * Installation profile for Drupal-SQLite
 *
 * This file is part of Drupal-SQLite by CoolSoft,
 * a patched Drupal 6 version that works with a SQLite database layer.
 *
 * @author Claudio Nicora (coolsoft@altervista.org)
 * @see http://coolsoft.altervista.org/drupal-sqlite
 */

/**
 * Return an array of the modules to be enabled when this profile is installed.
 *
 * @return
 *   An array of modules to enable.
 */
function drupal_sqlite_profile_modules() {
  // time-consuming modules (dblog and syslog) are not enabled
  return array('color', 'comment', 'help', 'menu', 'taxonomy');
}

/**
 * Return a description of the profile for the initial installation screen.
 *
 * @return
 *   An array with keys 'name' and 'description' describing this profile,
 *   and optional 'language' to override the language selection for
 *   language-specific profiles.
 */
function drupal_sqlite_profile_details() {

  // default description
  $desc = 'Select this profile to install Drupal with a SQLite database backend '
      . 'and optimize it by removing all time-consuming modules (like dblog).';

  // test requirements
  $requirements = drupal_sqlite_profile_test_requirements();

  // count failed requirements
  $failed = 0;
  foreach($requirements as $req => $value) {
    if (!$value['ok']) $failed +=1;
  }

  if($failed) {
    $desc .= <<< EOF
<br/><br/><div class="error">WARNING:
Drupal-SQLite won't work because one of the following requirements
is not satisfied; please fix it and refresh this page to retry.
</div>
EOF;
    $desc .= '<ul>';
    foreach($requirements as $req => $value) {
      $desc .= '<li><img src="misc/'
        . ($value['ok'] ? 'watchdog-ok.png' : 'watchdog-error.png')
        . '">&nbsp;'
        . $value['description'].'</li>';
    }
    $desc .= '</ul>';
  } else {
    $desc .= '<br/>In the next screen <b><u>choose "sqlite" as database type</b></u> and fill the required fields.';
  }

  return array(
    'name' => 'Drupal-SQLite',
    'description' => $desc,
    );
}

/**
 * Return a list of tasks that this profile supports.
 *
 * @return
 *   A keyed array of tasks the profile will perform during
 *   the final stage. The keys of the array will be used internally,
 *   while the values will be displayed to the user in the installer
 *   task list.
 */
function drupal_sqlite_profile_task_list() {
}

/**
 * Perform any final installation tasks for this profile.
 *
 * The installer goes through the profile-select -> locale-select
 * -> requirements -> database -> profile-install-batch
 * -> locale-initial-batch -> configure -> locale-remaining-batch
 * -> finished -> done tasks, in this order, if you don't implement
 * this function in your profile.
 *
 * If this function is implemented, you can have any number of
 * custom tasks to perform after 'configure', implementing a state
 * machine here to walk the user through those tasks. First time,
 * this function gets called with $task set to 'profile', and you
 * can advance to further tasks by setting $task to your tasks'
 * identifiers, used as array keys in the hook_profile_task_list()
 * above. You must avoid the reserved tasks listed in
 * install_reserved_tasks(). If you implement your custom tasks,
 * this function will get called in every HTTP request (for form
 * processing, printing your information screens and so on) until
 * you advance to the 'profile-finished' task, with which you
 * hand control back to the installer. Each custom page you
 * return needs to provide a way to continue, such as a form
 * submission or a link. You should also set custom page titles.
 *
 * You should define the list of custom tasks you implement by
 * returning an array of them in hook_profile_task_list(), as these
 * show up in the list of tasks on the installer user interface.
 *
 * Remember that the user will be able to reload the pages multiple
 * times, so you might want to use variable_set() and variable_get()
 * to remember your data and control further processing, if $task
 * is insufficient. Should a profile want to display a form here,
 * it can; the form should set '#redirect' to FALSE, and rely on
 * an action in the submit handler, such as variable_set(), to
 * detect submission and proceed to further tasks. See the configuration
 * form handling code in install_tasks() for an example.
 *
 * Important: Any temporary variables should be removed using
 * variable_del() before advancing to the 'profile-finished' phase.
 *
 * @param $task
 *   The current $task of the install system. When hook_profile_tasks()
 *   is first called, this is 'profile'.
 * @param $url
 *   Complete URL to be used for a link or form action on a custom page,
 *   if providing any, to allow the user to proceed with the installation.
 *
 * @return
 *   An optional HTML string to display to the user. Only used if you
 *   modify the $task, otherwise discarded.
 */
function drupal_sqlite_profile_tasks(&$task, $url) {

  // Insert default user-defined node types into the database. For a complete
  // list of available node type attributes, refer to the node type API
  // documentation at: http://api.drupal.org/api/HEAD/function/hook_node_info.
  $types = array(
  array(
      'type' => 'page',
      'name' => st('Page'),
      'module' => 'node',
      'description' => st("A <em>page</em>, similar in form to a <em>story</em>, is a simple method for creating and displaying information that rarely changes, such as an \"About us\" section of a website. By default, a <em>page</em> entry does not allow visitor comments and is not featured on the site's initial home page."),
      'custom' => TRUE,
      'modified' => TRUE,
      'locked' => FALSE,
      'help' => '',
      'min_word_count' => '',
    ),
    array(
      'type' => 'story',
      'name' => st('Story'),
      'module' => 'node',
      'description' => st("A <em>story</em>, similar in form to a <em>page</em>, is ideal for creating and displaying content that informs or engages website visitors. Press releases, site announcements, and informal blog-like entries may all be created with a <em>story</em> entry. By default, a <em>story</em> entry is automatically featured on the site's initial home page, and provides the ability to post comments."),
      'custom' => TRUE,
      'modified' => TRUE,
      'locked' => FALSE,
      'help' => '',
      'min_word_count' => '',
    ),
  );

  foreach ($types as $type) {
    $type = (object) _node_type_set_defaults($type);
    node_type_save($type);
  }

  // Default page to not be promoted and have comments disabled.
  variable_set('node_options_page', array('status'));
  variable_set('comment_page', COMMENT_NODE_DISABLED);

  // Don't display date and author information for page nodes by default.
  $theme_settings = variable_get('theme_settings', array());
  $theme_settings['toggle_node_info_page'] = FALSE;
  variable_set('theme_settings', $theme_settings);

  // Update the menu router information.
  menu_rebuild();
}

/**
 * Implementation of hook_form_alter().
 *
 * Allows the profile to alter the site-configuration form. This is
 * called through custom invocation, so $form_state is not populated.
 */
function drupal_sqlite_form_alter(&$form, $form_state, $form_id) {

  if ($form_id == 'install_configure') {

    // Add some default values
    $form['site_information']['site_name']['#default_value'] = 'Drupal-SQLite';
    $form['admin_account']['account']['name']['#default_value'] = 'administrator';

  }

}


/**
 * Test installation requirements
 * @return array
 */
function drupal_sqlite_profile_test_requirements() {

  $res = array();

  // PHP version (different from Drupal)
  $min_php_ver = "5.2.0";
  $res['sqlite-php-version'] = array(
    'description' => "Minimum PHP version: required $min_php_ver, found: ".phpversion(),
    'ok'   => (version_compare(phpversion(), $min_php_ver) >= 0),
  );

  // Test SQLite patch
  $res['sqlite-patch'] = array(
    'description' => 'Patch to run Drupal on a SQLite database',
    'ok'   => file_exists('includes/database.sqlite.inc'),
  );

  // test for PDO_SQLite support
  $res['sqlite-support'] = array(
    'description' => 'PHP PDO_SQLite support',
    'ok'   => class_exists('PDO') && extension_loaded('pdo_sqlite'),
  );

  return $res;
}