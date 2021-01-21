FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EstateManagement/NuGet.Config", "."]
COPY ["EstateManagement/EstateManagement.csproj", "EstateManagment/"]
COPY ["EstateManagement.BusinessLogic/EstateManagement.BusinessLogic.csproj", "EstateManagement.BusinessLogic/"]
COPY ["EstateManagement.EstateAggregate/EstateManagement.EstateAggregate.csproj", "EstateManagement.EstateAggregate/"]
COPY ["EstateManagement.DataTransferObjects/EstateManagement.DataTransferObjects.csproj", "EstateManagement.DataTransferObjects/"]
RUN dotnet restore "EstateManagment/EstateManagement.csproj"
COPY . .
WORKDIR "/src/EstateManagement"
RUN dotnet build "EstateManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EstateManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EstateManagement.dll"]