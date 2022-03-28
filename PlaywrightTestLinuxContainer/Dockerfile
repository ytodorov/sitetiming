FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app

RUN apt-get update && apt-get install -y npm
RUN apt-get update && apt-get install -y curl
RUN npm i -g n #Error: curl or wget command required, Install n, Node’s version manager:
RUN n stable # Install the latest NODE stable version
RUN npm install -D playwright
RUN npm i -g npx --force # Remove the existing file and try again, or run npm with --force to overwrite files recklessly
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

# Executable doesn't exist at /root/.cache/ms-playwright/chromium-965416/chrome-linux/chrome T
# This error is missmatch between latest chrome in docker and packages in nugget
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /app/publish/.playwright bin/.playwright
ENTRYPOINT ["dotnet", "PlaywrightTestLinuxContainer.dll"]