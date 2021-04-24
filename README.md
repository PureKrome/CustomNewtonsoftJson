<h1 align="center">Creating a custom Newtonsoft.Json nuget package</h1>

## Why?

- `Newtonsoft.Json` has a bug - yes! this actually happens on one of _the_ most popular/used nuget packages
- We want to patch the bug and make sure all our projects _and their transitive packages_ use this version, instead

## Thank you's

These answers were provided with the help of
- Martin Ullrich aka [@dasmulli](https://twitter.com/dasmulli).
- Patrick Westerhoff aka [@poke](https://twitter.com/poke)
- Alexandre Hertogs aka [@TeBeCo](https://twitter.com/Alexandre_HGS)

Also refer to [this StackOverflow question](https://stackoverflow.com/questions/67197213/how-to-create-a-custom-newtonsoft-json-nuget-to-use-in-my-net-core-3-1-app/67234069#67234069) I asked and they answered.

---
## How to get this to work

### 1. Fork then clone [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)

`git clone ... etc`

### 2. Fix up the bug.

This is your custom code

### 3. Export the existing public key from an _existing_ Newtonsoft.Json package

This is a bit tricky. We need to export something called a "public key" to a file. We then will use this file, later on.

Easiest way to do this is to find an _existing_ copy of the newtonsoft.json.nupkg file. For windows, this is:

`C:\Users\<username>\.nuget\packages\newtonsoft.json\12.0.3\lib\netstandard2.0>sn -e Newtonsoft.Json.dll backup-newtonsoft.json-public-key.snk`

Notice the:
- `username` in the path. Change to whatever yours is
- the `version` in the path. I'm doing this against v `12.0.3`.

Then copy the `backup-newtonsoft.json-public-key.snk` to your fork + cloned repo and copy it to the `<repo root>\src\Newtonsoft.Json\` folder.


### 4. Edit the main Newtonsoft.Json.csproj

First change the version in these places (using SEMVER):

```
    <AssemblyVersion>12.1.0.0</AssemblyVersion>
    <FileVersion>12.1.0</FileVersion>
    <VersionPrefix>12.1.0</VersionPrefix>
    <VersionSuffix>beta1</VersionSuffix>
```

Next add the delay signing stuff. This uses that file you exported in step #3, above:
```
    <SignAssembly>true</SignAssembly>
    <AssemblyOriginatorKeyFile>backup-newtonsoft.json-public-key.snk</AssemblyOriginatorKeyFile>
    <DelaySign>true</DelaySign>
```

I added this _right underneath the last item in this `<PropertyGroup>`:

```
    <EmbedUntrackedSources>true</EmbedUntrackedSources>  <-- last line
    <SignAssembly>true</SignAssembly>                    <-- start of new lines I've added
    ..... etc .....
```

### 5. Create the nuget

From the repo root:

`dotnet pack -c RELEASE -o .\custom-nuget-packages\  .\Newtonsoft.Json\Newtonsoft.Json.csproj`

This will now create the NuGet packages in a new `custom-nuget-packages` folder.

### 6. Use these new NuGet packages

Now copy these newly generated nuget packages (`.nupkg and .snupkg`) from this repo to your own other/custom repo.

e.g.

`<other-repo-root>/custom-nuget-packages/Newtonsoft.Json.12.1.0-beta1.nupkg`

### 7. Create/update your nuget.config

To use this nuget package, we need to be able to 'find' them. The easiest way to is to add (or update if it already exists) a `nuget.config` file:

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="localhost-custom-nugets" value="custom-nugets/" />
  </packageSources>
</configuration>
```

This adds a new nuget location to any existing configuration settings.

### 8. Reference this nuget in your project

And we're done. Add this to your `.csproj`

`<PackageReference Include="Newtonsoft.Json" Version="12.1.0-beta1" />`

---

## Sample programs

I've included 2x sample demo's to demonstrate
- ❌ not working (the dll was not delayed signed)
- ✅ working

With the not working, you will notice that it doesn't compile because it's still after a specific version of Newtonsoft.Json

With the working one, it compiles.

Finally, I added a NEW method to my custom Newtonsoft.Json library to _proove_ that my code is using _my custom version_:

```
// Proof -> my custom Newtonsoft.Json dll is being used.
Console.WriteLine(JsonConvert.HelloWorldTest());
```

---
