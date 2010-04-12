<?php

/**
 * Implementation of hook_content_default_fields().
 */
function drutnet_sample_content_default_fields() {
  module_load_include('inc', 'drutnet_sample', 'drutnet_sample.defaults');
  $args = func_get_args();
  return call_user_func_array('_drutnet_sample_content_default_fields', $args);
}

/**
 * Implementation of hook_node_info().
 */
function drutnet_sample_node_info() {
  module_load_include('inc', 'drutnet_sample', 'drutnet_sample.features.node');
  $args = func_get_args();
  return call_user_func_array('_drutnet_sample_node_info', $args);
}

/**
 * Implementation of hook_user_default_permissions().
 */
function drutnet_sample_user_default_permissions() {
  module_load_include('inc', 'drutnet_sample', 'drutnet_sample.defaults');
  $args = func_get_args();
  return call_user_func_array('_drutnet_sample_user_default_permissions', $args);
}
