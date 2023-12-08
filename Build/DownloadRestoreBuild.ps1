
"Download Zybach"
& "$PSScriptRoot\DatabaseDownload.ps1" -iniFile "./build.ini" -secretsIniFile "./secrets.ini"

"Restore Zybach"
& "$PSScriptRoot\DatabaseRestore.ps1" -iniFile "./build.ini"

"Build Zybach"
& "$PSScriptRoot\DatabaseBuild.ps1" -iniFile "./build.ini"
