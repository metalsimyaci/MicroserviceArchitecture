#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ApiGateway/ESourcing.ApiGateway/ESourcing.ApiGateway.csproj", "ApiGateway/ESourcing.ApiGateway/"]
RUN dotnet restore "ApiGateway/ESourcing.ApiGateway/ESourcing.ApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateway/ESourcing.ApiGateway"
RUN dotnet build "ESourcing.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESourcing.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESourcing.ApiGateway.dll"]
