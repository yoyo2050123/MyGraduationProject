# =========================================
# 建置階段 (Build Stage)
# =========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# 複製整個 repo（保持原始結構）
COPY ./ ./

# 還原套件（.sln 在 /src/JapaneseLearnSystem）
RUN dotnet restore ./JapaneseLearnSystem/JapaneseLearnSystem.sln

# 建置並發佈
RUN dotnet publish ./JapaneseLearnSystem/JapaneseLearnSystem.sln -c Release -o /app/publish

# =========================================
# 執行階段 (Runtime Stage)
# =========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "JapaneseLearnSystem.dll"]
