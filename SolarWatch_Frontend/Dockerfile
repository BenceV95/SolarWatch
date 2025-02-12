# Use Node.js as the base image
FROM node:18 AS build
WORKDIR /app

# Install dependencies
COPY package.json package-lock.json ./
RUN npm install

# Copy all source files and build the app
COPY . .
RUN npm run build

# Use Nginx to serve the built frontend
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html

# Expose the port
EXPOSE 3000
CMD ["nginx", "-g", "daemon off;"]
