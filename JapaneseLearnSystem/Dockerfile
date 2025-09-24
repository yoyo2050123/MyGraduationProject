# 使用官方 .NET 8 SDK 作為建置階段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 複製 sln 與 csproj
COPY JapaneseLearnSystem/JapaneseLearnSystem.sln ./
COPY JapaneseLearnSystem/JapaneseLearnSystem.csproj ./JapaneseLearnSystem/

# 還原套件
RUN dotnet restore JapaneseLearnSystem.sln

# 複製全部程式碼
COPY . .
WORKDIR /src/JapaneseLearnSystem

# 建置
RUN dotnet publish -c Release -o /app/publish
