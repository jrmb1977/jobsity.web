# jobsity.web

Mandatory Features

Done:

1. Allow registered users to log in and talk with other users in a chatroom.
2. Have the chat messages ordered by their timestamps and show only the last 50 messages
3. Unit test the functionality you prefer

Not Done:

1. Allow users to post messages as commands into the chatroom with the following format /stock=stock_code
2. Create a decoupled bot that will call an API using the stock_code as a parameter
3. The bot should parse the received CSV file and then it should send a message back into
   the chatroom using a message broker like RabbitMQ. The message will be a stock quote
   using the following format: “APPL.US quote is $93.42 per share”. The post owner will be
   the bot.
   
Bonus (Optional)

Done:

1. Use .NET identity for users authentication

RabbitMQ (Message Broker)

[Considerations - Keep confidential information secure]
In Web.config is saved the Full Path to the Json file with the data needed to connect to RabbitMQ

<appSettings>
	<add key="RabbitMQServerConfigPath" value="C:\RabbitMQ\RabbitMQ.json" />
</appSettings>

The content of Json file (RabbitMQ.json) is:

{
  "hostName" : "localhost",
  "userName" : "guest",
  "password" : "guest",
  "port" : 5672,
  "virtualHost" : "/",
  "exchangeName": "chat",
  "queueName": "friends"
}

That info is loaded on the constructor method of class App_Code/RabbitMQData

MySQL (DataBase)

1. Connection String

In Web.config is saved the Full Connection String needed to connect to MySQL
That info is loaded on the constructor method of class App_Code/MySQLData

[Considerations - Keep confidential information secure]
Although is plain in the project, it can be encripted/decripted for security reasons:

Opening a CMD windows:

Encrypt
CD C:\Windows\Microsoft.NET\Framework\v4.0.30319
aspnet_regiis -pef "connectionStrings" "D:\inetpub\wwwroot\applicationFolder"

Decrypt
CD C:\Windows\Microsoft.NET\Framework\v4.0.30319
aspnet_regiis -pdf "connectionStrings" "D:\inetpub\wwwroot\applicationFolder"

2. Parameters

Also, in Web.config is saved as a Parameter the number of Messages displayed on Screen (Chat Room)
The idea to leave this as a parameter is to be able to change it without further need of change in code

<appSettings>
	<add key="TopMessagesInChatRoom" value="50" />
</appSettings>

This parameter is used in the Method SelectNotReadMessages of the Class App_Code/ChatDataBase

3. Scripts to Create Objects

CREATE TABLE `usuarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Usuario` varchar(25) NOT NULL,
  `Clave` varchar(80) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Activo` int unsigned NOT NULL DEFAULT '1',
  `Codigo` varchar(20) NOT NULL DEFAULT 'A',
  `Descripcion` varchar(100) NOT NULL DEFAULT 'A',
  `Correo` varchar(320) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Codigo_UNIQUE` (`Codigo`),
  UNIQUE KEY `Usuario_UNIQUE` (`Usuario`),
  UNIQUE KEY `Correo_UNIQUE` (`Correo`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=latin1;


CREATE TABLE `chatroom` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Usuario` varchar(25) NOT NULL,
  `Message` varchar(6550) NOT NULL,
  `SentDate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=latin1;

CREATE TABLE `readmessages` (
  `Id` int NOT NULL,
  `Usuario` varchar(25) NOT NULL,
  `ReadDate` datetime NOT NULL,
  PRIMARY KEY (`Id`,`Usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

4. Scripts to Manipulate Data

SQL Syntax are in class App_Code/ChatDataBase in Methods:

RegisterUsuario
Login
InsertMessageInChatRoom
SelectNotReadMessages
InsertReadMessage

Although ADO.NET is used to manipulate date, I can also work with Entity Framework.
Although scripts are inserted in code, I can also work with procedures so code can be placed on server side.

Considerations - Feel free to use small helper libraries

Libraries used in this project:

Newtonsoft.Json
Microsoft.AspNet.Identity.Core
MySql.Data
RabbitMQ.Client

Considerations - The project is totally focused on the backend; please have the frontend as simple as you can

In Views/Chat/Index.cshtml can be found the references to the main methods of the backend

$("#btnsend").click(function () {.... /Controllers/ChatController/sendmsg (POST)

setInterval(function () {.... /Controllers/ChatController/receivemsg (POST)

JQuery is mostly used instead of just simple javascript
