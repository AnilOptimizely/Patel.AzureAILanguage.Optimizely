﻿# Setup

## 1. Install the addon

## 2. Create CMS Database
Before starting the solution for the first time, you will need to create a new cms database. Below is the syntax for this,
replace the quote marks with the details required to create the CMS Database

dotnet-episerver create-cms-database -S "Server" -E -dn "DatabaseName" -du "DatabaseUser" -dp "DatabasePassword" AzureAIContentSafety.csproj

For more information on how to create a new CMS database, the below links are useful for this

- https://www.herlitz.io/2022/06/30/creating-a-new-cms-database-using-the-optimizely-cli/
- https://docs.developers.optimizely.com/content-management-system/docs/installing-optimizely-net-5

## 3. Create Azure AI Content Safety Resource

1. Navigate to the Azure Portal by clicking [here](https://portal.azure.com/)
1. Click on create new resource 
1. Search for Azure AI Content Safety
2. Select the option Azure AI Content Safety
3. Fill out details in relation to Project Details (Choose Subscription) and Instance Details (Region/Name/Pricing Tier)
4. Click Create
5. When resource has been created, Navigate to the Keys and Endpoint section. An example screenshot of this is shown below

![ResourceKey.](https://github.com/AnilOptimizely/Patel-AzureAIContentSafety/blob/main/docs/Features/Configuration/ContentSafetyResourceKeyEndpointInfo.JPG)

7. Make a note of the Key and Endpoint variables - This will be needed in Step 5.

## 4. Create CMS Admin Account
When the package has loaded, navigate to /Util/Register to set up a CMS admin account. The screen will look something like the below screenshot

![CMS Admin Screen.](https://github.com/AnilOptimizely/Patel-AzureAIContentSafety/blob/main/docs/Features/Configuration/CreateAdminAccount.JPG)

Once the Admin Account has been created, navigate to /util/login to log in with these admin credentials.

The Admin account can also be created using a Powershell script like the one below

dotnet-episerver add-admin-user -u AdminUser -p Admin123! -e myemail@mail.com -c EPiServerDB "C:\Users\me\Work\Optimizely\src\MyProject.csproj"

Amend the above script to suit the CMS Admin User that's required to be created. There are links below which provide more information about how this is done 

- https://www.herlitz.io/2022/06/30/creating-a-new-cms-database-using-the-optimizely-cli/
- https://www.epinova.no/en/folg-med/blog/2022/how-to-create-an-admin-user-i-optimizely-cms--with-episerver-cli/

## 5. CMS Configuration
Before using the features offered by the Azure AI Content Safety Service, there is CMS configuration required for this. To do this, navigate to the Start Page to the Azure Content Safety Tab, a screenshot of this is below

![contentsafetyconfiguration.](https://github.com/AnilOptimizely/Patel-AzureAIContentSafety/blob/main/docs/Features/Configuration/ContentSafetyCMSConfiguration.JPG)

Below is a summary of what each CMS field is used for

- The Azure AI Content Safety Endpoint needs to be filled in with the endpoint value from the Content Safety Resource. This is a required field.
- The Azure AI Content Safety Key needs to be filled in the Key1 value from the Content Safety Resource. This is a required field.
- The Hate Result Severity Level / Sexual Result Severity Level / Violence Result Severity Level and Self Harm Result values need to be filled in with a number value. Ideally this needs to be between 1-7, The lower the value, the more harmful content will be allowed to be published in the CMS. For more information about the severity levels, please visit this link : https://learn.microsoft.com/en-us/azure/ai-services/content-safety/concepts/harm-categories?tabs=definitions
  
- Text Analysis Allowed tickbox will enable the Text Detection API to be used in the CMS. This needs to be ticked in order for any Text Analysis feature to be used
  
- Image Analysis Allowed tickbox will enable the Image Detection API to be used in the CMS. This needs to be ticked in order for the Image Analysis feature to be used
  
- Text Analysis on Page Field will enable Text Detection to be done on the Analyse Text field on a Standard Page
- Text Analysis on Multiple Text Properties Field will enable Text Detection to be done on the following fields; Analyse Text, Meta Description and Teaser Text fields on a Standard Page
- Text Analysis on One Property with Blocklist will enable Text Detection to be done on the Analyse Text field on a Standard Page, using a Blocklist
- Text Analysis on Multiple Text Properties Field will enable Text Detection to be done on the following fields; Analyse Text, Meta Description and Teaser Text fields on a Standard Page, using a blocklist


- For more information as to how Text Analysis has been done, please click [here](https://github.com/AnilOptimizely/Patel-AzureAIContentSafety/blob/main/docs/Features/Text%20Analysis/TextAnalysis.md) 
- For more information as to how Image Analysis has been done, please click [here](https://github.com/AnilOptimizely/Patel-AzureAIContentSafety/blob/main/docs/Features/Image%20Analysis/ImageAnalysis.md) 
