FROM microsoft/dotnet:2.2-sdk

WORKDIR /

COPY ./ ./

WORKDIR /source/PR
RUN dotnet build -c Debug
RUN dotnet pack -o ../../builds/PR /p:Version=999.0.0 -c Debug --no-build

# install dotnet-retire from local feed
WORKDIR /
RUN dotnet tool install -g dotnet-pr --add-source ./builds/PR --version=999.0.0
ENV PATH="/root/.dotnet/tools:${PATH}"

RUN dotnet tool list -g
WORKDIR /source/PR/Strategies
RUN pwd
RUN pr 