# Taken from https://www.appveyor.com/docs/running-tests/#uploading-xml-test-results

$wc = New-Object 'System.Net.WebClient'
$wc.UploadFile("https://ci.appveyor.com/api/testresults/nunit3/$($env:APPVEYOR_JOB_ID)", (Resolve-Path .\CSF.Validation.Tests\TestResults\TestResults.xml))