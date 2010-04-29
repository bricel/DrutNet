
Folders :
/Class - Source code of API classes.
/DrutNETSample - A sample API client.
/Dlls - All libraries dependent.
/Forms - Include a windows form for user login.
/Properties - Project properties
/Drupal Modules - Drupal modules (see Sample Installation instructions for more info)

Creating a client with C#:

Dependencies : all libraries required as dependencies are located in folder /Dlls	

- To connect with service use the Class Services, here is a brief overview of the function :
	Service.Login - Login to drupal
	Service.NodeGet - Load a Node
	Service.UserGet - Load a user
	Service.NodeSave - Save a node
	
- To Upload files using the API the the Class CURL Services, here is a brief overview of the function :
	Curl.Login - Login to Drupal with CURL
	Curl.UploadFile - Upload a file to a CCK file/image field (require the file_form module provide in the '/Drupal Module' folder)
	
- Permissions - Make sure to grand permissions to all services modules that you wish to use.
	


Sample installation instructions:

1. Place the 2 modules under '/Drupal Module' in you 'sites/all/modules'.
	- Drupal Example - The module is a features module to test the system with the DrutNETSample
	- file_form - this Module is required only for file upload with CURL
2. Download the modules :  Services, Features, CCK ,VIEW.
3. Enable all the modules above




