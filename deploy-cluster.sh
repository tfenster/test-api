# login
az login --use-device-code
az account set --subscription="94670b10-08d0-4d17-bcfe-e01f701be9ff"

# create cluster
location=northeurope
rg=aks-repro
clustername=$rg
windowsuser=vm-admin
windowspassword=Passw0rd*12345

az group create --name $rg --location $location
az aks create --resource-group $rg --name $clustername --node-count 1 --enable-addons monitoring --generate-ssh-keys --windows-admin-username $windowsuser --windows-admin-password $windowspassword --vm-set-type VirtualMachineScaleSets --network-plugin azure
az aks get-credentials --resource-group $rg --name $clustername
az aks nodepool add --resource-group $rg --cluster-name $clustername --os-type Windows --os-sku Windows2022 --name npwin --node-count 1

# delete resource group
#az group delete --name $rg --yes --no-wait
