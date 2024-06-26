# Zybach Fresh Env Setup
## Required
1. Clone repo, make sure it lives under C:/git/sitkatech
2. Grab all the config files from another dev's machine.
	- .env (docker-compose directory)
        - Update SITKA_EMAIL_REDIRECT to your ESA email to avoid spamming your coworker.
	- appsecrets (API directory)
	- config (Zybach.Web\src\assets\config)
	- secrets (Build directory)
    - GeoserverAdminPassword (C:\Sitka\Zybach\Geoserver)
    - GeoserverSqlServerPassword (C:\Sitka\Zybach\Geoserver)
3. Ensure you have .NET 8 SDK.
4. Ensure you have a SQL user 'DockerWebUser' with sysadmin role and password of 'password#1'.
5. Run DownloadRestoreBuild to get database set up.
6. Ensure you have NVM installed and it is set to use node 18.
7. Run npm install to grab all front end dependencies. 
8. Run npm run gen-model to make sure there isn't an issue there. Mikey ran into an issue where the project broke if generated with the latest generator. To fix that issue:
	- npm install @openapitools/openapi-generator-cli -g
	- openapi-generator-cli version-manager set 5.3.0
9. Point to QA keystone until there is a good reason to get it working locally. Replace local keystone URLS in C:\git\sitkatech\qanat\Qanat.Web\src\assets\config with https://identity-qa.sitkatech.com
10. Should be able to log into the app after pressing the go buttons.

## Optional/Recommended
1. Start gitflow process in SourceTree after checking out main/develop locally.