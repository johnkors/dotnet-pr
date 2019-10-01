using System;
using PR;
using PR.PRTools;
using Xunit;

namespace dotnet_pr.tests
{
    public class ToolTests
    {
        [Fact]
        public void AzureDevOpsTests()
        {
            var tool = new AzureDevOps();
            var org = "someorg";
            var proj = "someproj";
            var repo = "somerepo";
            var context = new GitContext
            {
                RemoteUrl = $"git@ssh.dev.azure.com:v3/{org}/{proj}/{repo}",
                SourceBranch = "feature",
                TargetBranch = "master"
            };
            var prUrl = tool.BuildUrl(context);
            Assert.Equal("https://dev.azure.com/someorg/someproj/_git/somerepo/pullrequestcreate?targetRef=master&sourceRef=feature", prUrl);
        }
    }
}