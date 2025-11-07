FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["SampleApp/FrontEnd/FrontEnd.csproj", "SampleApp/FrontEnd/"]
RUN dotnet restore "SampleApp/FrontEnd/FrontEnd.csproj"
COPY . .
WORKDIR "/src/SampleApp/FrontEnd"
RUN dotnet build "FrontEnd.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "FrontEnd.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrontEnd.dll"]
