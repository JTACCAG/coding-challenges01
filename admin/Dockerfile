# ---------- Build stage ----------
FROM node:20-alpine AS build

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build --prod

# ---------- Runtime stage ----------
FROM nginx:alpine

# Elimina configuración por defecto
RUN rm /etc/nginx/conf.d/default.conf

# Copia configuración personalizada (opcional)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copia el build de Angular
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
