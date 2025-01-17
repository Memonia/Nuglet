## About  
NuGet is a primary way of distributing packages in .NET ecosystem. NuGet supports local package feeds, which is useful when we need to try out packages before they are published to a remote repository. Setting up an environment in order to test packages requires several steps, such as creating a local package feed, creating a C# project, adding project references, etc. Most likely, a package will be updated several times during testing, so we also need to keep track of package versions (NuGet does not allow updating a package, only uploading a newer version) or manually replace packages, in which case we have to remember to clear the cache after every replacement.

Nuglet is a simple utility meant to automate this process. Given the packages, nuglet will create and set up a C# project, so we only need to open the project in our IDE and start working. Each project is completely independent from every other and does not affect system or user settings.

## How to use it
Nuglet is available as a [dotnet tool](https://www.nuget.org/packages/Memonia.Nuglet.Tool). The easiest way to get started is to go to a directory with packages and run ```nuglet *```. Once it's done, a new directory with a C# project should be created. For other options, type ```nuglet```, ```nuglet --help``` or see the screenshot below.  

![grafik](https://github.com/Memonia/Nuglet/assets/51869800/858d27fa-dae4-4654-83b8-4b21627d41e7)

## Caveats
#### NUGET_PACKAGES environmental variable
If you have set the ```NUGET_PACKAGES``` environmental variable, [it takes precedence](https://learn.microsoft.com/en-us/nuget/reference/nuget-config-file#config-section) over the NuGet package cache specified by nuglet, which may cause stale packages being restored for new projects.
