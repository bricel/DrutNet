<?php

/**
 * Helper to implementation of hook_content_default_fields().
 */
function _drutnet_sample_content_default_fields() {
  $fields = array();

  // Exported field: field_file
  $fields[] = array(
    'field_name' => 'field_file',
    'type_name' => 'drutnet_sample',
    'display_settings' => array(
      'label' => array(
        'format' => 'above',
        'exclude' => 0,
      ),
      'teaser' => array(
        'format' => 'default',
        'exclude' => 0,
      ),
      'full' => array(
        'format' => 'default',
        'exclude' => 0,
      ),
      '4' => array(
        'format' => 'default',
        'exclude' => 0,
      ),
      '2' => array(
        'format' => 'default',
        'exclude' => 0,
      ),
      '3' => array(
        'format' => 'default',
        'exclude' => 0,
      ),
    ),
    'widget_active' => '1',
    'type' => 'filefield',
    'required' => '0',
    'multiple' => '1',
    'module' => 'filefield',
    'active' => '1',
    'list_field' => '0',
    'list_default' => 1,
    'description_field' => '0',
    'widget' => array(
      'file_extensions' => 'txt',
      'file_path' => '',
      'progress_indicator' => 'bar',
      'max_filesize_per_file' => '',
      'max_filesize_per_node' => '',
      'label' => 'file',
      'weight' => '32',
      'description' => '',
      'type' => 'filefield_widget',
      'module' => 'filefield',
    ),
  );

  // Translatables
  array(
    t('file'),
  );

  return $fields;
}

/**
 * Helper to implementation of hook_user_default_permissions().
 */
function _drutnet_sample_user_default_permissions() {
  $permissions = array();

  // Exported permission: access all views
  $permissions[] = array(
    'name' => 'access all views',
    'roles' => array(),
  );

  // Exported permission: administer search service
  $permissions[] = array(
    'name' => 'administer search service',
    'roles' => array(),
  );

  // Exported permission: administer taxonomy from remote
  $permissions[] = array(
    'name' => 'administer taxonomy from remote',
    'roles' => array(),
  );

  // Exported permission: change own username
  $permissions[] = array(
    'name' => 'change own username',
    'roles' => array(),
  );

  // Exported permission: check module exists from remote
  $permissions[] = array(
    'name' => 'check module exists from remote',
    'roles' => array(),
  );

  // Exported permission: clear cache from remote
  $permissions[] = array(
    'name' => 'clear cache from remote',
    'roles' => array(),
  );

  // Exported permission: create new users
  $permissions[] = array(
    'name' => 'create new users',
    'roles' => array(),
  );

  // Exported permission: delete any user
  $permissions[] = array(
    'name' => 'delete any user',
    'roles' => array(),
  );

  // Exported permission: delete own user
  $permissions[] = array(
    'name' => 'delete own user',
    'roles' => array(),
  );

  // Exported permission: get any binary files
  $permissions[] = array(
    'name' => 'get any binary files',
    'roles' => array(
      '0' => 'authenticated user',
    ),
  );

  // Exported permission: get any user data
  $permissions[] = array(
    'name' => 'get any user data',
    'roles' => array(),
  );

  // Exported permission: get available services
  $permissions[] = array(
    'name' => 'get available services',
    'roles' => array(),
  );

  // Exported permission: get own binary files
  $permissions[] = array(
    'name' => 'get own binary files',
    'roles' => array(),
  );

  // Exported permission: get own user data
  $permissions[] = array(
    'name' => 'get own user data',
    'roles' => array(),
  );

  // Exported permission: get variable from remote
  $permissions[] = array(
    'name' => 'get variable from remote',
    'roles' => array(),
  );

  // Exported permission: load node data
  $permissions[] = array(
    'name' => 'load node data',
    'roles' => array(
      '0' => 'authenticated user',
    ),
  );

  // Exported permission: log a system message from remote
  $permissions[] = array(
    'name' => 'log a system message from remote',
    'roles' => array(),
  );

  // Exported permission: save file information
  $permissions[] = array(
    'name' => 'save file information',
    'roles' => array(
      '0' => 'authenticated user',
    ),
  );

  // Exported permission: set variable from remote
  $permissions[] = array(
    'name' => 'set variable from remote',
    'roles' => array(),
  );

  // Exported permission: update any user data
  $permissions[] = array(
    'name' => 'update any user data',
    'roles' => array(),
  );

  // Exported permission: update own user data
  $permissions[] = array(
    'name' => 'update own user data',
    'roles' => array(),
  );

  // Exported permission: upload files via file form
  $permissions[] = array(
    'name' => 'upload files via file form',
    'roles' => array(
      '0' => 'authenticated user',
    ),
  );

  return $permissions;
}
