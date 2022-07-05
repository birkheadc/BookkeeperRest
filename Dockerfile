FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5000

ARG API_KEY

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

ARG EMAILCONFIG_NAME
ARG EMAILCONFIG_ADDRESS
ARG EMAILCONFIG_SMTPSERVER
ARG EMAILCONFIG_PORT
ARG EMAILCONFIG_USERNAME
ARG EMAILCONFIG_PASSWORD

ARG DATABASE_USERNAME
ARG DATABASE_PASSWORD

ENV ASPNETCORE_EMAILCONFIG_NAME=${EMAILCONFIG_NAME}
ENV ASPNETCORE_EMAILCONFIG_ADDRESS=${EMAILCONFIG_ADDRESS}
ENV ASPNETCORE_EMAILCONFIG_SMTPSERVER=${EMAILCONFIG_SMTPSERVER}
ENV ASPNETCORE_EMAILCONFIG_PORT=${EMAILCONFIG_PORT}
ENV ASPNETCORE_EMAILCONFIG_USERNAME=${EMAILCONFIG_USERNAME}
ENV ASPNETCORE_EMAILCONFIG_PASSWORD=${EMAILCONFIG_PASSWORD}

ENV ASPNETCORE_CONNECTIONSTRING="server=bookkeeper-db;port=3306;database=bookkeeper;user=${DATABASE_USERNAME};password=${DATABASE_PASSWORD}"

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["BookkeeperRest.csproj", "./"]
RUN dotnet restore "BookkeeperRest.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BookkeeperRest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookkeeperRest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookkeeperRest.dll"]
