# WebApi.BasicAuth

WebApi.BasicAuth provides HTTP Basic Authentication with usernames and passwords stored in `Web.config`.

NuGet package:
* [WebApi.BasicAuth](https://www.nuget.org/packages/WebApi.BasicAuth/)



## Usage

 * Add `config.EnableBasicAuth();` to your `WebApiConfig.cs` file.
 * Add this to your `Web.config` (inserting your own users):
```xml
<configuration>
  <configSections>
    <section name="basicAuth" type="WebApi.BasicAuth.BasicAuthSection, WebApi.BasicAuth" />
  </configSections>
  <basicAuth>
    <users>
      <user username="JohnDoe" password="ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad" hashAlgorithm="sha256">
        <roles>
          <role name="Operator"/>
        </roles>
      </user>
      <user username="JaneDoe" password="abc">
        <roles>
          <role name="Operator"/>
        </roles>
      </user>
    </users>
  </basicAuth>
  <!--...-->
</configuration>
```



## Sample project

The source code includes a sample project that uses demonstrates the usage of WebApi.BasicAuth. You can build and run it using Visual Studio 2015. By default the instance will be hosted by IIS Express at `http://localhost:20536/`.
