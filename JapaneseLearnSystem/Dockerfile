# =========================================
# 建置階段 (Build Stage)
# 使用官方 .NET 8 SDK
# =========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 設定工作目錄
WORKDIR /src

# 複製整個 repo 到容器中
# 假設你執行 gcloud builds submit 時的 context 是 repo 根目錄
COPY ./ ./ 

# 還原套件 (.sln 位於 /src/JapaneseLearnSystem)
RUN dotnet restore ./JapaneseLearnSystem/JapaneseLearnSystem.sln

# 建置並發佈專案
RUN dotnet publish ./JapaneseLearnSystem/JapaneseLearnSystem.sln -c Release -o /app/publish

# =========================================
# 執行階段 (Runtime Stage)
# 使用官方 .NET 8 ASP.NET runtime
# =========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# 設定工作目錄
WORKDIR /app

# 複製從建置階段產生的發佈結果
COPY --from=build /app/publish .

# 設定環境變數
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# 開放容器埠口
EXPOSE 8080

# 啟動專案
ENTRYPOINT ["dotnet", "JapaneseLearnSystem.dll"]
