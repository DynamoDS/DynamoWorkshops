
# E1 - Make Nancy Webserver

## Webserver

Nancy allows you to create a self-hosted web server very easily.

First, we decide what URL the server will be available at, let's record this in a constant property on the `Program` class :

```cs
        private const string DEFAULT_URL_BASE = "http://localhost:1234";
```

Then, we create a configuration and tell it to figure out URL reservations for us. (take my word on this, you don't want to do this on your own.)

```cs
            var serverConfig = new HostConfiguration();
            serverConfig.UrlReservations.CreateAutomatically = true;
```

Finally, we can make the server itself & start it.
```cs
            var server = new NancyHost(serverConfig, new Uri(DEFAULT_URL_BASE));
            server.Start();
```
You can type in the URL in a browser when the server is running, or we can tell our app to launch it immediately.

```cs
            Process.Start("http://localhost:1234");
```

**Very important** : note that after launching the server, we need to instruct our app to wait for a key to be pressed, otherwise it would run out of lines to execute and close the app.

```cs
            Console.WriteLine("Press any key to terminate server");
            Console.ReadKey();
```
