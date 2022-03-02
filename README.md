# Developer Set-up Instructions
## Prerequisites

1. Install Visual Studio 2019
2. Install Visual Studio Code
3. Install Docker
4. Install Node.js
5. Install these extensions for Visual Studio Code:

- Angular CLI from Balram Chavan
- Debugger for Chrome
- Docker
- npm support for VS Code from egamma
- npm commands for VS Code from Florian Knop
- npm Intellisense from Christian Kohler

## Zybach Set-up (API)

1. Clone the git repository to your development machine
2. Create an empty database called ZybachDB and create a user for it. Give the user the owner role.
3. Copy [repo root dir]\Source\docker-compose\.env.template to [repo root dir]\Source\docker-compose\.env
4. Update the values in the new .env file
5. Add an entry to your hosts file pointing ZYBACH_WEB_URL from the .env file to 127.0.0.1
6. Open the solution in VS19 and set docker-compose as the startup project
7. Press the green "play" triangle to start the API server

## Zybach Set-up (Web)
1. Open the zybach-web-workspace in VSC ([repo root dir]\Source\Zybach.Web)
2. Open the VSC terminal and run npm install, then npm build, then npm start.
3. Press F5 to open the web app in Google Chrome. You will be able to debug JavaScript directly in VSC.
