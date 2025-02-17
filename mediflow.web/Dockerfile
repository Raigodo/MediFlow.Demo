# Stage 1: Build Stage
FROM node:23-alpine3.20 AS build

WORKDIR /app

ENV PORT=3000
ENV HOST=0.0.0.0

COPY package.json . 
RUN corepack enable pnpm || npm install -g pnpm
RUN pnpm i

COPY . .

RUN pnpm build

# Stage 2: Production Stage
FROM build

WORKDIR /app

COPY --from=build /app . 

RUN pnpm install --prod

CMD ["pnpm", "start"]