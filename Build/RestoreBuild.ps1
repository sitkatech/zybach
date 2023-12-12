

"Restore Zybach"
& "$PSScriptRoot\DatabaseRestore.ps1"  -iniFile "./build.ini"

"Build Zybach"
& "$PSScriptRoot\DatabaseBuild.ps1" -iniFile "./build.ini"
