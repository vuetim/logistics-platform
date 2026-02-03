# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy whole backend
COPY . .

# publish nga solution 
RUN dotnet publish LogisticsPlatform/LogisticsPlatform.Api/LogisticsPlatform.Api.csproj -c Release -o /app/publish


# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "LogisticsPlatform.Api.dll"]
