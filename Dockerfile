FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /source
COPY test-api.csproj .
RUN dotnet restore -r linux-musl-x64 /p:PublishReadyToRun=true test-api.csproj

COPY . .
RUN dotnet publish -c release --property:PublishDir=/app -r linux-musl-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine
COPY --from=build /app .
ENTRYPOINT ["./test-api"]