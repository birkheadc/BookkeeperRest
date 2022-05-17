# Book-keeper Back End

This is the back end for my simple book keeping application, a REST API built in ASP.NET Core.

## Necessary Secrets:

The following secrets must be established, via `dotnet user-secrets`, for the application to run:

- EmailConfig: contains all the data the server needs to log into it's email so it can send emails.

- ConnectionString: the connection string to log into the database

For example:

{\
&emsp;"EmailConfig":\
&emsp;{\
&emsp;&emsp;"From": "example@place.com",\
&emsp;&emsp;"SmtpServer": "smtp.place.com",\
&emsp;&emsp;"Port": 123,\
&emsp;&emsp;"UserName": "example@place.com",\
&emsp;&emsp;"Password": "sECRETpASSWORD"\
&emsp;},\
&emsp;"ConnectionString": "server=xyz;port=1234;database=bookkeeper;user=user;password=secret"\
}
