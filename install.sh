dotnet tool uninstall -g dotnet-pr
dotnet cake -target=pack -shipit=1
dotnet tool install -g --add-source ./builds/PR dotnet-pr --version 1.2.0
