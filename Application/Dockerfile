FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Application/Application.csproj", "Application/"]
RUN dotnet restore "Application/Application.csproj"
COPY . .
WORKDIR "/src/Application"
RUN dotnet build "Application.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Application.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Application.dll"]