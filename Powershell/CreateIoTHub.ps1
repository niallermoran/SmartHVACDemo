## Login-AzureRmAccount

Select-AzureSubscription "Nialls Demo Account"

New-AzureRmResourceGroup -Name IoTDemo -Location "North Europe"

New-AzureRmResourceGroupDeployment -ResourceGroupName IoTDemo -TemplateFile "C:\Code\Demos\IoT\SmartStadium\Powershell\IoTHub.json" -hubName "SmartHVACDemo"