Release Notes:
v1.1.*
* Smarter and simpler template authoring. Just set BuildAction to None on all your 
  template content as well as the .vstemplate, and they become Smart Templates automatically:
	* Supports &lt;Include&gt; metadata to add shared artifacts to the generated ZIP files
	* Does not regenerate ZIP files if content didn't change
	* Supports linked files that are copied to the output directory
* Sets up the project to always start by running devenv.exe in the experimental 
  instance and with logging enabled for easy error diagnostics.

v1.0

* Provides the following MSBuild properties for version-aware projects:
	* VisualStudioVersion: for VS2010, sets it to '10.0'
	* MinimumVisualStudioVersion: equals VisualStudioVersion to allow opening on any version
	* DevEnvDir: if it's empty, for safe command-line building. Can be overriden.
	* PublicAssemblies: $(DevEnvDir)\PublicAssemblies\
	* PrivateAssemblies: $(DevEnvDir)\PrivateAssemblies\
	* VSSDK: the [VSSDK install directory]\VisualStudioIntegration\Common\Assemblies\ folder. Can be overriden.
	* VSSDK20: $(VSSDK)v2.0\
	* VSSDK40: $(VSSDK)v4.0\
	* VSToolsPath: path to the MSBuild targets for the VSSDK