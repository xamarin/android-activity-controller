var target = Argument ("t", Argument ("target", "libs"));

var NUGET_VERSION = "0.1.0-beta2";

Task ("libs").Does (() => {
	EnsureDirectoryExists ("./output");

    NuGetRestore ("./Xamarin.Android.ActivityController.sln");
    DotNetBuild ("./Xamarin.Android.ActivityController.sln", c => c.Configuration = "Release");

    CopyFiles ("./**/bin/Release/*.dll", "./output");
});

Task ("nuget").IsDependentOn ("libs").Does (() => {
	EnsureDirectoryExists ("./output");
	
    NuGetPack ("./Xamarin.Android.ActivityController.nuspec", new NuGetPackSettings {
        Version = NUGET_VERSION,
        OutputDirectory = "./output"
    });
});

Task ("clean").Does (() => {
    CleanDirectories ("./**/bin");
    CleanDirectories ("./**/obj");
    CleanDirectories ("./**/packages");
    CleanDirectories ("./**/output");
    DeleteFiles ("./**/*.nupkg");
});

RunTarget (target);