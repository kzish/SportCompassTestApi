# SportCompassTestApi
SportCompass Test Api

1) intall mysql latest version<br />
2) install dotnet core v2.2<br />
3) install visual studio 2019<br />


run mysql script "sportcompass.sql" to install the database<br />

1) open sportcompass project in visual studio 2019<br />
2) open SportCompassRestApi/Models/dbContext.cs <br />
3) on line 28 <br />
   change connection string to connect to your local db<br />
   optionsBuilder.UseMySql("server=localhost;database=sportcompass;uid=root;pwd=;");<br /><br /><br />

open wwwroot folder in explorer <br />
give full access to this folder as the images will be uploaded to it<br /><br />

4) press f5 to debug the program<br />
5) the project must open in your default browser<br />
6) navigate to the url "swagger" or click the link to open the api<br />
7) both api's are present 'triangle and blog'<br />
<h2> Note</h2>
This is a rest api and uses rest verb semantics<br />
where "post" is to upload data to the server<br />
	  "put" is to update the entire model<br />
	  "patch" is to update part of the information<br />
	  "delete" is to remove information from the server<br />
	  "get" is to fetch information from the server<br />

if you prefer to launch the api from iis webserver<br /><br />

right click on the project folder from within visual studio and select publish<br />
simply follow the instructions to deploy<br />




