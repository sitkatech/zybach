FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src

COPY ./DroolTool.Models ./DroolTool.Models
COPY ./DroolTool.EFModels ./DroolTool.EFModels
COPY ./DroolTool.API ./DroolTool.API

RUN dotnet publish ./DroolTool.API/DroolTool.API.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app /app
RUN ls -la
EXPOSE 80
ENTRYPOINT ["dotnet", "DroolTool.API.dll"]