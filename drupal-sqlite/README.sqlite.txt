// $Id:

WHAT IS DRUPAL-SQLITE ?
=======================
Drupal-SQLite is a project to make Drupal 6 work with a SQLite database.

SQLite is a lightweight, fast, public domain, easily integrable,
multiplatform database system. It's used by many greatest
open source projects; just to name one... Mozilla Foudation on
their Firefox & Thunderbird.

Why should I need this?
Well, most hosting providers gives you PHP 5.2+ for free,
but they ask a few bucks for a MySQL database.
If this could worth the case for mid-large sites, with hundreds or
thousands users a day, it's a waste for mid-small sites.


REQUIREMENTS
============
Drupal-SQLite requires PHP 5.2+ with PDO-SQLite support enabled.
WARNING: PDO-SQLite is NOT the standard SQLite support (non-PDO) and
should be built into your PHP or (usually) provided by an external
extension named php_pdo_sqlite.dll (win) or php_pdo_sqlite.so (linux).

To test if PDO_SQLite support is enabled create a text file,
name it phpinfo.php and insert a single line:

<?php phpinfo(); ?>

Now open this file in your browser and check if a section
named "pdo_sqlite" exists and is enabled.

NOTE to Windows XAMPP users:
XAMPP 1.7.0 comes with pdo_sqlite disabled by default.
To enable it:
* open C:\xampp\apache\bin\php.ini file
* find the row containing
  ;extension=php_pdo_sqlite.dll
* remove the leading ";" to enable PDO-SQLite
* restart Apache


HOW TO INSTALL A NEW WEBSITE WITH SQLITE SUPPORT
================================================
Drupal-SQLite comes in two flavours:

* Full archive
  This should be the best solution for Windows based hosting,
  but it surely works on Linux too.
  Download the archive and extract it in your web server root folder.
   
* Patch file
  Since applying patches on Windows is not so easy,
  I suggest this to Linux users only.
  Download patch file of the original Drupal version you're going to patch
  and apply it to Drupal tree you already extracted to your web server
  root folder. If the patch applied successfully then go on.
	  
In both cases you'll end with a patched Drupal source tree.

Now you're ready to start installation: open /install.php in your browser
and follow the instructions. Be careful to:

* choose the "Drupal-SQLite" profile at the very first page to have
  an optimized configuration.
  This will disable time-consuming modules like, syslog and dblog.

* select "sqlite" as database type inside the "basic options" block
  (it should be the first available)

* when asked for a database name, remember that this will be the
  full (relative) path to your database file and not only the db name,
  as it happens with mysql.
  Default value is: sites/default/db/db.s3db

* username is not used at all, but in Drupal installer this is a required
  field; this is why it will be pre-filled with "coolsoft".


SECURITY NOTES
==============
Your database is now a single file (by default sites/default/db/*.s3db),
and you should protect it from being downloaded.
Drupal-SQLite will create a preconfigured .htaccess files to deny access
to *.s3db files. This file will be created by the setup procedure into
the same folder which will contain the DB file.

Please edit it if you changed proposed database file name, then test if it works.


UPGRADE FROM PREVIOUS VERSIONS
==============================
* Full archive version
  Download the archive of the new version and extract it into the
  root folder of your Drupal setup, overwriting existing files.
  Your website content will be preserved.

* Patch file version
  Update Drupal as you will do with a standard Drupal setup, then apply
  the corresponding patch.


ADDITIONAL ACTION AFTER UPGRADING FROM Drupal-SQLite-6.10-1.0 
=============================================================
If you are upgrading from Drupal-SQLite 1.0, please note that for security
reasons, database file is now contained in a reserved subfolder.
(see: http://coolsoft.altervista.org/en/drupal-sqlite#comment-40)

If you previously let Drupal-SQLite 1.0 create the file in "sites/default/db.s3db",
you should move it in a reserved folder like "sites/default/db/db.s3db".
This folder must be read/writable by the webserver user.

After doing this, open your config file (sites/default/settings.php), search
a line like this (around line 92):
  $db_url = 'sqlite://coolsoft@localhost/sites%2Fdefault%2Fdb.s3db';
and edit it to make it link to the new DB location, like
  $db_url = 'sqlite://coolsoft@localhost/sites%2Fdefault%2Fdb%2Fdb.s3db';

NOTE: path separators "/" must be encoded as %2F.


ACKNOWLEDGEMENTS
================
- Drupal team
  Their CMS is great, even if it misses SQLite support ;)

- Roberto Capuzzo (http://interdict.altervista.org)
  He wrote a great italian tutorial on installing Drupal-SQLite on
  Altervista WebHosting. The tutorial is here:
  http://coolsoft.altervista.org/it/drupal-sqlite#ack
  
- Dmitri Schamschurko
  He gave me a great feedback and help debugging SQLite
  table schema functions in database.sqlite.inc. 
  This lead to CCK module support in Drupal-SQLite-6.13-1.2


CHANGELOG
=========

Drupal-SQLite-6.15-1.5, 2010-01-07
----------------------------------
- Drupal official version 6.15

- Optimized query rewriting by adding shortcut returns after
  a successful rewrite.

- New core rewrite rule for cache and update modules ("TRUNCATE TABLE" SQL commands)

- Fixed an error during setup that causes install.php not using
  the values of "Site Name" and "Administrator username" fields.
  The default values were used instead.

  
Drupal-SQLite-6.14-1.4, 2009-10-04
----------------------------------
- Drupal official version 6.14

- New query rewriting system: it allows Drupal-SQLite to rewrite
  SQL queries just before their execution, without the needing to
  patch (core) modules.
  Rewrite rules are contained into two new files:
  
  * database.sqlite.core-patches.inc
    this file is maintained by CoolSoft and will include
    rewrite rules for core modules and for widely used ones ;) (like "devel")
	
  * database.sqlite.user-patches.inc (optional)
    here you could add rewrite rules for all other modules
  
- Drupal DB functions "db_add_unique_key" and "db_remove_unique_key"
  are now supported.

- Workaround for SQLite not returning short column names for queries
  with JOIN or GROUP BY clauses.
  
- Added support for SQL function STDDEV (thanks again Dmitri)


Drupal-SQLite-6.13-1.3, 2009-08-06
----------------------------------
- Fixed a bug in _db_query function which causes multiple/nested queries to fail.

  
Drupal-SQLite-6.13-1.2, 2009-07-15
----------------------------------
- Bug fixing and code cleanup in SQLite table schema management
  functions: db_column_exists, db_create_table_sql, _db_create_index_sql,
  _db_alterTable, _db_introspectSchema.
  Thanks to Dmitri Schamschurko for his great help and feedback.
  
- CCK module (cck-6.4-2.4) now works with Drupal-SQLite.


Drupal-SQLite-6.13-1.1, 2009-07-02
----------------------------------
- Drupal official version 6.13


Drupal-SQLite-6.12-1.1, 2009-05-14
----------------------------------
- Drupal official version 6.12
  
  
Drupal-SQLite-6.11-1.1.1, 2009-05-09
------------------------------------
- This is only a repackage of the previous archive version, due to missing
  folders in drupal-sqlite-6.11-1.1.zip file. Missing folders were:
   /modules/color/images
   /sites
   /themes/garland/images
	
  I'm sorry for that <:) 


Drupal-SQLite-6.11-1.1, 2009-05-05
----------------------------------
- Drupal official version 6.11

- Increased length of database name field (which here is used as a path)

- Moved new 'sqlite' database type as first in install.inc, so it
  becomes the default selected during install
  
- The profile Drupal-SQLite now checks requirements during the install.
  see: http://coolsoft.altervista.org/en/drupal-sqlite#comment-28
  
- Database file is now, by default, contained in a reserved folder.
  SQLite needs RW access to both the database file and the containing folder.
  see: http://coolsoft.altervista.org/en/drupal-sqlite#comment-40

- Install procedure will now create both the database and its folder,
  and test their permissions.
  see: http://coolsoft.altervista.org/en/drupal-sqlite#comment-40
  
- Fixed usage of undeclared variables in includes/database.sqlite.inc
  see: http://coolsoft.altervista.org/en/drupal-sqlite#comment-37


Drupal-SQLite-6.10-1.0, 2009-03-21
----------------------------------
- Drupal official version 6.10

- First public release
