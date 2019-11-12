# CBW227911 - Control Dynamo from the Web
Radu Gidei
Mark Thorley

Join this breakout session to learn how to build Dynamo extensions from scratch and why they're so powerful. We'll showcase this by building an extension that embeds a web server & REST API inside Dynamo, allowing users to remotely open & run graphs, extract results, add/remove nodes, & more, all from a web browser. 

We’ll start by exploring the structure & concepts of a Dynamo extension through annotated code samples. Using the Dynamo API, we’ll then learn how to build up our extension in a modular way, so that we can collaboratively add new features to our extension. By the end, we’ll have turned Dynamo into a service that everyone at your practice can use by simply visiting a web page.

## Exercises

We will go through a number of exercises in this workshop. Each numbered exercise folder has the code required to start that exercise.

To see the completed version, simply go to the next exercise folder, so completed files for `Exercise 6` are in folder `Exercise 7`. 

Our workshop is split in 2 session :
- HelloDynamo : basic extension skills
- DynamoServer : building & extending a server inside Dynamo

### HelloDynamo

We have 6 exercises covering :

1 - anatomy of an extension & set-up
2 - making an extension
3 - making a view extension
4 - keeping references to Dynamo objects
5 - adding menus to Dynamo UI
6 - handling Dynamo events

### DynamoServer

This session will cover :

- how the web works
- background on the DynaWeb package
- C# background tasks
- C# thread-safe execution
- making a web server with the Nancy framework
- referencing a different project in a solution
- Nancy modules

And several exercises : 

1 - adding a module to `DynamoServer` extension 
2 - web-enabled extensions brainstorm