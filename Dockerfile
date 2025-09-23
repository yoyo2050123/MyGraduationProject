# 使用官方 .NET 8 SDK 作為建置階段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 複製 csproj 並還原相依套件
COPY *.sln ./
COPY JapaneseLearnSystem/*.csproj ./JapaneseLearnSystem/
COPY JapaneseLearnSystem/* ./JapaneseLearnSystem/
RUN dotnet restore

# 複製專案檔案並建置
COPY . .
WORKDIR /src/JapaneseLearnSystem
RUN dotnet publish -c Release -o /app/publish

# 執行階段映像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# 設定環境變數（可依需求調整）
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# 開放 80 port
EXPOSE 80

# 啟動應用程式
ENTRYPOINT ["dotnet", "JapaneseLearnSystem.dll"]