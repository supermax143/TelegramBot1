# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "TelegramBot.csproj"
RUN dotnet publish "TelegramBot.csproj" -c Release -o /app/publish

# Runtime stage 
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TelegramBot.dll"]