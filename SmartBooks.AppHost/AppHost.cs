var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql").WithDataVolume("mysql_data");
var database = mysql.AddDatabase("appdb");

builder.AddProject<Projects.SmartBooks_Api>("api")
    .WithReference(database)   // inyecta la connection string
    .WaitFor(mysql);           // espera el healthcheck de MySQL

builder.Build().Run();
