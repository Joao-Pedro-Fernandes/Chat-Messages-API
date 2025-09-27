FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
EXPOSE 80
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ChatMessages.Api/ChatMessages.API.csproj", "ChatMessages.Api/"]
COPY ["ChatMessages.Domain/ChatMessages.Domain.csproj", "ChatMessages.Domain/"]
COPY ["ChatMessages.Infrastructure/ChatMessages.Infrastructure.csproj", "ChatMessages.Infrastructure/"]
COPY ["ChatMessages.Application/ChatMessages.Application.csproj", "ChatMessages.Application/"]
RUN dotnet restore "ChatMessages.Api/ChatMessages.API.csproj"
COPY . .
WORKDIR "/src/ChatMessages.Api"
RUN dotnet build "ChatMessages.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatMessages.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatMessages.API.dll"]