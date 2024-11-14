FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-env

WORKDIR /src

COPY "Reactivities.sln" "Reactivities.sln"
COPY ["Reactivities.Api/Reactivities.Api.csproj", "Reactivities.Api/"]
COPY ["Reactivities.Application/Reactivities.Application.csproj", "Reactivities.Application/"]
COPY ["Reactivities.Infrastructure/Reactivities.Infrastructure.csproj", "Reactivities.Infrastructure/"]
COPY ["Reactivities.Persistence/Reactivities.Persistence.csproj", "Reactivities.Persistence/"]

RUN dotnet restore "Reactivities.sln"

COPY . .

RUN dotnet build "./Reactivities.sln" -c Release -o /app/build


FROM build-env AS publish-env

RUN dotnet publish "./Reactivities.sln" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 as work-env

WORKDIR /app

COPY --from=publish-env /app/publish .

EXPOSE 8080

ENTRYPOINT [ "dotnet", "Reactivities.Api.dll" ]