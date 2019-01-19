#addin nuget:?package=Cake.WebDeploy&version=0.3.3

var target = Argument("target", "Default");

var machine = EnvironmentVariable("PUBLISH_MACHINE");
var site = EnvironmentVariable("PUBLISH_SITE");
var username = EnvironmentVariable("PUBLISH_CREDENTIALS_USR");
var password = EnvironmentVariable("PUBLISH_CREDENTIALS_PSW");

if (string.IsNullOrWhiteSpace(machine)) {
	Error("Publish machine not provided.");

	return;
}

if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) {
	Error("Publish credentials not provided.");
	
	return;
}

Task("StartWebsite")
    .Does(() =>
	{
		StartSite(site);
	});

Task("StopWebsite")
    .Does(() =>
	{
		StopSite(site);
	});

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
    .IsDependentOn("StopWebsite")
    .IsDependentOn("Deploy")
    .IsDependentOn("StartWebsite");
	
RunTarget(target);