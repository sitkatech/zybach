FROM node:12.18-alpine

ENV NODE_ENV production
WORKDIR /usr/src/app
COPY ["package.json", "package-lock.json*", "npm-shrinkwrap.json*", "./"]
RUN npm install --production --silent && mv node_modules ../
COPY . .

RUN echo "0       1       *       *       *       /usr/src/app/teaspoon-cron" >> /etc/crontabs/root

RUN apk add dos2unix
RUN dos2unix ./teaspoon-cron

RUN chmod a+rx ./teaspoon-cron

CMD ["crond","-f", "-l", "8"]