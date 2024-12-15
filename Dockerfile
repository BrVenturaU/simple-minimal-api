FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /home/app
COPY ./*.sln ./
COPY ./*.csproj ./
RUN dotnet restore
COPY . .
FROM build AS publish
COPY --from=build /home/app /home/app/
RUN dotnet publish ./BackgroundDemo.csproj -o /publish/
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS deploy
COPY --from=publish /publish /app
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT [ "dotnet", "BackgroundDemo.dll" ]