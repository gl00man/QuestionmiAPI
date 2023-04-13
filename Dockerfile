# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env
WORKDIR /Questionmi
COPY Questionmi/*.csproj .
RUN dotnet restore
COPY Questionmi .
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Questionmi.dll"]