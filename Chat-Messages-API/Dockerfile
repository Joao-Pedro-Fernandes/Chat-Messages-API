FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
EXPOSE 80
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Odon.Track.Api/Odon.Track.Api.csproj", "Odon.Track.Api/"]
COPY ["Odon.Track.Application/Odon.Track.Application.csproj", "Odon.Track.Application/"]
RUN dotnet restore "Odon.Track.Api/Odon.Track.Api.csproj"
COPY . .
WORKDIR "/src/Odon.Track.Api"
RUN dotnet build "Odon.Track.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Odon.Track.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Odon.Track.Api.dll"]