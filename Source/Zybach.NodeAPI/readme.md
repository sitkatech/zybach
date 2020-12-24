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
* Code goes in the `src` directory. Code should be `*.ts` files.
* Make sure your TypeScript builds without errors by running `yarn run build` or `yarn run tsc -p src` (the former being an alias for the latter set in `package.json`)
# Debug it
* The debug launch task is already configured for VS Code. Just hit `F5` after running the `docker-compose` command above.
* The `docker-compose.debug.yml` file runs `node -r ts-node/register` in the container, launching the application directly from the TypeScript files. This, together with the `"sourceMap": true` option in the `src/.tsconfig` file and the `sourceMapPathOverrides` option in the `launch.json` file will allow the VS Code debug client to hook into breakpoints that you set in the `*.ts` files, so you don't need to worry about the compiled code when debugging.
# Todos
* Eventually we'll want to use a multistage build for QA/Production to build the TypeScript in one container and copy the built files to a separate container for slimmer images.
* We might also consider using a separate Dockerfile for development/debugging that only installs modules and copies TypeScript, without building.