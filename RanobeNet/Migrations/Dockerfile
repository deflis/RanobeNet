#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RanobeNet/RanobeNet.csproj", "RanobeNet/"]
RUN dotnet restore "RanobeNet/RanobeNet.csproj"
COPY . .
WORKDIR "/src/RanobeNet"
RUN dotnet build "RanobeNet.csproj" -c Release -o /app/build

FROM build AS publish
COPY [".config/dotnet-tools.json", "."]
RUN dotnet tools restore
RUN dotnet ef migrations bundle --no-build -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "efbundle.exe"]