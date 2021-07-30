#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Order/ESourcing.Order/ESourcing.Order.csproj", "Services/Order/ESourcing.Order/"]
COPY ["Services/Order/Ordering.Application/Ordering.Application.csproj", "Services/Order/Ordering.Application/"]
COPY ["Services/Order/Ordering.Domain/Ordering.Domain.csproj", "Services/Order/Ordering.Domain/"]
COPY ["Services/Order/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "Services/Order/Ordering.Infrastructure/"]
COPY ["Services/Common/EventBusRabbitMq.csproj", "Services/Common/"]
RUN dotnet restore "Services/Order/ESourcing.Order/ESourcing.Order.csproj"
COPY . .
WORKDIR "/src/Services/Order/ESourcing.Order"
RUN dotnet build "ESourcing.Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESourcing.Order.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESourcing.Order.dll"]
