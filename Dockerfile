	# Build stage
	FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
	WORKDIR /src
	COPY . .
	RUN dotnet restore BPCalculator/BPCalculator.csproj
	RUN dotnet publish BPCalculator/BPCalculator.csproj -c Release -o /app/published
	# Runtime stage
	FROM mcr.microsoft.com/dotnet/aspnet:9.0
	WORKDIR /app
	COPY --from=build /app/published .
	EXPOSE 8080
	# Azure App Service expects the app to listen on port 8080
	ENV ASPNETCORE_URLS=http://0.0.0.0:8080
	ENTRYPOINT ["dotnet", "BPCalculator.dll"]
