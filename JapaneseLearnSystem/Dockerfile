# 使用官方 .NET 8 SDK 作為建置階段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 建立工作目錄
WORKDIR /src

# 複製整個專案資料夾到容器
COPY JapaneseLearnSystem/ ./JapaneseLearnSystem/

# 切換到專案資料夾
WORKDIR /src/JapaneseLearnSystem

# 還原套件
RUN dotnet restore

# 建置並發佈到 /app/publish
RUN dotnet publish -c Release -o /app/publish

# 執行階段
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# 複製發佈結果
COPY --from=build /app/publish .

# 設定環境變數
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# 開放 80 port
EXPOSE 80

# 啟動應用程式
ENTRYPOINT ["dotnet", "JapaneseLearnSystem.dll"]
