# ORight-API

This Project Takes an excel file and store its data in table, Table Schema is simple
Id - INT AutoIncrement (PK)
CliendId - INT NOT NULL
FAT - DECIMAL NOT NULL
SNF - DECIMAL NOT NULL
RATE - DECIMAL NOT NULL
Send client ID and FILE through POSTMAN while storing data.

URL FOR 1st API to store data in DB-
POST http://localhost:5000/api/upload?clientId=3
Body > Form Data > Select key as file and type as File and upload the file.

URL FOR 2nd API to fetch the stored data in DB-
http://localhost:5000/api/getPrice?clientId=1&snf=10.2&fat=11.1

## Dependencies-
Frameworks - Microsoft.NETCore.App
Packages - 
ClosedXML(0.102.2)
Dapper (2.1.35)
EmbedIO (3.5.2)
EPPlus (7.1.3)
HttpMultiPartParser (8.4.0)
Microsoft.Extensions.Configurations (8.0.0)
Microsoft.Extensions.Configurations.JSON (8.0.0)
Microsoft.Extensions.DependencyInjection (8.0.0)
MySql.Data(8.4.0)

## For Brownie points:
In rateChartRepository.cs - Instead of writing records in DB for each record record. Send them in batched or store them in a Data Structure and Write at the end of Process, this will reduce the time significantly for POST Call.
And For GET CALL set up indexing in DB over ClientID, Fat and SNF for faster retrival.


## Flow of Project-
Start the project by creating a Dotnet Core console app. 
Setup a server through program.cs file. 
Establish a connection to DB (MySql).
Then start off with controller, create two APIs for sending a file to store in DB and One retrival of data.
Create BO or Model that will handle the data to be stored or fetched.
Process further by writing their services.

After completion test the api with the help or api tools like Postman or HTTpie.

GOOD LUCK!
