#!/usr/bin/env pwsh
param(
    $basePath = $PSScriptRoot
)
docker build --file=src/Dockerfile $basePath -t=samplewebapp-bff

docker run --rm -it --name samplewebapp-bff -v "$($basePath)/out:/app/out" samplewebapp-bff 

Use-AwsProfile

stackit up --stack-name samplewebapp --template ./app.yml