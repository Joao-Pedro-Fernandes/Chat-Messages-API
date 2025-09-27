FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
EXPOSE 80
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Chat-Messages-API/ChatMessages.Api/ChatMessages.Api.csproj", "ChatMessages.Api/"]
COPY ["Chat-Messages-API/ChatMessages.Domain/ChatMessages.Domain.csproj", "ChatMessages.Domain/"]
COPY ["Chat-Messages-API/ChatMessages.Infrastructure/ChatMessages.Infrastructure.csproj", "ChatMessages.Infrastructure/"]
COPY ["Chat-Messages-API/ChatMessages.Application/ChatMessages.Application.csproj", "ChatMessages.Application/"]
RUN dotnet restore "Chat-Messages-API/ChatMessages.Api/ChatMessages.Api.csproj"
COPY . .
WORKDIR "/src/ChatMessages.Api"
RUN dotnet build "ChatMessages.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatMessages.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatMessages.Api.dll"]