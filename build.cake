var target = Argument("target", "Default");
var configuration = Argument("Configuration", "Release");

Information($"Running target {target} in configuration {configuration}");

var projectName = "AlbaVulpes.API";
var distDirectory = Directory("./dist");

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(distDirectory);
    });

Task("Restore")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

Task("Build")
	.Does(() =>
	{
		DotNetCoreBuild(".",
			new DotNetCoreBuildSettings()
			{
				Configuration = configuration,
				NoRestore = true
			});
	});

Task("PublishDist")
    .Does(() =>
    {
        DotNetCorePublish(
            $"./src/{projectName}.csproj",
            new DotNetCorePublishSettings()
            {
                Configuration = configuration,
                OutputDirectory = distDirectory,
				NoRestore = true,
				SelfContained = true,
				Runtime = "win-x64"
            });
    });

Task("ZipDist")
    .IsDependentOn("PublishDist")
    .Does(() =>
    {
		var releaseZipPath = "./release/release.zip";

		var releaseDir = System.IO.Path.GetDirectoryName(releaseZipPath);

		CreateDirectory(releaseDir);
		CleanDirectory(releaseDir);

		Zip("./dist", releaseZipPath);

		Information($"Created release zip at {releaseZipPath}");
    });

Task("FullBuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build");
	
Task("Release")
    .IsDependentOn("FullBuild")
    .IsDependentOn("ZipDist");

Task("Default")
    .IsDependentOn("Release");
	
RunTarget(target);