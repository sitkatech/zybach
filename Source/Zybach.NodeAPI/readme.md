# Developer tools
* VS Code
* Yarn package manager
* * We're doing this instead of npm for this one. It works almost exactly the same as npm with a few minor differences in CLI, and uses the npm package registry as its source for packages, but is slightly more modern.
* * You can install it with `npm install -g yarn`
# Make it run
* As always, copy `secrets.template.json` to `secrets.json` and fill it out.
* Currently, there is no `.env` file.
* Open this directory in VS Code. There is no workspace file.
* Basically, all you have to do is `docker-compose -f docker-compose.debug.yml up -d`
* * In VS Code, you can do `CTRL + SHIFT + P` to open the command palette, select "Docker: Compose Up", and then select the `docker-compose.debug.yml` file.

# Develop it
* Do a `yarn install` to get all the dependencies.
* * Even though we're running in a docker container, this is necessary to build your code with `tsc` and for VS Code intellisense to be useful.
* * This is also necessary if you want to use `nodemon` to regenerate routes when you make changes to controllers. (See below for details.) So just do it.
* Code goes in the `src` directory. Code should be `*.ts` files.
* Make sure your TypeScript builds without errors by running `yarn run build`
* * This command runs `tsoa` to re-generate routes from controllers, then `tsc` to compile TypeScript to JavaScript
* * Because we are using `tsoa`, there is **no need to write routing code**. 

# Debug it
* The debug launch task is already configured for VS Code. Just hit `F5` after running the `docker-compose` command above.
* The `docker-compose.debug.yml` file runs `node -r ts-node/register` in the container, launching the application directly from the TypeScript files. This, together with the `"sourceMap": true` option in the `src/.tsconfig` file and the `sourceMapPathOverrides` option in the `launch.json` file will allow the VS Code debug client to hook into breakpoints that you set in the `*.ts` files, so you don't need to worry about the compiled code when debugging.
* In fact, the `docker-compose.debug.yml` file is executing this with `nodemon`, and has the `src` directory mounted to the container. Your changes will therefore take effect immediately, with no need to restart the container.
* * Note that you will need to restart the container if you add dependencies with `yarn`, since those will need to be installed.
* Additionally, there is a script in `package.json` to run `nodemon` on your host to automatically regenerate routes when you make changes to controllers. IFf you want to use this script, run `yarn run autogen-routes` on your host machine.

# Todos
* Eventually we'll want to use a multistage build for QA/Production to build the TypeScript in one container and copy the built files to a separate container for slimmer images.
* We might also consider using a separate Dockerfile for development/debugging that only installs modules and copies TypeScript, without building.