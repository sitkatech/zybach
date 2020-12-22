FROM node:current-alpine3.10
WORKDIR /
COPY ["package.json", "yarn.lock*", "npm-shrinkwrap.json*", "./"]
RUN yarn install --production --silent && mv node_modules ../
COPY . .
EXPOSE 80
CMD ["yarn", "start"]
