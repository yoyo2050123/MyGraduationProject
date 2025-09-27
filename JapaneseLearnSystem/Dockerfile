# =========================================
# 建置階段 (Build Stage)
# 使用官方 .NET 8 SDK
# =========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 設定工作目錄
WORKDIR /src

# 複製專案資料夾到容器
COPY ./ ./JapaneseLearnSystem/

# 切換工作目錄到專案資料夾（這裡有 .sln）
WORKDIR /src/JapaneseLearnSystem

# 還原套件（使用 .sln）
RUN dotnet restore JapaneseLearnSystem.sln

# 建置並發佈專案
RUN dotnet publish JapaneseLearnSystem.sln -c Release -o /app/publish

# =========================================
# 執行階段 (Runtime Stage)
# 使用官方 .NET 8 ASP.NET runtime
# =========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# 複製發佈結果
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "JapaneseLearnSystem.dll"]
