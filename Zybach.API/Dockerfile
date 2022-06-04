FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src

COPY ./Zybach.Models ./Zybach.Models
COPY ./Zybach.EFModels ./Zybach.EFModels
COPY ./Zybach.API ./Zybach.API
COPY ./VegaRenderService ./VegaRenderService

RUN dotnet publish ./Zybach.API/Zybach.API.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app /app
EXPOSE 80
ENTRYPOINT ["dotnet", "Zybach.API.dll"]
