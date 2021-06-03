FROM node:current-alpine3.10
WORKDIR /
COPY ["package.json", "yarn.lock*", "npm-shrinkwrap.json*", "./"]
# will need to use multi-stage builds so that we can install dev deps for compiling and only prod deps for running...
RUN yarn install --silent && mv node_modules ../
COPY . .
RUN yarn run build

RUN echo "0       2       *       *       *       /cron.sh" >> /etc/crontabs/root

RUN apk add dos2unix
RUN dos2unix ./cron.sh

RUN chmod a+rx ./cron.sh

CMD ["crond","-f", "-l", "8"]