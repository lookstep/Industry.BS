#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EmoloyeeTask.API/Industry.BS.API.csproj", "EmoloyeeTask.API/"]
COPY ["EmoloyeeTask.Data.Migrations/Industry.BS.Data.Migrations.csproj", "EmoloyeeTask.Data.Migrations/"]
COPY ["EmoloyeeTask.Data/Industry.BS.Data.csproj", "EmoloyeeTask.Data/"]
COPY ["EmpolyeeTask.Shared/Industry.BS.Shared.csproj", "EmpolyeeTask.Shared/"]
RUN dotnet restore "EmoloyeeTask.API/Industry.BS.API.csproj"
COPY . .
WORKDIR "/src/EmoloyeeTask.API"
RUN dotnet build "Industry.BS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Industry.BS.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Industry.BS.API.dll"]