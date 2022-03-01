#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app



RUN apt-get update && apt-get install -y --no-install-recommends \
    xvfb

RUN apt-get update && apt-get install -y --no-install-recommends \
    fonts-liberation\
    libasound2\
    libatk-bridge2.0-0\
    libatk1.0-0\
    libatspi2.0-0\
    libcairo2\
    libcups2\
    libdbus-1-3\
    libdrm2\
    libgbm1\
    libglib2.0-0\
    libgtk-3-0\
    libnspr4\
    libnss3\
    libpango-1.0-0\
    libx11-6\
    libxcb1\
    libxcomposite1\
    libxdamage1\
    libxext6\
    libxfixes3\
    libxrandr2


 RUN apt-get update && apt-get install -y npm && \
     npm i playwright-chromium@latest && \
     npm i playwright-firefox@latest && \
     npm i playwright-webkit@latest

RUN apt-get update && apt-get install -y --no-install-recommends libegl1\
          libnotify4\
          libopus0\
          libxslt1.1\
          libwoff1\
          libharfbuzz-icu0\
          gstreamer1.0-plugins-base\
          libgstreamer1.0-0\
          libgstreamer-gl1.0-0\
          gstreamer1.0-plugins-bad\
          libopenjp2-7\
          libwebpdemux2\
          libsecret-1-0\
          libhyphen0\
          libevdev2\
          libgles2\
          gstreamer1.0-libav

RUN npm install -D playwright
RUN npm i -g npx
RUN npx playwright install-deps


EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
#COPY ["NuGet.Config", "."]
COPY ["PlaywrightTestLinuxContainer/PlaywrightTestLinuxContainer.csproj", "PlaywrightTestLinuxContainer/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "PlaywrightTestLinuxContainer/PlaywrightTestLinuxContainer.csproj"
COPY . .
WORKDIR "/src/PlaywrightTestLinuxContainer"
RUN dotnet build "PlaywrightTestLinuxContainer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlaywrightTestLinuxContainer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /app/publish/.playwright bin/.playwright
ENTRYPOINT ["dotnet", "PlaywrightTestLinuxContainer.dll"]