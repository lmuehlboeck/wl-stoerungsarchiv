# build frontend
FROM node:lts-alpine AS build-stage
WORKDIR /app
COPY ./wls-frontend/package*.json ./
RUN npm config set legacy-peer-deps=true --location=project
RUN npm install
COPY ./wls-frontend .
RUN npm run build

# run nginx
FROM nginx AS prod-stage
COPY ./wls-proxy/nginx.conf /etc/nginx/nginx.conf
COPY --from=build-stage /app/dist /var/www/html
