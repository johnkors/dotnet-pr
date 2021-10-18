[![NuGet](https://img.shields.io/nuget/v/dotnet-pr.svg)](https://www.nuget.org/packages/dotnet-pr/)
[![NuGet](https://img.shields.io/nuget/dt/dotnet-pr.svg)](https://www.nuget.org/packages/dotnet-pr/)
[![Build](https://github.com/johnkors/dotnet-pr/workflows/CI/badge.svg)](https://github.com/johnkors/dotnet-pr/actions)
## dotnet-pr


This a .NET Core Global Tool that ..

1) Given your terminals working directory is a git repo

2) That git repo has a remote that is a code collaboration tool (ex. `origin` -> GitHub/BitBucket/GitLab/Azure DevOps)

3) Opens your default browser in the Pull Request UI for the _current branch_ in the tool connected to the remote branch.


## Requirements

* [.NET Core SDK 2.1 or later](https://dotnet.microsoft.com/download)

## Install

```
$ dotnet tool install dotnet-pr -g
```

## Usage

After install, navigate to a git repo and call `pr` from the terminal.

### Optional arguments

* debug output

```
$ pr --debug
```

* Target the PR towards another branch than `master` (default)

```
$ pr <your-target-branch-here>
```

## Supported PR tools

* github.com
* bitbucket.org
* gitlab.com
* dev.azure.com (Azure DevOps, dev.azure.com/{org})
* dev.azure.com (Azure DevOps Private, {org}.visualstudio.com)
* (Bitbucket (Self-Hosted))
