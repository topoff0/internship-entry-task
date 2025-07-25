FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore TTT.Tests/TTT.Tests.Unit.csproj
RUN dotnet restore TTT.Tests/TTT.Tests.Integration.csproj

RUN dotnet build TTT.Tests/TTT.Tests.Unit.csproj -c Release
RUN dotnet build TTT.Tests/TTT.Tests.Integration.csproj -c Release

CMD ["dotnet", "test", "TTT.Tests/TTT.Tests.Unit.csproj"]
