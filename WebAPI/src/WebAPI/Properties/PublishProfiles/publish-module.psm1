#WARNING:DONOTMODIFYthisfile.VisualStudiowilloverrideit.
param()

$script:AspNetPublishHandlers=@{}

<#
Thesesettingscanbeoverriddenwithenvironmentvariables.
Thenameoftheenvironmentvariableshoulduse"Publish"asa
prefixandthenamesbelow.Forexample:

$env:PublishMSDeployUseChecksum=$true
#>
$global:AspNetPublishSettings=New-Object-TypeNamePSCustomObject@{
MsdeployDefaultProperties=@{
'MSDeployUseChecksum'=$false
'SkipExtraFilesOnServer'=$true
'retryAttempts'=20
'EnableMSDeployBackup'=$false
'DeleteExistingFiles'=$false
'AllowUntrustedCertificate'=$false
'MSDeployPackageContentFoldername'='website\'
'EnvironmentName'='Production'
'AuthType'='Basic'
'MSDeployPublishMethod'='WMSVC'
}
}

functionInternalOverrideSettingsFromEnv{
[cmdletbinding()]
param(
[Parameter(Position=0)]
[object[]]$settings=($global:AspNetPublishSettings,$global:AspNetPublishSettings.MsdeployDefaultProperties),

[Parameter(Position=1)]
[string]$prefix='Publish'
)
process{
foreach($settingsObjin$settings){
if($settingsObj-eq$null){
continue
}

$settingNames=$null
if($settingsObj-is[hashtable]){
$settingNames=$settingsObj.Keys
}
else{
$settingNames=($settingsObj|Get-Member-MemberTypeNoteProperty|Select-Object-ExpandPropertyName)

}

foreach($namein@($settingNames)){
$fullname=('{0}{1}'-f$prefix,$name)
if(Test-Path"env:$fullname"){
$settingsObj.$name=((get-childitem"env:$fullname").Value)
}
}
}
}
}

InternalOverrideSettingsFromEnv-prefix'Publish'-settings$global:AspNetPublishSettings,$global:AspNetPublishSettings.MsdeployDefaultProperties

functionRegister-AspnetPublishHandler{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0)]
$name,
[Parameter(Mandatory=$true,Position=1)]
[ScriptBlock]$handler,
[switch]$force
)
process{
if(!($script:AspNetPublishHandlers[$name])-or$force){
'Addinghandlerfor[{0}]'-f$name|Write-Verbose
$script:AspNetPublishHandlers[$name]=$handler
}
elseif(!($force)){
'IgnoringcalltoRegister-AspnetPublishHandlerfor[name={0}],becauseahandlerwiththatnameexistsand-forcewasnotpassed.'-f$name|Write-Verbose
}
}
}

functionGet-AspnetPublishHandler{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0)]
$name
)
process{
$foundHandler=$script:AspNetPublishHandlers[$name]

if(!$foundHandler){
throw('AspnetPublishHandlerwithname"{0}"wasnotfound'-f$name)
}

$foundHandler
}
}

functionGetInternal-ExcludeFilesArg{
[cmdletbinding()]
param(
$publishProperties
)
process{
$excludeFiles=$publishProperties['ExcludeFiles']
foreach($excludein$excludeFiles){
if($exclude){
[string]$objName=$exclude['objectname']

if([string]::IsNullOrEmpty($objName)){
$objName='filePath'
}

$excludePath=$exclude['absolutepath']

#outputtheresulttothereturnlist
('-skip:objectName={0},absolutePath=''{1}'''-f$objName,$excludePath)
}
}
}
}

functionGetInternal-ReplacementsMSDeployArgs{
[cmdletbinding()]
param(
$publishProperties
)
process{
foreach($replacein($publishProperties['Replacements'])){
if($replace){
$typeValue=$replace['type']
if(!$typeValue){$typeValue='TextFile'}

$file=$replace['file']
$match=$replace['match']
$newValue=$replace['newValue']

if($file-and$match-and$newValue){
$setParam=('-setParam:type={0},scope={1},match={2},value={3}'-f$typeValue,$file,$match,$newValue)
'Addingsetparam[{0}]'-f$setParam|Write-Verbose

#returnit
$setParam
}
else{
'Skippingreplacementbecauseitsmissingarequiredvalue.[file="{0}",match="{1}",newValue="{2}"]'-f$file,$match,$newValue|Write-Verbose
}
}
}
}
}

<#
.SYNOPSIS
Returnsanarrayofmsdeployargumentsthatareusedacrossdifferentproviders.
ForexamplethiswillhandleuseChecksum,AppOfflineetc.
Thiswillalsoadddefaultpropertiesiftheyaremissing.
#>
functionGetInternal-SharedMSDeployParametersFrom{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0)]
[HashTable]$publishProperties,
[Parameter(Mandatory=$true,Position=1)]
[System.IO.FileInfo]$packOutput
)
process{
$sharedArgs=New-Objectpsobject-Property@{
ExtraArgs=@()
DestFragment=''
EFMigrationData=@{}
}

#adddefaultpropertiesiftheyaremissing
foreach($propNamein$global:AspNetPublishSettings.MsdeployDefaultProperties.Keys){
if($publishProperties["$propName"]-eq$null){
$defValue=$global:AspNetPublishSettings.MsdeployDefaultProperties["$propName"]
'AddingdefaultpropertytopublishProperties["{0}"="{1}"]'-f$propName,$defValue|Write-Verbose
$publishProperties["$propName"]=$defValue
}
}

if($publishProperties['MSDeployUseChecksum']-eq$true){
$sharedArgs.ExtraArgs+='-usechecksum'
}

if($publishProperties['EnableMSDeployAppOffline']-eq$true){
$sharedArgs.ExtraArgs+='-enablerule:AppOffline'
}

if($publishProperties['WebPublishMethod']-eq'MSDeploy'){
if($publishProperties['SkipExtraFilesOnServer']-eq$true){
$sharedArgs.ExtraArgs+='-enableRule:DoNotDeleteRule'
}
}

if($publishProperties['WebPublishMethod']-eq'FileSystem'){
if($publishProperties['DeleteExistingFiles']-eq$false){
$sharedArgs.ExtraArgs+='-enableRule:DoNotDeleteRule'
}
}

if($publishProperties['retryAttempts']){
$sharedArgs.ExtraArgs+=('-retryAttempts:{0}'-f([int]$publishProperties['retryAttempts']))
}

if($publishProperties['EncryptWebConfig']-eq$true){
$sharedArgs.ExtraArgs+='-EnableRule:EncryptWebConfig'
}

if($publishProperties['EnableMSDeployBackup']-eq$false){
$sharedArgs.ExtraArgs+='-disablerule:BackupRule'
}

if($publishProperties['AllowUntrustedCertificate']-eq$true){
$sharedArgs.ExtraArgs+='-allowUntrusted'
}

#addexcludes
$sharedArgs.ExtraArgs+=(GetInternal-ExcludeFilesArg-publishProperties$publishProperties)
#addreplacements
$sharedArgs.ExtraArgs+=(GetInternal-ReplacementsMSDeployArgs-publishProperties$publishProperties)

#addEFMigration
if(($publishProperties['EfMigrations']-ne$null)-and$publishProperties['EfMigrations'].Count-gt0){
if(!(Test-Path-Path$publishProperties['ProjectPath'])){
throw'ProjectPathpropertyneedstobedefinedinthepubxmlforEFmigration.'
}
try{
#generateT-SQLfiles
$EFSqlFiles=GenerateInternal-EFMigrationScripts-projectPath$publishProperties['ProjectPath']-packOutput$packOutput-EFMigrations$publishProperties['EfMigrations']
$sharedArgs.EFMigrationData.Add('EFSqlFiles',$EFSqlFiles)
}
catch{
throw('AnerroroccurredwhilegeneratingEFmigrations.{0}{1}'-f$_.Exception,(Get-PSCallStack))
}
}
#addconnectionstringupdate
if(($publishProperties['DestinationConnectionStrings']-ne$null)-and$publishProperties['DestinationConnectionStrings'].Count-gt0){
try{
#create/updateappsettings.[environment].json
GenerateInternal-AppSettingsFile-packOutput$packOutput-environmentName$publishProperties['EnvironmentName']-connectionStrings$publishProperties['DestinationConnectionStrings']
}
catch{
throw('Anerroroccurredwhilegeneratingthepublishappsettingsfile.{0}{1}'-f$_.Exception,(Get-PSCallStack))
}
}

if(-not[string]::IsNullOrWhiteSpace($publishProperties['ProjectGuid'])){
AddInternal-ProjectGuidToWebConfig-publishProperties$publishProperties-packOutput$packOutput
}

#returntheargs
$sharedArgs
}
}

<#
.SYNOPSIS
Thiswillpublishthefolderbasedonthepropertiesin$publishProperties

.PARAMETERpublishProperties
Thisisahashtablecontainingthepublishproperties.Seetheexampleshereformoreinfoonhowtousethisparameter.

.PARAMETERpackOutput
Thefolderpathtotheoutputofthednupublishcommand.Thisfoldercontainsthefiles
thatwillbepublished.

.PARAMETERpubProfilePath
Pathtoapublishprofile(.pubxmlfile)toimportpublishpropertiesfrom.Ifthesamepropertyexistsin
publishPropertiesandthepublishprofilethenpublishPropertieswillwin.

.EXAMPLE
Publish-AspNet-packOutput$packOutput-publishProperties@{
'WebPublishMethod'='MSDeploy'
'MSDeployServiceURL'='contoso.scm.azurewebsites.net:443';`
'DeployIisAppPath'='contoso';'Username'='$contoso';'Password'="$env:PublishPwd"}

.EXAMPLE
Publish-AspNet-packOutput$packOutput-publishProperties@{
'WebPublishMethod'='FileSystem'
'publishUrl'="$publishDest"
}

.EXAMPLE
Publish-AspNet-packOutput$packOutput-publishProperties@{
'WebPublishMethod'='MSDeploy'
'MSDeployServiceURL'='contoso.scm.azurewebsites.net:443';`
'DeployIisAppPath'='contoso';'Username'='$contoso';'Password'="$env:PublishPwd"
'ExcludeFiles'=@(
@{'absolutepath'='test.txt'},
@{'absolutepath'='references.js'}
)}

.EXAMPLE
Publish-AspNet-packOutput$packOutput-publishProperties@{
'WebPublishMethod'='FileSystem'
'publishUrl'="$publishDest"
'ExcludeFiles'=@(
@{'absolutepath'='test.txt'},
@{'absolutepath'='_references.js'})
'Replacements'=@(
@{'file'='test.txt$';'match'='REPLACEME';'newValue'='updatedValue'})
}

Publish-AspNet-packOutput$packOutput-publishProperties@{
'WebPublishMethod'='FileSystem'
'publishUrl'="$publishDest"
'ExcludeFiles'=@(
@{'absolutepath'='test.txt'},
@{'absolutepath'='c:\\full\\path\\ok\\as\\well\\_references.js'})
'Replacements'=@(
@{'file'='test.txt$';'match'='REPLACEME';'newValue'='updatedValue'})
}

.EXAMPLE
Publish-AspNet-packOutput$packOutput-publishProperties@{
'WebPublishMethod'='FileSystem'
'publishUrl'="$publishDest"
'EnableMSDeployAppOffline'='true'
'AppOfflineTemplate'='offline-template.html'
'MSDeployUseChecksum'='true'
}
#>
functionPublish-AspNet{
param(
[Parameter(Position=0,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
[hashtable]$publishProperties=@{},

[Parameter(Mandatory=$true,Position=1,ValueFromPipelineByPropertyName=$true)]
[System.IO.FileInfo]$packOutput,

[Parameter(Position=2,ValueFromPipelineByPropertyName=$true)]
[System.IO.FileInfo]$pubProfilePath
)
process{
if($publishProperties['WebPublishMethodOverride']){
'Overridingpublishmethodfrom$publishProperties[''WebPublishMethodOverride'']to[{0}]'-f($publishProperties['WebPublishMethodOverride'])|Write-Verbose
$publishProperties['WebPublishMethod']=$publishProperties['WebPublishMethodOverride']
}

if(-not[string]::IsNullOrWhiteSpace($pubProfilePath)){
$profileProperties=Get-PropertiesFromPublishProfile-filepath$pubProfilePath
foreach($keyin$profileProperties.Keys){
if(-not($publishProperties.ContainsKey($key))){
'Addingpropertiesfrompublishprofile[''{0}''=''{1}'']'-f$key,$profileProperties[$key]|Write-Verbose
$publishProperties.Add($key,$profileProperties[$key])
}
}
}

if(!([System.IO.Path]::IsPathRooted($packOutput))){
$packOutput=[System.IO.Path]::GetFullPath((Join-Path$pwd$packOutput))
}

$pubMethod=$publishProperties['WebPublishMethod']
'Publishingwithpublishmethod[{0}]'-f$pubMethod|Write-Output

#getthehandlerbasedonWebPublishMethod,andcallit.
&(Get-AspnetPublishHandler-name$pubMethod)$publishProperties$packOutput
}
}

<#
.SYNOPSIS

Inputs:

Exampleof$xmlDocument:'<?xmlversion="1.0"encoding="utf-8"?><sitemanifest></sitemanifest>'
Exampleof$providerDataArray:

[System.Collections.ArrayList]$providerDataArray=@()

$iisAppSourceKeyValue=@{"iisApp"=@{"path"='c:\temp\pathtofiles';"appOfflineTemplate"='offline-template.html'}}
$providerDataArray.Add($iisAppSourceKeyValue)

$dbfullsqlKeyValue=@{"dbfullsql"=@{"path"="c:\Temp\PathToSqlFile"}}
$providerDataArray.Add($dbfullsqlKeyValue)

$dbfullsqlKeyValue=@{"dbfullsql"=@{"path"="c:\Temp\PathToSqlFile2"}}
$providerDataArray.Add($dbfullsqlKeyValue)

ManifestFilecontent:
<?xmlversion="1.0"encoding="utf-8"?>
<sitemanifest>
<iisApppath="c:\temp\pathtofiles"appOfflineTemplate=�offline-template.html"/>
<dbFullSqlpath="c:\Temp\PathToSqlFile"/>
<dbFullSqlpath="c:\Temp\PathToSqlFile2"/>
</sitemanifest>
#>
functionAddInternal-ProviderDataToManifest{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0)]
[XML]$xmlDocument,
[Parameter(Position=1)]
[System.Collections.ArrayList]$providerDataArray
)
process{
$siteNode=$xmlDocument.SelectSingleNode("/sitemanifest")
if($siteNode-eq$null){
throw'sitemanifestelementismissinginthexmlobject'
}
foreach($providerDatain$providerDataArray){
foreach($providerNamein$providerData.Keys){
$providerValue=$providerData[$providerName]
$xmlNode=$xmlDocument.CreateElement($providerName)
foreach($providerValueKeyin$providerValue.Keys){
$xmlNode.SetAttribute($providerValueKey,$providerValue[$providerValueKey])|Out-Null
}
$siteNode.AppendChild($xmlNode)|Out-Null
}
}
}
}

functionAddInternal-ProjectGuidToWebConfig{
[cmdletbinding()]
param(
[Parameter(Position=0)]
[HashTable]$publishProperties,
[Parameter(Position=1)]
[System.IO.FileInfo]$packOutput
)
process{
try{
[Reflection.Assembly]::LoadWithPartialName("System.Xml.Linq")|Out-Null
$webConfigPath=Join-Path$packOutput'web.config'
$projectGuidCommentValue='ProjectGuid:{0}'-f$publishProperties['ProjectGuid']
$xDoc=[System.Xml.Linq.XDocument]::Load($webConfigPath)
$allNodes=$xDoc.DescendantNodes()
$projectGuidComment=$allNodes|Where-Object{$_.NodeType-eq[System.Xml.XmlNodeType]::Comment-and$_.Value-eq$projectGuidCommentValue}|Select-First1
if($projectGuidComment-ne$null){
if($publishProperties['IgnoreProjectGuid']-eq$true){
$projectGuidComment.Remove()|Out-Null
$xDoc.Save($webConfigPath)|Out-Null
}
}
else{
if(-not($publishProperties['IgnoreProjectGuid']-eq$true)){
$projectGuidComment=New-Object-TypeNameSystem.Xml.Linq.XComment-ArgumentList$projectGuidCommentValue
$xDoc.LastNode.AddAfterSelf($projectGuidComment)|Out-Null
$xDoc.Save($webConfigPath)|Out-Null
}
}
}
catch{
}
}
}

<#
.SYNOPSIS

Exampleof$EFMigrations:
$EFMigrations=@{'CarContext'='CarContextConnectionString';'MovieContext'='MovieContextConnectionString'}

#>

functionGenerateInternal-EFMigrationScripts{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0)]
[System.IO.FileInfo]$projectPath,
[Parameter(Mandatory=$true,Position=1)]
[System.IO.FileInfo]$packOutput,
[Parameter(Position=2)]
[HashTable]$EFMigrations
)
process{
$files=@{}
$dotnetExePath=GetInternal-DotNetExePath
foreach($dbContextNamein$EFMigrations.Keys){
try
{
$tempDir=GetInternal-PublishTempPath-packOutput$packOutput
$efScriptFile=Join-Path$tempDir('{0}.sql'-f$dbContextName)
$arg=('efmigrationsscript--idempotent--output{0}--context{1}'-f
$efScriptFile,
$dbContextName)

Execute-Command$dotnetExePath$arg$projectPath|Out-Null
if(Test-Path-Path$efScriptFile){
if(!($files.ContainsKey($dbContextName))){
$files.Add($dbContextName,$efScriptFile)|Out-Null
}
}
}
catch
{
throw'erroroccuredwhenexecutingdotnet.exetogenerateEFT-SQLfile'
}
}
#returnfilesobject
$files
}
}

<#
.SYNOPSIS

Exampleof$connectionStrings:
$connectionStrings=@{'DefaultConnection'='DefaultConnectionString';'CarConnection'='CarConnectionString'}

#>
functionGenerateInternal-AppSettingsFile{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0)]
[System.IO.FileInfo]$packOutput,
[Parameter(Mandatory=$true,Position=1)]
[string]$environmentName,
[Parameter(Position=2)]
[HashTable]$connectionStrings
)
process{
$configProdJsonFile='appsettings.{0}.json'-f$environmentName
$configProdJsonFilePath=Join-Path-Path$packOutput-ChildPath$configProdJsonFile

if([string]::IsNullOrEmpty($configProdJsonFilePath)){
throw('Thepathof{0}isempty'-f$configProdJsonFilePath)
}

if(!(Test-Path-Path$configProdJsonFilePath)){
#createnewfile
'{}'|out-file-encodingutf8-filePath$configProdJsonFilePath-Force
}

$jsonObj=ConvertFrom-Json-InputObject(Get-Content-Path$configProdJsonFilePath-Raw)
#updatewhenthereexistsoneormoreconnectionstrings
if($connectionStrings-ne$null){
foreach($namein$connectionStrings.Keys){
#checkforhierarchystyle
if($jsonObj.ConnectionStrings.$name){
$jsonObj.ConnectionStrings.$name=$connectionStrings[$name]
continue
}
#checkforhorizontalstyle
$horizontalName='ConnectionStrings.{0}:'-f$name
if($jsonObj.$horizontalName){
$jsonObj.$horizontalName=$connectionStrings[$name]
continue
}
#createnewone
if(!($jsonObj.ConnectionStrings)){
$contentForDefaultConnection='{}'
$jsonObj|Add-Member-name'ConnectionStrings'-value(ConvertFrom-Json-InputObject$contentForDefaultConnection)-MemberTypeNoteProperty|Out-Null
}
if(!($jsonObj.ConnectionStrings.$name)){
$jsonObj.ConnectionStrings|Add-Member-name$name-value$connectionStrings[$name]-MemberTypeNoteProperty|Out-Null
}
}
}

$jsonObj|ConvertTo-Json|out-file-encodingutf8-filePath$configProdJsonFilePath-Force

#returnthepathofconfig.[environment].json
$configProdJsonFilePath
}
}

<#
.SYNOPSIS

Inputs:
Exampleof$providerDataArray:

[System.Collections.ArrayList]$providerDataArray=@()

$iisAppSourceKeyValue=@{"iisApp"=@{"path"='c:\temp\pathtofiles';"appOfflineTemplate"='offline-template.html'}}
$providerDataArray.Add($iisAppSourceKeyValue)

$dbfullsqlKeyValue=@{"dbfullsql"=@{"path"="c:\Temp\PathToSqlFile"}}
$providerDataArray.Add($dbfullsqlKeyValue)

$dbfullsqlKeyValue=@{"dbfullsql"=@{"path"="c:\Temp\PathToSqlFile2"}}
$providerDataArray.Add($dbfullsqlKeyValue)

ManifestFilecontent:
<?xmlversion="1.0"encoding="utf-8"?>
<sitemanifest>
<iisApppath="c:\temp\pathtofiles"appOfflineTemplate=�offline-template.html"/>
<dbFullSqlpath="c:\Temp\PathToSqlFile"/>
<dbFullSqlpath="c:\Temp\PathToSqlFile2"/>
</sitemanifest>

#>

functionGenerateInternal-ManifestFile{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0)]
[System.IO.FileInfo]$packOutput,
[Parameter(Mandatory=$true,Position=1)]
$publishProperties,
[Parameter(Mandatory=$true,Position=2)]
[System.Collections.ArrayList]$providerDataArray,
[Parameter(Mandatory=$true,Position=3)]
[ValidateNotNull()]
$manifestFileName
)
process{
$xmlDocument=[xml]'<?xmlversion="1.0"encoding="utf-8"?><sitemanifest></sitemanifest>'
AddInternal-ProviderDataToManifest-xmlDocument$xmlDocument-providerDataArray$providerDataArray|Out-Null
$publishTempDir=GetInternal-PublishTempPath-packOutput$packOutput
$XMLFile=Join-Path$publishTempDir$manifestFileName
$xmlDocument.OuterXml|out-file-encodingutf8-filePath$XMLFile-Force

#return
[System.IO.FileInfo]$XMLFile
}
}

functionGetInternal-PublishTempPath{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0)]
[System.IO.FileInfo]$packOutput
)
process{
$tempDir=[io.path]::GetTempPath()
$packOutputFolderName=Split-Path$packOutput-Leaf
$publishTempDir=[io.path]::combine($tempDir,'PublishTemp','obj',$packOutputFolderName)
if(!(Test-Path-Path$publishTempDir)){
New-Item-Path$publishTempDir-typedirectory|Out-Null
}
#return
[System.IO.FileInfo]$publishTempDir
}
}

functionPublish-AspNetMSDeploy{
param(
[Parameter(Mandatory=$true,Position=0,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
$publishProperties,
[Parameter(Mandatory=$true,Position=1,ValueFromPipelineByPropertyName=$true)]
$packOutput
)
process{
if($publishProperties){
$publishPwd=$publishProperties['Password']

$sharedArgs=GetInternal-SharedMSDeployParametersFrom-publishProperties$publishProperties-packOutput$packOutput
$iisAppPath=$publishProperties['DeployIisAppPath']

#createsourcemanifest

#e.g
#<?xmlversion="1.0"encoding="utf-8"?>
#<sitemanifest>
#<iisApppath="C:\Temp\PublishTemp\WebApplication\"appOfflineTemplate=�offline-template.html"/>
#<dbFullSqlpath="C:\Temp\PublishTemp\obj\WebApplication.Data.ApplicationDbContext.sql"/>
#<dbFullSqlpath="C:\Temp\PublishTemp\obj\WebApplication.Data.CarContext.sql"/>
#</sitemanifest>

[System.Collections.ArrayList]$providerDataArray=@()
$iisAppValues=@{"path"=$packOutput};
$iisAppSourceKeyValue=@{"iisApp"=$iisAppValues}
$providerDataArray.Add($iisAppSourceKeyValue)|Out-Null

if($sharedArgs.EFMigrationData-ne$null-and$sharedArgs.EFMigrationData.Contains('EFSqlFiles')){
foreach($sqlFilein$sharedArgs.EFMigrationData['EFSqlFiles'].Values){
$dbFullSqlSourceKeyValue=@{"dbFullSql"=@{"path"=$sqlFile}}
$providerDataArray.Add($dbFullSqlSourceKeyValue)|Out-Null
}
}

[System.IO.FileInfo]$sourceXMLFile=GenerateInternal-ManifestFile-packOutput$packOutput-publishProperties$publishProperties-providerDataArray$providerDataArray-manifestFileName'SourceManifest.xml'

$providerDataArray.Clear()|Out-Null
#createdestinationmanifest

#e.g
#<?xmlversion="1.0"encoding="utf-8"?>
#<sitemanifest><iisApppath="WebApplication8020160609015407"/>
#<dbFullSqlpath="DataSource=tcp:webapplicationdbserver.database.windows.net,1433;InitialCatalog=WebApplication_db;UserId=sqladmin@webapplicationdbserver;Password=<password>"/>
#<dbFullSqlpath="DataSource=tcp:webapplicationdbserver.database.windows.net,1433;InitialCatalog=WebApplication_db;UserId=sqladmin@webapplicationdbserver;Password=<password>"/>
#</sitemanifest>

$iisAppValues=@{"path"=$iisAppPath};
if(-not[string]::IsNullOrWhiteSpace($publishProperties['AppOfflineTemplate'])){
$iisAppValues.Add("appOfflineTemplate",$publishProperties['AppOfflineTemplate'])|Out-Null
}

$iisAppDestinationKeyValue=@{"iisApp"=$iisAppValues}
$providerDataArray.Add($iisAppDestinationKeyValue)|Out-Null

if($publishProperties['EfMigrations']-ne$null-and$publishProperties['EfMigrations'].Count-gt0){
foreach($connectionStringin$publishProperties['EfMigrations'].Values){
$dbFullSqlDestinationKeyValue=@{"dbFullSql"=@{"path"=$connectionString}}
$providerDataArray.Add($dbFullSqlDestinationKeyValue)|Out-Null
}
}


[System.IO.FileInfo]$destXMLFile=GenerateInternal-ManifestFile-packOutput$packOutput-publishProperties$publishProperties-providerDataArray$providerDataArray-manifestFileName'DestinationManifest.xml'

<#
"C:\ProgramFiles(x86)\IIS\MicrosoftWebDeployV3\msdeploy.exe"
-source:manifest='C:\Users\testuser\AppData\Local\Temp\PublishTemp\obj\SourceManifest.xml'
-dest:manifest='C:\Users\testuser\AppData\Local\Temp\PublishTemp\obj\DestManifest.xml',ComputerName='https://contoso.scm.azurewebsites.net/msdeploy.axd',UserName='$contoso',Password='<PWD>',IncludeAcls='False',AuthType='Basic'
-verb:sync
-enableRule:DoNotDeleteRule
-retryAttempts=2"
#>

if(-not[string]::IsNullOrWhiteSpace($publishProperties['MSDeployPublishMethod'])){
$serviceMethod=$publishProperties['MSDeployPublishMethod']
}

$msdeployComputerName=InternalNormalize-MSDeployUrl-serviceUrl$publishProperties['MSDeployServiceURL']-siteName$iisAppPath-serviceMethod$publishProperties['MSDeployPublishMethod']
if($publishProperties['UseMSDeployServiceURLAsIs']-eq$true){
$msdeployComputerName=$publishProperties['MSDeployServiceURL']
}

$publishArgs=@()
#usemanifesttopublish
$publishArgs+=('-source:manifest=''{0}'''-f$sourceXMLFile.FullName)
$publishArgs+=('-dest:manifest=''{0}'',ComputerName=''{1}'',UserName=''{2}'',Password=''{3}'',IncludeAcls=''False'',AuthType=''{4}''{5}'-f
$destXMLFile.FullName,
$msdeployComputerName,
$publishProperties['UserName'],
$publishPwd,
$publishProperties['AuthType'],
$sharedArgs.DestFragment)
$publishArgs+='-verb:sync'
$publishArgs+=$sharedArgs.ExtraArgs

$command='"{0}"{1}'-f(Get-MSDeploy),($publishArgs-join'')

if(![String]::IsNullOrEmpty($publishPwd)){
$command.Replace($publishPwd,'{PASSWORD-REMOVED-FROM-LOG}')|Print-CommandString
}
Execute-Command-exePath(Get-MSDeploy)-arguments($publishArgs-join'')
}
else{
throw'publishPropertiesisempty,cannotpublish'
}
}
}

functionEscape-TextForRegularExpressions{
[cmdletbinding()]
param(
[Parameter(Position=0,Mandatory=$true)]
[string]$text
)
process{
[regex]::Escape($text)
}
}

functionPublish-AspNetMSDeployPackage{
param(
[Parameter(Mandatory=$true,Position=0,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
$publishProperties,
[Parameter(Mandatory=$true,Position=1,ValueFromPipelineByPropertyName=$true)]
$packOutput
)
process{
if($publishProperties){
$packageDestinationFilepath=$publishProperties['DesktopBuildPackageLocation']

if(!$packageDestinationFilepath){
throw('Thepackagedestinationproperty(DesktopBuildPackageLocation)wasnotfoundinthepublishproperties')
}

if(!([System.IO.Path]::IsPathRooted($packageDestinationFilepath))){
$packageDestinationFilepath=[System.IO.Path]::GetFullPath((Join-Path$pwd$packageDestinationFilepath))
}

#ifthedirdoesn'texistcreateit
$pkgDir=((new-object-typenameSystem.IO.FileInfo($packageDestinationFilepath)).Directory)
if(!(Test-Path-Path$pkgDir)){
New-Item$pkgDir-typeDirectory|Out-Null
}

<#
"C:\ProgramFiles(x86)\IIS\MicrosoftWebDeployV3\msdeploy.exe"
-source:manifest='C:\Users\testuser\AppData\Local\Temp\PublishTemp\obj\SourceManifest.xml'
-dest:package=c:\temp\path\contosoweb.zip
-verb:sync
-enableRule:DoNotDeleteRule
-retryAttempts=2
#>

$sharedArgs=GetInternal-SharedMSDeployParametersFrom-publishProperties$publishProperties-packOutput$packOutput

#createsourcemanifest

#e.g
#<?xmlversion="1.0"encoding="utf-8"?>
#<sitemanifest>
#<iisApppath="C:\Temp\PublishTemp\WebApplication\"/>
#</sitemanifest>

[System.Collections.ArrayList]$providerDataArray=@()
$iisAppSourceKeyValue=@{"iisApp"=@{"path"=$packOutput}}
$providerDataArray.Add($iisAppSourceKeyValue)|Out-Null

[System.IO.FileInfo]$sourceXMLFile=GenerateInternal-ManifestFile-packOutput$packOutput-publishProperties$publishProperties-providerDataArray$providerDataArray-manifestFileName'SourceManifest.xml'

$publishArgs=@()
$publishArgs+=('-source:manifest=''{0}'''-f$sourceXMLFile.FullName)
$publishArgs+=('-dest:package=''{0}'''-f$packageDestinationFilepath)
$publishArgs+='-verb:sync'
$packageContentFolder=$publishProperties['MSDeployPackageContentFoldername']
if(!$packageContentFolder){$packageContentFolder='website'}
$publishArgs+=('-replace:match=''{0}'',replace=''{1}'''-f(Escape-TextForRegularExpressions$packOutput),$packageContentFolder)
$publishArgs+=$sharedArgs.ExtraArgs

$command='"{0}"{1}'-f(Get-MSDeploy),($publishArgs-join'')
$command|Print-CommandString
Execute-Command-exePath(Get-MSDeploy)-arguments($publishArgs-join'')
}
else{
throw'publishPropertiesisempty,cannotpublish'
}
}
}

functionPublish-AspNetFileSystem{
param(
[Parameter(Mandatory=$true,Position=0,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
$publishProperties,
[Parameter(Mandatory=$true,Position=1,ValueFromPipelineByPropertyName=$true)]
$packOutput
)
process{
$pubOut=$publishProperties['publishUrl']

if([string]::IsNullOrWhiteSpace($pubOut)){
throw('publishUrlisarequiredpropertyforFileSystempublishbutitwasempty.')
}

#ifit'sarelativepaththenupdateittoafullpath
if(!([System.IO.Path]::IsPathRooted($pubOut))){
$pubOut=[System.IO.Path]::GetFullPath((Join-Path$pwd$pubOut))
$publishProperties['publishUrl']="$pubOut"
}

'Publishingfilesto{0}'-f$pubOut|Write-Output

#weusemsdeploy.exebecauseitsupportsincrementalpublish/skips/replacements/etc
#msdeploy.exe-verb:sync-source:manifest='C:\Users\testuser\AppData\Local\Temp\PublishTemp\obj\SourceManifest.xml'-dest:manifest='C:\Users\testuser\AppData\Local\Temp\PublishTemp\obj\DestManifest.xml'

$sharedArgs=GetInternal-SharedMSDeployParametersFrom-publishProperties$publishProperties-packOutput$packOutput

#createsourcemanifest

#e.g
#<?xmlversion="1.0"encoding="utf-8"?>
#<sitemanifest>
#<contentPathpath="C:\Temp\PublishTemp\WebApplication\"appOfflineTemplate=�offline-template.html"/>
#</sitemanifest>

[System.Collections.ArrayList]$providerDataArray=@()
$contentPathValues=@{"path"=$packOutput};
$contentPathSourceKeyValue=@{"contentPath"=$contentPathValues}
$providerDataArray.Add($contentPathSourceKeyValue)|Out-Null

[System.IO.FileInfo]$sourceXMLFile=GenerateInternal-ManifestFile-packOutput$packOutput-publishProperties$publishProperties-providerDataArray$providerDataArray-manifestFileName'SourceManifest.xml'

$providerDataArray.Clear()|Out-Null
#createdestinationmanifest

#e.g
#<?xmlversion="1.0"encoding="utf-8"?>
#<sitemanifest><contentPathpath="C:\Temp\PublishTemp\WebApplicationDestination\"/>
#</sitemanifest>
$contentPathValues=@{"path"=$publishProperties['publishUrl']};
if(-not[string]::IsNullOrWhiteSpace($publishProperties['AppOfflineTemplate'])){
$contentPathValues.Add("appOfflineTemplate",$publishProperties['AppOfflineTemplate'])|Out-Null
}
$contentPathDestinationKeyValue=@{"contentPath"=$contentPathValues}
$providerDataArray.Add($contentPathDestinationKeyValue)|Out-Null

[System.IO.FileInfo]$destXMLFile=GenerateInternal-ManifestFile-packOutput$packOutput-publishProperties$publishProperties-providerDataArray$providerDataArray-manifestFileName'DestinationManifest.xml'

$publishArgs=@()
$publishArgs+=('-source:manifest=''{0}'''-f$sourceXMLFile.FullName)
$publishArgs+=('-dest:manifest=''{0}''{1}'-f$destXMLFile.FullName,$sharedArgs.DestFragment)
$publishArgs+='-verb:sync'
$publishArgs+=$sharedArgs.ExtraArgs

$command='"{0}"{1}'-f(Get-MSDeploy),($publishArgs-join'')
$command|Print-CommandString
Execute-Command-exePath(Get-MSDeploy)-arguments($publishArgs-join'')

#copysqlscripttoscriptfolder
if(($sharedArgs.EFMigrationData['EFSqlFiles']-ne$null)-and($sharedArgs.EFMigrationData['EFSqlFiles'].Count-gt0)){
$scriptsDir=Join-Path$pubOut'efscripts'

if(!(Test-Path-Path$scriptsDir)){
New-Item-Path$scriptsDir-typedirectory|Out-Null
}

foreach($sqlFilein$sharedArgs.EFMigrationData['EFSqlFiles'].Values){
Copy-Item$sqlFile-Destination$scriptsDir-Force-Recurse|Out-Null
}
}
}
}

<#
.SYNOPSIS
Thiscanbeusedtoreadapublishprofiletoextractthepropertyvaluesintoahashtable.

.PARAMETERfilepath
Pathtothepublishprofiletogetthepropertiesfrom.Currenltythisonlysupportsreading
.pubxmlfiles.

.EXAMPLE
Get-PropertiesFromPublishProfile-filepathc:\projects\publish\devpublish.pubxml
#>
functionGet-PropertiesFromPublishProfile{
[cmdletbinding()]
param(
[Parameter(Position=0,Mandatory=$true)]
[ValidateNotNull()]
[ValidateScript({Test-Path$_})]
[System.IO.FileInfo]$filepath
)
begin{
Add-Type-AssemblyNameSystem.Core
Add-Type-AssemblyNameMicrosoft.Build
}
process{
'Readingpublishpropertiesfromprofile[{0}]'-f$filepath|Write-Verbose
#useMSBuildtogettheprojectandreadproperties
$projectCollection=(New-ObjectMicrosoft.Build.Evaluation.ProjectCollection)
if(!([System.IO.Path]::IsPathRooted($filepath))){
$filepath=[System.IO.Path]::GetFullPath((Join-Path$pwd$filepath))
}
$project=([Microsoft.Build.Construction.ProjectRootElement]::Open([string]$filepath.Fullname,$projectCollection))

$properties=@{}
foreach($propertyin$project.Properties){
$properties[$property.Name]=$property.Value
}

$properties
}
}

functionPrint-CommandString{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0,ValueFromPipeline=$true)]
$command
)
process{
'Executingcommand[{0}]'-f$command|Write-Output
}
}

functionExecute-CommandString{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0,ValueFromPipeline=$true)]
[string[]]$command,

[switch]
$useInvokeExpression,

[switch]
$ignoreErrors
)
process{
foreach($cmdToExecin$command){
'Executingcommand[{0}]'-f$cmdToExec|Write-Verbose
if($useInvokeExpression){
try{
Invoke-Expression-Command$cmdToExec
}
catch{
if(-not$ignoreErrors){
$msg=('Thecommand[{0}]exitedwithexception[{1}]'-f$cmdToExec,$_.ToString())
throw$msg
}
}
}
else{
cmd.exe/D/C$cmdToExec

if(-not$ignoreErrors-and($LASTEXITCODE-ne0)){
$msg=('Thecommand[{0}]exitedwithcode[{1}]'-f$cmdToExec,$LASTEXITCODE)
throw$msg
}
}
}
}
}

functionExecute-Command{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
[String]$exePath,
[Parameter(Mandatory=$true,Position=1,ValueFromPipelineByPropertyName=$true)]
[String]$arguments,
[Parameter(Position=2)]
[System.IO.FileInfo]$workingDirectory
)
process{
$psi=New-Object-TypeNameSystem.Diagnostics.ProcessStartInfo
$psi.CreateNoWindow=$true
$psi.UseShellExecute=$false
$psi.RedirectStandardOutput=$true
$psi.RedirectStandardError=$true
$psi.FileName=$exePath
$psi.Arguments=$arguments
if($workingDirectory-and(Test-Path-Path$workingDirectory)){
$psi.WorkingDirectory=$workingDirectory
}

$process=New-Object-TypeNameSystem.Diagnostics.Process
$process.StartInfo=$psi
$process.EnableRaisingEvents=$true

#Registertheeventhandlerforerror
$stdErrEvent=Register-ObjectEvent-InputObject$process-EventName'ErrorDataReceived'-Action{
if(![String]::IsNullOrEmpty($EventArgs.Data)){
$EventArgs.Data|Write-Error
}
}

#Startingprocess.
$process.Start()|Out-Null
$process.BeginErrorReadLine()|Out-Null
$output=$process.StandardOutput.ReadToEnd()
$process.WaitForExit()|Out-Null
$output|Write-Output

#UnRegistertheeventhandlerforerror
Unregister-Event-SourceIdentifier$stdErrEvent.Name|Out-Null
}
}


functionGetInternal-DotNetExePath{
process{
$dotnetinstallpath=$env:dotnetinstallpath
if(!$dotnetinstallpath){
$DotNetRegItem=Get-ItemProperty-Path'hklm:\software\dotnet\setup\'
if($env:DOTNET_HOME){
$dotnetinstallpath=Join-Path$env:DOTNET_HOME-ChildPath'dotnet.exe'
}
elseif($DotNetRegItem-and$DotNetRegItem.InstallDir){
$dotnetinstallpath=Join-Path$DotNetRegItem.InstallDir-ChildPath'dotnet.exe'
}
}
if(!(Test-Path$dotnetinstallpath)){
throw'Unabletofinddotnet.exe,pleaseinstallitandtryagain'
}
#return
[System.IO.FileInfo]$dotnetinstallpath
}
}

functionGet-MSDeploy{
[cmdletbinding()]
param()
process{
$installPath=$env:msdeployinstallpath

if(!$installPath){
$keysToCheck=@('hklm:\SOFTWARE\Microsoft\IISExtensions\MSDeploy\3','hklm:\SOFTWARE\Microsoft\IISExtensions\MSDeploy\2','hklm:\SOFTWARE\Microsoft\IISExtensions\MSDeploy\1')

foreach($keyToCheckin$keysToCheck){
if(Test-Path$keyToCheck){
$installPath=(Get-itemproperty$keyToCheck-NameInstallPath-ErrorActionSilentlyContinue|select-ExpandPropertyInstallPath-ErrorActionSilentlyContinue)
}

if($installPath){
break;
}
}
}

if(!$installPath){
throw"Unabletofindmsdeploy.exe,pleaseinstallitandtryagain"
}

[string]$msdInstallLoc=(join-path$installPath'msdeploy.exe')

"Foundmsdeploy.exeat[{0}]"-f$msdInstallLoc|Write-Verbose

$msdInstallLoc
}
}

functionInternalNormalize-MSDeployUrl{
[cmdletbinding()]
param(
[Parameter(Position=0,Mandatory=$true)]
[string]$serviceUrl,

[string]$siteName,

[ValidateSet('WMSVC','RemoteAgent','InProc')]
[string]$serviceMethod='WMSVC'
)
process{
$tempUrl=$serviceUrl
$resultUrl=$serviceUrl

$httpsStr='https://'
$httpStr='http://'
$msdeployAxd='msdeploy.axd'

if(-not[string]::IsNullOrWhiteSpace($serviceUrl)){
if([string]::Compare($serviceMethod,'WMSVC',[StringComparison]::OrdinalIgnoreCase)-eq0){
#ifnohttporhttpsthenaddone
if(-not($serviceUrl.StartsWith($httpStr,[StringComparison]::OrdinalIgnoreCase)-or
$serviceUrl.StartsWith($httpsStr,[StringComparison]::OrdinalIgnoreCase))){

$serviceUrl=[string]::Concat($httpsStr,$serviceUrl.TrimStart())
}
[System.Uri]$serviceUri=New-Object-TypeName'System.Uri'$serviceUrl
[System.UriBuilder]$serviceUriBuilder=New-Object-TypeName'System.UriBuilder'$serviceUrl

#ifit'shttpsandtheportwasnotpassedinoverrideitto8172
if(([string]::Compare('https',$serviceUriBuilder.Scheme,[StringComparison]::OrdinalIgnoreCase)-eq0)-and
-not$serviceUrl.Contains((':{0}'-f$serviceUriBuilder.Port))){
$serviceUriBuilder.Port=8172
}

#ifnopaththenaddone
if([string]::Compare('/',$serviceUriBuilder.Path,[StringComparison]::OrdinalIgnoreCase)-eq0){
$serviceUriBuilder.Path=$msdeployAxd
}

if([string]::IsNullOrEmpty($serviceUriBuilder.Query)-and-not([string]::IsNullOrEmpty($siteName)))
{
$serviceUriBuilder.Query="site="+$siteName;
}

$resultUrl=$serviceUriBuilder.Uri.AbsoluteUri
}
elseif([string]::Compare($serviceMethod,'RemoteAgent',[StringComparison]::OrdinalIgnoreCase)-eq0){
[System.UriBuilder]$serviceUriBuilder=New-Object-TypeName'System.UriBuilder'$serviceUrl
#http://{computername}/MSDEPLOYAGENTSERVICE
#remoteagentmustusehttp
$serviceUriBuilder.Scheme='http'
$serviceUriBuilder.Path='/MSDEPLOYAGENTSERVICE'

$resultUrl=$serviceUriBuilder.Uri.AbsoluteUri
}
else{
#seeifit'sforlocalhost
[System.Uri]$serviceUri=New-Object-TypeName'System.Uri'$serviceUrl
$resultUrl=$serviceUri.AbsoluteUri
}
}

#returntheresulttothecaller
$resultUrl
}
}

functionInternalRegister-AspNetKnownPublishHandlers{
[cmdletbinding()]
param()
process{
'RegisteringMSDeployhandler'|Write-Verbose
Register-AspnetPublishHandler-name'MSDeploy'-force-handler{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
$publishProperties,
[Parameter(Mandatory=$true,Position=1,ValueFromPipelineByPropertyName=$true)]
$packOutput
)

Publish-AspNetMSDeploy-publishProperties$publishProperties-packOutput$packOutput
}

'RegisteringMSDeploypackagehandler'|Write-Verbose
Register-AspnetPublishHandler-name'Package'-force-handler{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
$publishProperties,
[Parameter(Mandatory=$true,Position=1,ValueFromPipelineByPropertyName=$true)]
$packOutput
)

Publish-AspNetMSDeployPackage-publishProperties$publishProperties-packOutput$packOutput
}

'RegisteringFileSystemhandler'|Write-Verbose
Register-AspnetPublishHandler-name'FileSystem'-force-handler{
[cmdletbinding()]
param(
[Parameter(Mandatory=$true,Position=0,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
$publishProperties,
[Parameter(Mandatory=$true,Position=1,ValueFromPipelineByPropertyName=$true)]
$packOutput
)

Publish-AspNetFileSystem-publishProperties$publishProperties-packOutput$packOutput
}
}
}

<#
.SYNOPSIS
Usedfortestingpurposesonly.
#>
functionInternalReset-AspNetPublishHandlers{
[cmdletbinding()]
param()
process{
$script:AspNetPublishHandlers=@{}
InternalRegister-AspNetKnownPublishHandlers
}
}

Export-ModuleMember-functionGet-*,Publish-*,Register-*,Enable-*
if($env:IsDeveloperMachine){
#youcansettheenvvartoexposeallfunctionstoimporter.easyfordevelopment.
#thisisrequiredforexecutingpestertestcases,it'ssetbybuild.ps1
Export-ModuleMember-function*
}

#registerthehandlerssothatPublish-AspNetcanbecalled
InternalRegister-AspNetKnownPublishHandlers

