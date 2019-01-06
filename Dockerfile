FROM microsoft/dotnet:2.2-sdk

WORKDIR /

COPY ./ ./therepo
RUN dotnet tool install -g cake.tool
ENV PATH="/root/.dotnet/tools:${PATH}"
WORKDIR /therepo
RUN dotnet cake -verbosity=diagnostic
WORKDIR /therepo/source/dotnet-pr
RUN dotnet build -c Debug
RUN dotnet pack -o ../../builds/dotnet-pr /p:Version=999.0.0 -c Debug --no-build

# install dotnet-pr from local feed
WORKDIR /
RUN dotnet tool install -g dotnet-pr --add-source ./therepo/builds/dotnet-pr --version=999.0.0

RUN dotnet tool list -g
WORKDIR /therepo/source/dotnet-PR/Strategies
RUN pr --debug
RUN pr
RUN pr some-target-branch
RUN pr atarget --debug
RUN pr --debug somothertarget