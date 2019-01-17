#addin nuget:?package=Cake.WebDeploy&version=0.3.3

var target = Argument("target", "Default");

var machine = EnvironmentVariable("PUBLISH_MACHINE");
var username = EnvironmentVariable("PUBLISH_CREDENTIALS_USR");
var password = EnvironmentVariable("PUBLISH_CREDENTIALS_PSW");

if (string.IsNullOrWhiteSpace(machine)) {
	Information("Publish machine not provided.");
}

if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) {
	Information("Publish credentials not provided.");
}

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