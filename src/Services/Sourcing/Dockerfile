#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Sourcing/ESourcing.Sourcing.csproj", "Services/Sourcing/"]
COPY ["Services/Common/EventBusRabbitMq.csproj", "Services/Common/"]
RUN dotnet restore "Services/Sourcing/ESourcing.Sourcing.csproj"
COPY . .
WORKDIR "/src/Services/Sourcing"
RUN dotnet build "ESourcing.Sourcing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESourcing.Sourcing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESourcing.Sourcing.dll"]