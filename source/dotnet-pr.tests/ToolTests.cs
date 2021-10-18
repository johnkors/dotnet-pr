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

        private void TestAzureDevops(string remoteUrl, string expectedUrl, IPRTool tool)
        {
            var org = "someorg";
            var proj = "someproj";
            var repo = "somerepo";
            var remoteUrlFormatted = string.Format(remoteUrl, org, proj, repo);

            var context = new GitContext
            {
                RemoteUrl = remoteUrlFormatted,
                SourceBranch = "feature",
                TargetBranch = "master"
            };

            Assert.True(tool.IsMatch(remoteUrlFormatted));

            var prUrl = tool.BuildUrl(context);

            _testOutputHelper.WriteLine($"{remoteUrlFormatted} => {prUrl}");

            Assert.Equal(expectedUrl, prUrl);
        }

        [Theory]
        [InlineData("git@ssh.dev.azure.com:v3/{0}/{1}/{2}")]
        [InlineData("https://{0}@dev.azure.com/{0}/{1}/_git/{2}")]
        public void AzureDevOpsPublicTests(string remoteUrl)
        {
            TestAzureDevops(remoteUrl, "https://dev.azure.com/someorg/someproj/_git/somerepo/pullrequestcreate?targetRef=master&sourceRef=feature", new AzureDevOps());
        }

        [Theory]
        [InlineData("https://{0}.visualstudio.com/DefaultCollection/{1}/_git/{2}")]
        [InlineData("{0}@vs-ssh.visualstudio.com:v3/{0}/{1}/{2}")]
        public void AzureDevOpsPrivateTests(string remoteUrl)
        {
            TestAzureDevops(remoteUrl, "https://someorg.visualstudio.com/someproj/_git/somerepo/pullrequestcreate?targetRef=master&sourceRef=feature", new AzureDevOpsPrivate());
        }
    }
}