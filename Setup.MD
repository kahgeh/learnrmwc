
# Building 

## cd into repo root location

docker build --file=src/Dockerfile . -t=samplewebapp-bff

docker run --rm -it --name samplewebapp-bff -v "$(PWD)/out":/app/out samplewebapp 