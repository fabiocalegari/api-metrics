FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["APIMetrics22/APIMetrics22.csproj", "APIMetrics22/"]
RUN dotnet restore "APIMetrics22/APIMetrics22.csproj"
COPY . .
WORKDIR "/src/APIMetrics22"
RUN dotnet build "APIMetrics22.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "APIMetrics22.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "APIMetrics22.dll"]