#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#https://stackoverflow.com/questions/52991469/getting-program-does-not-contain-a-static-main-method-suitable-for-an-entry-p
# docker build -t thox:V1.0 .
# docker tag thox:dev s1188298/thox:V0.0
# docker push s1188298/thox:V0.0



FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
COPY . .
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Thox.csproj", "Thox/"]
RUN dotnet restore "./Thox/Thox.csproj"

WORKDIR "/src/Thox"
COPY . .
RUN dotnet build "Thox.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Thox.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Thox.dll"]