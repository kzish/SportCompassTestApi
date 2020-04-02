# SportCompassTestApi
SportCompass Test Api

intall mysql latest version
install dotnet core v2.2
install visual studio 2019


run mysql script "sportcompass.sql"

1) open project in visual studio 2019
2) open SportCompassRestApi/Models/dbContext.cs 
3) on line 28 
   change connection string to connect to your local db
   optionsBuilder.UseMySql("server=localhost;database=sportcompass;uid=root;pwd=;");

open wwwroot folder in explorer 
give full access to this folder as the images will be uploaded to it

press f5 to debug the program

if you prefer to lauch the api from iis webserver

right click on the project folder from within visual studio and select publish
simply follow the instructions to deploy




