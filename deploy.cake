#addin nuget:?package=Cake.WebDeploy&version=0.3.3

var target = Argument("target", "Default");

var machine = Argument("machine");
var username = Argument("username");
var password = Argument("password");

Task("Deploy")
    .Does(() =>
    {
        DeployWebsite(new DeploySettings()
        {
            SourcePath = "./release/release.zip",
            SiteName = "stage-api",

            ComputerName = machine,
            Username = username,
            Password = password
        });
    });
	
Task("Default")
    .IsDependentOn("Deploy");
	
RunTarget(target);