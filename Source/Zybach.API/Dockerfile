FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src

COPY ./Zybach.Models ./Zybach.Models
COPY ./Zybach.EFModels ./Zybach.EFModels
COPY ./Zybach.API ./Zybach.API

RUN dotnet publish ./Zybach.API/Zybach.API.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app /app
RUN ls -la
EXPOSE 80
ENTRYPOINT ["dotnet", "Zybach.API.dll"]
