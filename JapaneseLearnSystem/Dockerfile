# =========================================
# 建置階段 (Build Stage)
# =========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 設定工作目錄
WORKDIR /src

# 複製專案到容器
COPY ./ ./

# 還原套件
RUN dotnet restore JapaneseLearnSystem.sln

# 建置並發佈
RUN dotnet publish JapaneseLearnSystem.sln -c Release -o /app/publish

# =========================================
# 執行階段 (Runtime Stage)
# =========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# 複製發佈結果
COPY --from=build /app/publish ./

# 設定環境變數
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "JapaneseLearnSystem.dll"]
