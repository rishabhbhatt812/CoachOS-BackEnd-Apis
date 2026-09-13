# Multi-stage Dockerfile for CoachOS ASP.NET Core API (.NET 10 preview)
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["CoachOS.Api/CoachOS.Api.csproj", "CoachOS.Api/"]
COPY ["CoachOS.Application/CoachOS.Application.csproj", "CoachOS.Application/"]
COPY ["CoachOS.Domain/CoachOS.Domain.csproj", "CoachOS.Domain/"]
COPY ["CoachOS.Infrastructure/CoachOS.Infrastructure.csproj", "CoachOS.Infrastructure/"]
COPY ["CoachOS.Shared/CoachOS.Shared.csproj", "CoachOS.Shared/"]

RUN dotnet restore "CoachOS.Api/CoachOS.Api.csproj"

# Copy full source and build
COPY . .
WORKDIR "/src/CoachOS.Api"
RUN dotnet publish "CoachOS.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CoachOS.Api.dll"]
