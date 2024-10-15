FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Chat.Backend.Start/Chat.Backend.Start.csproj", "Chat.Backend.Start/"]
COPY ["Chat.Backend.Services/Chat.Backend.Services.csproj", "Chat.Backend.Services/"]
COPY ["Storage/Storage.csproj", "Storage/"]
COPY ["Chat.Backend.Abstractions/Chat.Backend.Abstractions.csproj", "Chat.Backend.Abstractions/"]
COPY ["Chat.Backend.Web/Chat.Backend.Web.csproj", "Chat.Backend.Web/"]
RUN dotnet restore "Chat.Backend.Start/Chat.Backend.Start.csproj"
COPY . .
WORKDIR "/src/Chat.Backend.Start"
RUN dotnet build "Chat.Backend.Start.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Chat.Backend.Start.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.Backend.Start.dll"]
