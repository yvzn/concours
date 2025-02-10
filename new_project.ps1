param (
    [Parameter(Mandatory=$true)]
    [string]$Name
)

$TemplatePath = ".\blank-project-net4.7"

Write-Host "Creating new project $Name from template $TemplatePath"
Copy-Item -Recurse -Force $TemplatePath $Name
Rename-Item -Path "$Name\blank-project-net4.7.sln" -NewName "$Name.sln"

$gitignorePath = "$Name\.gitignore"
$foldersToDelete = Get-Content $gitignorePath | Where-Object { $_ -match '.*/$' }

Get-ChildItem $Name -Recurse -Directory -include $($foldersToDelete) -Depth 4 `
    | Remove-Item -Recurse -Force



