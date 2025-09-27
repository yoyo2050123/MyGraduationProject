# =========================================
# 建置階段 (Build Stage)
# 使用官方 .NET 8 SDK
# =========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 設定工作目錄
WORKDIR /src

# 複製整個專案資料夾到容器
COPY JapaneseLearnSystem/ ./JapaneseLearnSystem/

# 切換工作目錄到專案資料夾
WORKDIR /src/JapaneseLearnSystem

# 還原 NuGet 套件
RUN dotnet restore

# 建置並發佈專案到 /app/publish
RUN dotnet publish -c Release -o /app/publish

# =========================================
# 執行階段 (Runtime Stage)
# 使用官方 .NET 8 ASP.NET runtime
# =========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# 設定工作目錄
WORKDIR /app

# 從建置階段複製發佈的檔案
COPY --from=build /app/publish .

# 設定環境變數
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# 開放 HTTP 80 port
EXPOSE 80

# 啟動應用程式
ENTRYPOINT ["dotnet", "JapaneseLearnSystem.dll"]
