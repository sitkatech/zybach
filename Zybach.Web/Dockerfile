FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

ENV NODE_VERSION 18.10.0
ENV NODE_DOWNLOAD_SHA f468b86031cca41ee9aa7a911e70eb624413153c7432754cbe9206c7ef3de090

SHELL ["/bin/bash", "-o", "pipefail", "-c"]
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

    
COPY Zybach.Web/                        ./Zybach.Web

RUN dotnet restore Zybach.Web/Zybach.Web.csproj

RUN dotnet build ./Zybach.Web/Zybach.Web.csproj -c Release -o /app
WORKDIR /src/Zybach.Web/

RUN npm cache clean --force
RUN npm uninstall   -g angular-cli
RUN npm install     -g @angular/cli@16.2.8
RUN npm install

ARG env
RUN npm run build-${env}

FROM build AS publish
RUN dotnet publish Zybach.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Zybach.Web.dll"]
