[cmdletbinding(SupportsShouldProcess=$true)]
param($publishProperties=@{},$packOutput,$pubProfilePath)

#tolearnmoreaboutthisfilevisithttps://go.microsoft.com/fwlink/?LinkId=524327

try{
if($publishProperties['ProjectGuid']-eq$null){
$publishProperties['ProjectGuid']='1d00547d-f670-4722-8ca1-36419aafefe7'
}

$publishModulePath=Join-Path(Split-Path$MyInvocation.MyCommand.Path)'publish-module.psm1'
Import-Module$publishModulePath-DisableNameChecking-Force

#callPublish-AspNettoperformthepublishoperation
Publish-AspNet-publishProperties$publishProperties-packOutput$packOutput-pubProfilePath$pubProfilePath
}
catch{
"Anerroroccurredduringpublish.`n{0}"-f$_.Exception.Message|Write-Error
}