FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EnglishWordBot.Api/EnglishWordBot.Api.csproj", "EnglishWordBot.Api/"]
RUN dotnet restore "EnglishWordBot.Api/EnglishWordBot.Api.csproj"
COPY . .
WORKDIR "/src/EnglishWordBot.Api"
RUN dotnet build "EnglishWordBot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnglishWordBot.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnglishWordBot.Api.dll"]
