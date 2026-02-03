# build
FROM node:20 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build -- --configuration production


# nginx
FROM nginx:alpine
COPY --from=build /app/dist/logistics-platform /usr/share/nginx/html

EXPOSE 80
