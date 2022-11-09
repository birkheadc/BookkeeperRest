# Book-keeper Back End

This is the back end for my simple book keeping application, a REST API built in ASP.NET Core.

## Necessary Environment Variables:

The following environment variables must be established for the application to run:

- `ASPNETCORE_EMAILCONFIG_NAME`: The name of the email client the app will use
- `ASPNETCORE_EMAILCONFIG_ADDRESS`: The address of the same email client.
- `ASPNETCORE_EMAILCONFIG_SMTPSERVER`: The address of the email client's smtp server.
- `ASPNETCORE_EMAILCONFIG_PORT`: The port for the email client.
- `ASPNETCORE_EMAILCONFIG_USERNAME`: The username of the email client.
- `ASPNETCORE_EMAILCONFIG_PASSWORD`: The password of the email client.

- `ASPNETCORE_CONNECTIONSTRING`: The connection string for the application to connect to the database.

In Development, `dotnet user-secrets set key value` should be sufficient. In production, these should be passed to the Dockerfile via arguments in the docker-compose.yml.