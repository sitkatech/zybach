FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

ENV NODE_VERSION 12.16.1
ENV NODE_DOWNLOAD_SHA b2d9787da97d6c0d5cbf24c69fdbbf376b19089f921432c5a61aa323bc070bea

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
RUN npm install     -g @angular/cli@9.1.3
RUN npm install

ARG env
RUN npm run build-${env}

FROM build AS publish
RUN dotnet publish Zybach.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Zybach.Web.dll"]
