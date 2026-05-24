# https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/docker

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ./ProductManagement.Api/ProductManagement.Api.csproj ProductManagement.Api/
RUN dotnet restore ProductManagement.Api/ProductManagement.Api.csproj

COPY ./ProductManagement.Api ProductManagement.Api
RUN dotnet publish ProductManagement.Api/ProductManagement.Api.csproj -c release -o /publish/ProductManagement.Api --no-restore -p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /publish/ProductManagement.Api .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ProductManagement.Api.dll"]