# Op Tools
=========================

github.com.trondr Op Tools

## Usage

```text
trondr OpTools 1.0.18022.2.1091f1a - github.com.trondr Op Tools
Copyright © github.com.trondr 2018
Author: trondr@outlook.com
Usage: trondr.OpTools.exe <command> [parameters]

Commands:
---------
Help                         Display this help text
License                      Display license
Credits                      Display credits
RunScript                    Run PowerShell script against all (or a random
                             sample of) targets in a host name list.

Commands and parameters:
------------------------
RunScript                    Run PowerShell script against all (or a random
                             sample of) targets in a host name list. The
                             online status (ping) of each host will be
                             checked before attempting to run the script
                             against a host. The script will run concurrently
                             to speed up the overall processing time. The
                             Powershell script should supports two input
                             parameters: The computer name or ip address of
                             the target host 'HostName' and the result path
                             'ResultFolderPath'. The powershell script is by
                             it self responsible for accessing the remote
                             host and uploading the result to the given
                             result path.
   /scriptPath               [Required] Path to the Powershell script.
                             Alternative parameter name: /pss
   /hostNameListCsv          [Required] Path to the csv file with list of
                             host names to run the Powershell script against.
                             Csv format: HostName. Alternative parameter
                             name: /hnl
   /resultFolderPath         [Required] Result folder path. Each script
                             execution can upload the result to this path.
                             Alternative parameter name: /rfp
   /samplePercent            [Optional] Specify a samplePercent less than 100
                             to run the script against a random sample of the
                             host names in the host list. A value of 100
                             (default) means the script will run against all
                             host names in the list. Alternative parameter
                             name: /sp. Default value: 100
   /resolveToIpv4Address     [Optional] Resolve host name to ip v4 address
                             before executing script. Alternative parameter
                             name: /rip. Default value: False
   /scriptExecutionParallelism[Optional] The number of concurrent script
                             executions. Alternative parameter name: /sep.
                             Default value: 10

   Example: trondr.OpTools.exe RunScript /scriptPath="c:\temp\test.ps1" /hostNameListCsv="c:\temp\hostnames.csv" /resultFolderPath="c:\temp" /samplePercent="100" /resolveToIpv4Address="False" /scriptExecutionParallelism="10" 
   Example (alternative): trondr.OpTools.exe RunScript /pss="c:\temp\test.ps1" /hnl="c:\temp\hostnames.csv" /rfp="c:\temp" /sp="100" /rip="False" /sep="10" 
```
   
## Minimum Build Requirements

* MS Build 15
* .NET Framework 4.5.2 Runtime (http://go.microsoft.com/fwlink/?LinkId=397674)
* .NET Framework 4.5.2 Developer Pack (http://go.microsoft.com/fwlink/?LinkId=328857)
* .NET Framework 2.0/3.5 (Install from Windows Features)
* Wix Toolset 3.11.1 (https://github.com/wixtoolset/wix3/releases)
