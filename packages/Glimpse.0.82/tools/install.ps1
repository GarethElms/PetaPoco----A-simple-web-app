﻿param($installPath, $toolsPath, $package, $project)

$path = [System.IO.Path]
$readmefile = $path::Combine($path::GetDirectoryName($project.FileName), "App_Readme\glimpse.readme.txt")
$DTE.ExecuteCommand("File.OpenFile", $readmefile)