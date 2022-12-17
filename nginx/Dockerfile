# build frontend
FROM node:lts-alpine AS build-stage
WORKDIR /app
COPY ./frontend/package*.json ./
RUN npm config set legacy-peer-deps=true --location=project
RUN npm install
COPY ./frontend .
RUN npm run build

# run nginx
FROM nginx AS prod-stage
COPY ./nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=build-stage /app/dist /var/www/html