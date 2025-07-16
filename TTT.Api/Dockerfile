FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8000

ENV ASPNETCORE_URLS=http://+:8000

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["TTT.Api/TTT.Api.csproj", "TTT.Api/"]
COPY ["TTT.Core/TTT.Core.csproj", "TTT.Core/"]
COPY ["TTT.Data/TTT.Data.csproj", "TTT.Data/"]
COPY ["TTT.Services/TTT.Services.csproj", "TTT.Services/"]
RUN dotnet restore "TTT.Api/TTT.Api.csproj"
COPY . .
WORKDIR "/src/TTT.Api"
RUN dotnet build "TTT.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "TTT.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TTT.Api.dll"]
