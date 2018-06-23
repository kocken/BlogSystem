# EF_Mikael_Karlgren_Blog
## About
A web blog system written with ASP.NET Core (Entity Framework Core and MVC Core). Microsoft SQL Server Express LocalDB is used for database storage, whereas the connection details are found at "Blog/appsettings.json". This project was made as a final assignment in the course "Dataåtkomster i NET" (".NET data access") winter 2018 at [Teknikhögskolan Gothenburg](http://teknikhogskolan.se/utbildningar/fullstack-developer-net).

## Features
The system contains moderation functionality, which restricts users to access certain functions. For example, only users having the mod/admin rank are able to create threads, but the threads can be commented by anyone logged in. When a comment is made, it requires approval from staff. This is done through the "Moderation" navigation bar link, which is seen and can be accessed from moderators and administators. The "Moderation" navbar title color will be red when there are available comments that requires evaluating. There are some other restrictions in place too, to avoid actions that the user might of not have wanted to intend, such as registering/logging in when already logged in. All these acitons, whatever successful or not, are neatly tied up together with log messages to have the ability to spot errors and whatnot. The user also gets informerical client-messages when executing actions.

Furthermore there's also features to improve the user experience. The frontend layout is very appealing, it's modern with simple but a nice looking bootstrap design, supported on all major browsers, with a few nice feautures to make it even better: 
* when filling in & submitting forms there is client-sided (and server-sided) validation that checks if field values are valid, which also contains validation for "special occasions" such as checking that the username is unique on register
* when logging in and registering there is ["floating" label](https://www.jquery-az.com/bootstrap4/demo2.php?ex=94.0_3) support for modern browsers (such as chrome and firefox), which is excluded when using certain non-supported browsers such as Internet Explorer and Edge
* when the user receives messages, it will either be shown on the form as an error message or through a [nice looking alert box](https://github.com/HubSpot/vex)

Lastly, for convenience, logged in users will remain logged in for up to two weeks of inactive usage, through the use of server session (which is based of client cookies).
