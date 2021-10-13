FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /app
COPY . .

RUN dotnet build ./twelve-factor-app.csproj --configuration Release

RUN dotnet publish twelve-factor-app.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "twelve-factor-app.dll"]