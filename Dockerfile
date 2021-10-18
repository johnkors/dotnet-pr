FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /
COPY ./ ./therepo

WORKDIR /therepo/source/dotnet-pr
RUN dotnet pack -o ../../builds/dotnet-pr /p:Version=999.3.1

# install dotnet-pr from local feed
WORKDIR /
RUN dotnet tool install -g dotnet-pr --add-source ./therepo/builds/dotnet-pr --version=999.3.1
ENV PATH="/root/.dotnet/tools:${PATH}"
RUN dotnet tool list -g
WORKDIR /therepo/source/dotnet-PR/Strategies
RUN pr --debug
RUN pr
RUN pr some-target-branch
RUN pr atarget --debug
RUN pr --debug somothertarget
