Create a new solution, a self-hosted web API server as a console application  (dotnet core, Kestrel) listening on port 9091

Create a "CountryGwp" controller
Create endpoint listening for POST requests(application/json) at route http://localhost: 9091/server/api/gwp/avg. The request allows user to choose a country and one or more lines of business (LOB). The api should return an average gross written premium (GWP) over 2008-2015 period for the selected lines of business.
The input should be of the following format:
{
    "country": "ae",
    "lob": ["property", "transport"]
}

The output should look similar to this:
{
    "transport": 446001906.1,
    "liability": 634545022.9
}

Accompany your solution with a brief description of how it should be run/tested (the application must be able to compile and run!)
Push it to a public repository in github/bitbucket and send us the link.
Bonus points for:

•             Implement the calls asynchronously

•             Follow SOLID principles when designing the Client API, especially the use of DI/IoC

•             Decouple the database concerns with the business logic using a repository pattern to access the data

•             Create a unit or E2E test to run  and test the data extraction code using framework of your choice, e.g. NUnit

•             Provide some automated documentation of the API (e.g. using tools like Swagger) or create unit test to run the code

•             Add some basic error handling

•             Cache the results of each request and return the cached result if available
