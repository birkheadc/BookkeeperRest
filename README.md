# Book-keeper Back End

This is the back end for my simple book keeping application, a REST API built in ASP.NET Core.

## Necessary Environment Variables:

The following environment variables must be established, via Dockerfile, for the application to run:

- `ASPNETCORE_EMAILCONFIG_NAME`: The name of the email client the app will use
- `ASPNETCORE_EMAILCONFIG_ADDRESS`: The address of the same email client.
- `ASPNETCORE_EMAILCONFIG_SMTPSERVER`: The address of the email client's smtp server.
- `ASPNETCORE_EMAILCONFIG_PORT`: The port for the email client.
- `ASPNETCORE_EMAILCONFIG_USERNAME`: The username of the email client.
- `ASPNETCORE_EMAILCONFIG_PASSWORD`: The password of the email client.

- ASPNETCORE_CONNECTIONSTRING: The connection string for the application to connect to the database.

## Notes on User Settings

(In retrospect, this was an awful idea, but I have not yet rebuilt this)
(While reading, please keep in mind that I have since learned better)

User settings are stored as NAME-VALUE pairs in the UserSettingsRepository ('KEY' is a reserved word in MySql, so 'NAME' is used instead).

Some settings should be integers, others should be boolean, but there is no hard-coding or checks for this internally. The front-end should be managed 
in such a way as to not store arbitrary string VALUEs with NAMEs that are expected to be boolean. And also be able to handle strings where a boolean was expected.

E.g. the front end expects "IsCashDefault" to be a boolean value, so it can display it as a checkbox rather than an input. It should take care to only send 'true' or 'false' when updating this VALUE, and also do some validation when retreiving the VALUE to make sure it is 'true' or 'false'. And probably know what to default to in the case that it isn't.

User Settings is essentially a place for the front-end to store key-value pairs however it sees fit. If the front-end makes a mess of things it is entirely on the front-end to deal with it.

Because of this nature of User Settings, any User Settings should ONLY be used to store front-end logic pertaining to how data is displayed / the user experience. Nothing related to business logic should ever be stored in this way, as it is not designed to be that reliable.