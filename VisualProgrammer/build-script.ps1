Param([string]$configName = "Debug")

$rootPath = $PSCommandPath + "\..\..\LibraryFiles"
$copyPath = $PSCommandPath + "\..\bin\" + $configName

# Function for performing recursive update
function RecursiveCopyUpdate ($rootPath, $folderName, $copyPath)
{
    if(!(Test-Path $copyPath\$folderName))
    {
        # Folder does not exists
        Copy-Item -Path $rootPath\$folderName -Destination $copyPath -Recurse -Force
    }
    else
    {
        # Get all subfolders and files
        Get-ChildItem $rootPath\$folderName |
        Foreach-Object {
            # Check if the object is another folder
            if(Test-Path $rootPath\$folderName\$_ -PathType Container)
            {
                # Folder, do recursive call
                RecursiveCopyUpdate -rootPath $rootPath\$folderName -folderName $_ -copyPath $copyPath\$folderName
            }
            else 
            {
                # Check if file exists
                if(Test-Path $copyPath\$folderName\$_)
                {
                    $file = Get-Item $copyPath\$folderName\$_

                    # Check if file is outdated and update
                    if($_.LastWriteTime -gt $file.LastWriteTime)
                    {
                        Copy-Item -Path $rootPath\$folderName\$_ -Destination $copyPath\$folderName
                    }
                }
                else  # File does not exist
                {
                    #Just copy the file to the copyPath
                    Copy-Item -Path $rootPath\$folderName\$_ -Destination $copyPath\$folderName
                }
            }
        }
    }
}

Write-Host "[#] Updating needed libary files..."

if(Test-Path $rootPath)
{
    RecursiveCopyUpdate -rootPath $rootPath -folderName Extended_Library -copyPath $copyPath
    RecursiveCopyUpdate -rootPath $rootPath -folderName Main_Library -copyPath $copyPath
    Write-Host "[#] Libary file update complete!"
}
else 
{
    # The needed Library files could not be found
    Write-Error "Could not find the needed library files, the folder LibraryFiles is missing." -Category ObjectNotFound
}