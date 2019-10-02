using System;
using PR;
using PR.PRTools;
using Xunit;
using Xunit.Abstractions;

namespace dotnet_pr.tests
{
    public class ToolTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ToolTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
        [Theory]
        [InlineData("git@ssh.dev.azure.com:v3/{0}/{1}/{2}")]
        [InlineData("https://{0}@dev.azure.com/{0}/{1}/_git/{2}")]
        public void AzureDevOpsTests(string remoteUrl)
        {
            var tool = new AzureDevOps();
            var org = "someorg";
            var proj = "someproj";
            var repo = "somerepo";
            var remoteUrlFormatted = string.Format(remoteUrl,org, proj, repo);
            var context = new GitContext
            {
                RemoteUrl = remoteUrlFormatted,
                SourceBranch = "feature",
                TargetBranch = "master"
            };
            var prUrl = tool.BuildUrl(context);
            _testOutputHelper.WriteLine($"{remoteUrlFormatted} => {prUrl}");
            Assert.Equal("https://dev.azure.com/someorg/someproj/_git/somerepo/pullrequestcreate?targetRef=master&sourceRef=feature", prUrl);
        }
    }
}