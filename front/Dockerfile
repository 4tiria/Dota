FROM node:18-alpine as dev

WORKDIR /app

COPY package*.json ./

RUN npm i

COPY ./src ./src
COPY ./public ./public
COPY ./tsconfig.json ./tsconfig.json

RUN npm run build

FROM node:18-alpine

RUN npm install -g serve

EXPOSE 3000

CMD ["serve", "-s", "build", "-l", "3000", "--single"]