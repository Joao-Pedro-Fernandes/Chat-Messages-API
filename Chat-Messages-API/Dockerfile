FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
EXPOSE 80
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ChatMessages.API/ChatMessages.API.csproj", "ChatMessages.API/"]
COPY ["ChatMessages.Domain/ChatMessages.Domain.csproj", "ChatMessages.Domain/"]
COPY ["ChatMessages.Infrastructure/ChatMessages.Infrastructure.csproj", "ChatMessages.Infrastructure/"]
COPY ["ChatMessages.Application/ChatMessages.Application.csproj", "ChatMessages.Application/"]
RUN dotnet restore "ChatMessages.API/ChatMessages.API.csproj"
COPY . .
WORKDIR "/src/ChatMessages.API"
RUN dotnet build "ChatMessages.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatMessages.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatMessages.API.dll"]