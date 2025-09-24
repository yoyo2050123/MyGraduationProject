# 使用官方 .NET 8 SDK 作為建置階段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 複製 .sln 和 csproj
COPY JapaneseLearnSystem/JapaneseLearnSystem.sln ./
COPY JapaneseLearnSystem/JapaneseLearnSystem.csproj ./JapaneseLearnSystem/

# 還原套件
RUN dotnet restore JapaneseLearnSystem.sln

# 複製專案檔案
COPY JapaneseLearnSystem/. ./JapaneseLearnSystem/

WORKDIR /src/JapaneseLearnSystem

# 建置
RUN dotnet publish -c Release -o /app/publish

# 執行階段
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 80

ENTRYPOINT ["dotnet", "JapaneseLearnSystem.dll"]
