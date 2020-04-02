# SportCompassTestApi
SportCompass Test Api

intall mysql latest version
install dotnet core v2.2
install visual studio 2019



run mysql script "sportcompass.sql" to install the database

1) open sportcompass project in visual studio 2019
2) open SportCompassRestApi/Models/dbContext.cs 
3) on line 28 
   change connection string to connect to your local db
   optionsBuilder.UseMySql("server=localhost;database=sportcompass;uid=root;pwd=;");

open wwwroot folder in explorer 
give full access to this folder as the images will be uploaded to it

4) press f5 to debug the program
5) the project must open in your default browser
6) navigate to the url "swagger"
7) both api's are present 'triangle and blog'
<h2> Note</h2>
This is a rest api and uses rest verb semantics
where "post" is to upload data to the server
	  "put" is to update the entire model
	  "patch" is to update part of the information
	  "delete" is to remove information from the server
	  "get" is to fetch information from the server

if you prefer to lauch the api from iis webserver

right click on the project folder from within visual studio and select publish
simply follow the instructions to deploy




