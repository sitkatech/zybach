FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src

COPY ./Zybach.Models ./Zybach.Models
COPY ./Zybach.EFModels ./Zybach.EFModels
COPY ./Zybach.API ./Zybach.API
COPY ./SitkaCaptureService ./SitkaCaptureService

RUN dotnet publish ./Zybach.API/Zybach.API.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS final
WORKDIR /app
COPY --from=build /app /app
RUN ls -la
EXPOSE 80
ENTRYPOINT ["dotnet", "Zybach.API.dll"]
