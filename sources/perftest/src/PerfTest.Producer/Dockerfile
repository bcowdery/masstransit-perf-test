FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

# Restore nuget packages
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /sln
COPY . .
RUN dotnet restore /sln/src/PerfTest.Producer

# Build and publish applications
FROM build as publish
WORKDIR /out
RUN dotnet publish /sln/src/PerfTest.Producer -c Release -o /out

# Build runtime image
FROM base as final
WORKDIR /app
COPY --from=publish /out /app/
ENTRYPOINT [ "dotnet", "PerfTest.Producer.dll" ]