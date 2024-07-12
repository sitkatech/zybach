FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

SHELL ["/bin/bash", "-o", "pipefail", "-c"]
COPY Zybach.Web/.nvmrc                        ./Zybach.Web/.nvmrc
RUN NODE_VERSION=$(cat Zybach.Web/.nvmrc) \
    && echo "Installing Node Version: ${NODE_VERSION}" \
    && curl -SL "https://nodejs.org/dist/${NODE_VERSION}/node-${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && curl -SL "https://nodejs.org/dist/${NODE_VERSION}/SHASUMS256.txt" --output SHASUMS256.txt \
    && grep "linux-x64.tar.gz" SHASUMS256.txt > sha-sum.txt \
    && NODE_DOWNLOAD_SHA=$(awk '{print $1}' sha-sum.txt) \
    && echo "${NODE_DOWNLOAD_SHA} nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz SHASUMS256.txt sha-sum.txt \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs
    
COPY Zybach.Web/                        ./Zybach.Web

RUN dotnet restore Zybach.Web/Zybach.Web.csproj

RUN dotnet build ./Zybach.Web/Zybach.Web.csproj -c Release -o /app
WORKDIR /src/Zybach.Web/

RUN npm cache clean --force
RUN npm uninstall   -g angular-cli
RUN npm install     -g @angular/cli@16.2.8
RUN npm ci

ARG env
RUN npm run build-${env}

FROM build AS publish
RUN dotnet publish Zybach.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Zybach.Web.dll"]
