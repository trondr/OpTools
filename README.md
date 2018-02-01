# Op Tools
=========================

github.com.trondr Op Tools

## Usage

```text
trondr OpTools 1.0.18022.2.564b217 - github.com.trondr Op Tools
Copyright © github.com.trondr 2018
Author: trondr@outlook.com
Usage: trondr.OpTools.exe <command> [parameters]

Commands:
---------
Help                         Display this help text
License                      Display license
Credits                      Display credits
RunScript                    Run PowerShell script against all (or a random
                             sample of) targets in a host list.

Commands and parameters:
------------------------
RunScript                    Run PowerShell script against all (or a random
                             sample of) targets in a host list. The online
                             status (ping) of each host will be checked
                             before attempting to run the script. The script
                             will run concurrently to speed up the overall
                             processing time. The Powershell script should
                             supports two input parameters: The computer name
                             or ip address of the target host 'HostName' and
                             the result path 'ResultFolderPath'. The
                             powershell script is itself responsible for
                             accessing the remote host and uploading the
                             result to the given result path.
   /scriptPath               [Required] Path to the Powershell script.
                             Alternative parameter name: /sp
   /hostNameListCsv          [Required] Path to a csv file with list of host
                             names to run the Powershell script against. Csv
                             format: HostName. Alternative parameter name:
                             /hnl
   /resultFolderPath         [Required] Result folder path. Each script
                             execution can upload the result to this path.
                             Alternative parameter name: /rfp
   /samplePercent            [Optional] Specify a samplePercent less than 100
                             to run the script against a random sample of the
                             host names in the host list. A value of 100
                             (default) means the script will run against all
                             host names in the list. Alternative parameter
                             name: /sap. Default value: 100
   /resolveToIpv4Address     [Optional] Resolve host name to ip v4 address
                             before executing script. Alternative parameter
                             name: /rip. Default value: False
   /scriptExecutionParallelism[Optional] The number of concurrent script
                             executions. Alternative parameter name: /sep.
                             Default value: 10

   Example: trondr.OpTools.exe RunScript /scriptPath="c:\temp\test.ps1" /hostNameListCsv="c:\temp\hostnames.csv" /resultFolderPath="c:\temp" /samplePercent="100" /resolveToIpv4Address="False" /scriptExecutionParallelism="10" 
   Example (alternative): trondr.OpTools.exe RunScript /sp="c:\temp\test.ps1" /hnl="c:\temp\hostnames.csv" /rfp="c:\temp" /sap="100" /rip="False" /sep="10" 
 

```

## Powershell script template

```powershell
param (
    [Parameter(Mandatory=$true)]
    [string]$HostName,
    [Parameter(Mandatory=$true)]
    [string]$ResultFolderPath
    )

Set-StrictMode -Version Latest
Write-Host "HostName=$HostName"
Write-Host "ResultFolderPath=$ResultFolderPath"

#
# Your script here
#
```
  
  
## Powershell script example

```powershell
param (
    [Parameter(Mandatory=$true)]
    [string]$HostName,
    [Parameter(Mandatory=$true)]
    [string]$ResultFolderPath
    )

Set-StrictMode -Version Latest
Write-Host "HostName=$HostName"
Write-Host "ResultFolderPath=$ResultFolderPath"
$resultFileName = $HostName + "_result_wmic.txt"
$resultFilePath = [System.IO.Path]::Combine($ResultFolderPath,$resultFileName)
wmic /node:$HostName qfe list full > "$resultFilePath"
```   
   
## Host list csv example

```csv
HostName
192.168.1.54
192.168.1.101
machine01
```   
   
## Credits

```text
  (*) For use of Castle Windsor (https://github.com/castleproject/Windsor) : trondr.OpTools uses Castle Windsor IOC container to couple components at runtime rather at development time. For more information about Inversion of Control (IOC) see http://en.wikipedia.org/wiki/Inversion_of_control
  (*) For use of NCmdLiner (https://github.com/trondr) : trondr.OpTools uses NCmdLiner to provide command line parsing and this automatic documentation. trondr.OpTools also utilize a copy of the ApplicationInfoHelper.cs class from NCmdLiner.
  (*) For use of NCmdLiner.SolutionTemplates (https://github.com/trondr) : trondr.OpTools uses NCmdLiner.SolutionTemplates to provide a start solution.
  (*) For use of Common.Logging (http://netcommon.sourceforge.net/index.html) : trondr.OpTools uses Common.Logging to provide a logging abstraction around log4net. This enables loose coupling to log4net as logging mechanism.
  (*) For use of Log4Net (http://logging.apache.org/log4net/) : trondr.OpTools uses log4net to provide industry standard logging.
  (*) For use of WindsorBootstrap (http://stackoverflow.com/questions/6358206/how-to-force-the-order-of-installer-execution) : trondr.OpTools uses stackoverflow answer by Jonas Stensved to provide prioritized execution of container installers.
  (*) For use of AutoMapper (https://github.com/AutoMapper) : trondr.OpTools uses AutoMapper to map between info objects and viewmodel objects.
  (*) For use of MvvmLightLibs (http://www.mvvmlight.net/) : trondr.OpTools uses Mvvm Light Toolkit components (ViewModelBase and IMessenger) to facilitate user interface development using MVVM.
  (*) For use of Json.NET (http://www.newtonsoft.com/json) : trondr.OpTools uses Json.NET to serialize objects to string representation, primarly for debug logging state of an object.
  (*) For use of RhinoMocks (https://github.com/meisinger/rhino-mocks/) : trondr.OpTools uses RhinoMocks in unit tests to create stub instances of dependent services not under test.
  (*) For use of Nunit (http://nunit.org/) : trondr.OpTools uses Nunit for faciliation of unit testing.
  (*) For use of Akka.NET (https://github.com/akkadotnet/akka.net) : trondr.OpTools uses Akka.Net to provide an actor model for concurrent and message driven operations.
  (*) For use of Language-Ext (https://github.com/louthy/language-ext) : trondr.OpTools uses Language-Ext to provide functional programming constructs.
  (*) For use of NConsoler (http://nconsoler.csharpus.com) : NCmdLiner is derived from NConsoler which implement self documenting commands using attribute on methods and method parameters.
  (*) For use of TinyIoC (https://github.com/grumpydev/TinyIoC) : NCmdLiner internally use TinyIoC to provide inversion of control.
``` 
   
## Licences

```text
  (*) trondr.OpTools, , New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause 
  (*) Castle Windsor, http://www.castleproject.org/, http://www.apache.org/licenses/LICENSE-2.0
  (*) NCmdLiner, https://github.com/trondr, New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause 
  (*) NCmdLiner.SolutionTemplates, https://github.com/trondr, New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause 
  (*) Common.Logging, http://netcommon.sourceforge.net/, Apache License, Version 2.0 http://www.apache.org/licenses/LICENSE-2.0
  (*) Log4Net, http://logging.apache.org/log4net/, Apache License, Version 2.0 http://www.apache.org/licenses/LICENSE-2.0
  (*) AutoMapper, https://github.com/AutoMapper/AutoMapper, MIT license http://opensource.org/licenses/MIT
  (*) Json.NET, http://www.newtonsoft.com/json, MIT License https://opensource.org/licenses/MIT 
  (*) RhinoMocks, https://github.com/meisinger/rhino-mocks/, New BSD License (BSD) https://raw.githubusercontent.com/meisinger/rhino-mocks/master/license.txt 
  (*) NUnit, http://nunit.org/, The MIT License http://nunit.org/nuget/nunit3-license.txt and the NUnit License http://nunit.org/nuget/license.html 
  (*) Akka.Net, https://github.com/akkadotnet/akka.net, Apache License 2.0 http://www.apache.org/licenses/LICENSE-2.0 
  (*) Language-Ext, https://github.com/louthy/language-ext, The MIT License (MIT)  https://raw.githubusercontent.com/louthy/language-ext/master/LICENSE.md 
  (*) NConsoler, http://nconsoler.csharpus.com, Mozilla Public License: http://www.mozilla.org/MPL
  (*) TinyIoC, https://github.com/grumpydev/TinyIoC, Microsoft Public License (Ms-PL): http://www.mozilla.org/MPL
```
   
## Minimum Build Requirements

* MS Build 15
* .NET Framework 4.5.2 Runtime (http://go.microsoft.com/fwlink/?LinkId=397674)
* .NET Framework 4.5.2 Developer Pack (http://go.microsoft.com/fwlink/?LinkId=328857)
* .NET Framework 2.0/3.5 (Install from Windows Features)
* Wix Toolset 3.11.1 (https://github.com/wixtoolset/wix3/releases)
