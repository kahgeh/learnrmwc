FROM mcr.microsoft.com/dotnet/core/sdk:3.0
RUN apt-get update
RUN apt-get install zip -y
RUN dotnet tool install -g Amazon.Lambda.Tools
WORKDIR /app
COPY src/libs/AppOps/AppOps.csproj src/libs/AppOps/AppOps.csproj
COPY src/SampleWebApp.Bff/SampleWebApp.Bff.csproj src/SampleWebApp.Bff/SampleWebApp.Bff.csproj
COPY src/*.sln src
RUN dotnet restore ./src/SampleApp.sln

COPY .git .git
COPY src/libs src/libs
COPY src/SampleWebApp.Bff src/SampleWebApp.Bff
RUN dotnet build ./src/SampleApp.sln
ENTRYPOINT ["/root/.dotnet/tools/dotnet-lambda", "package", "-pl", "src/SampleWebApp.Bff", "-o", "/app/out/SampleWebApp.Bff.zip"]