FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Products/ESourcing.Products.csproj", "Products/"]
RUN dotnet restore "Services/Products/ESourcing.Products.csproj"
COPY . .
WORKDIR "/src/Products"
RUN dotnet build "ESourcing.Products.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESourcing.Products.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESourcing.Products.dll"]
