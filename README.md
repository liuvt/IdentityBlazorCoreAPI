<h1> IdentityBlazorCoreAPI v1 👋 </h1>

- This's website with Identity Blazor Server Core API with Client and Server in a hosting/localhost

<h3>Enviroment</h3>

- Languages: C#, HTML, CSS, JavaScript
- Back-End: Blazor Server Core API, EF, LinQ
- Front-End: Blazor Server
- Databases: MySQL
- Other: Firebase, Drive Google API

<h3>Change configs in files</h3>

- IdentityBlazorCoreAPIDbContext.cs: connenction string to MySQL
- Program.cs: connenction string to MySQL
- appsettings.json: ConnectionStrings, JWT, localhost

<h4>Connection String MySQL</h4>

- _server=localhost;user=root;password=123456aA@;Port=3306;database=IdentityBlazorAPI; Persist Security Info=False; Connect Timeout=300_

<h4>Github cmd</h4>

- Clone project: _git clone https://github.com/liuvt/IdentityBlazorCoreAPI.git_

*Some command help for Dev*
- Git add fiile changes: _git add *_
- Git commit: _git commit -m "comment"_

<h3>Command dotnet to run project step by step</h3>

- Clone project: _git clone https://github.com/liuvt/IdentityBlazorCoreAPI.git_
- Build: _dotnet build_
- Create mirations: _dotnet ef migrations add Init -o Data/Migrations_
- Create database: _dotnet ef database update_
- Hot run: _dotnet watch run /a_
- Publish project: _dotnet publish -c Release --output ./Publish IdentityBlazorCoreAPI.csproj_

<h3>Learn by Video</h3>

Tutorial video learn about: https://www.youtube.com/playlist?list=PL1GjZTGoP_IldvgKuAipAUIok0T5Sgpu5

⭐️ From [Ruộng](https://github.com/liuvt)
