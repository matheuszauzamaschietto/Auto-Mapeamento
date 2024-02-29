#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LeitorExcelV3/LeitorExcelV3.csproj", "LeitorExcelV3/"]
RUN dotnet restore "./LeitorExcelV3/./LeitorExcelV3.csproj"
COPY . .
WORKDIR "/src/LeitorExcelV3"
RUN dotnet build "./LeitorExcelV3.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LeitorExcelV3.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
#RUN apt-get install libgdiplus=32.x
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeitorExcelV3.dll"]