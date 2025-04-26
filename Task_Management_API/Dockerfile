   # Build Stage
   FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
   WORKDIR /app

   # Copy csproj and restore as distinct layers
   COPY *.csproj ./
   RUN dotnet restore

   # Copy everything else and build
   COPY . ./
   RUN dotnet publish -c Release -o out

   # Runtime Stage
   FROM mcr.microsoft.com/dotnet/aspnet:9.0
   WORKDIR /app
   COPY --from=build /app/out .

   # Expose port 80
   EXPOSE 80

   # Start the application
   ENTRYPOINT ["dotnet", "Task_Management_API.dll"]
   