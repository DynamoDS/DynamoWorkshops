# Session 2 : C#

# Exercise 1 : Modelling the world as objects (classes) 

Launch VisualStudio 2019 and
- make a new solution+project as a `.NET Framework Library`
- right-click on project and add a new class called `Desk`


## Adding properties

Inside this new class, add a few properties

```cs
        public double Length { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        public double PricePerDay { get; }
```

The `{ get; set; }` syntax is shorthand for auto-implemented properties, where the C# compiled adds a `backing field` (`length` below) and uses this value when getting or setting the `Length` property.

```cs
        private double length;
        public double Length { get; set; }
```

Notice the `PricePerDay` only has a `getter` : this means the property cannot be updated from outside this class.

## Properties that are other classes

Since desks are occupied by people, let's record its occupier too :
```cs
        public Person Occupier { get; set; }
```

But wait, what data type is Person ? The dreaded red squiggly lines tells us it doesn't exist.

Well, let's make it then!

```cs
    public class Person
    {
       public string Name { get; set; }
       public string Email { get; set; }
       public Guid Id { get; set; }
       public Guid AssignedDesk { get; set; }
    }
```
Notice the `Guid` type. Guids are `globally unique identifiers`, meaning that text (`string`) is *almost* guaranteed to be unique in the universe. Read up on `hash collisions` if you're interested in the subject of why it's only *almost* guaranteed.

An `Id` would be a good idea for a desk too, let's go back and add it to the `Desk` class
```cs
        public Guid Id { get; set; }
```

We also need to know where in space the desk is, so let's add an origin point :
```cs
        public Point Origin { get; set; }
```

The `Point` type is part of the Dynamo geometry library, so we'll need to add this using statement at the top of the file :
```cs
using Autodesk.DesignScript.Geometry;
```

## Enums

We need to know what areas of our space a member has access to, so let's add a `Membership` property to `Person` :

```cs
        public Membership Membership { get; set; }
```

This is of course a new class :
```cs
    public class Membership
    {
        public int Number { get; set; }
        public string Type { get; set; }

    }
```

If we left it like this, we'd have to check the string representing the membershipy type is not empty and that it's one of a few select options we have - every single time we use this property.

That's bonkers and thankfully, we have a better option : an `Enum`.

Add the following enum to the same file, but not inside the class :

```cs
    public enum MembershipType
    {
        Coworking,
        DedicatedDesk,
        PrivateOffice,
        Enterprise
    }
```

Like their name hints, `Enums` are simple enumerations that can be used to represent a series of options, with the added benefits over strings that they represent a `type` and are therefore much easier to use and less prone to errors.

Now change the type of the `Type` property to the `MembershipType` enum.

# Exercise 2 : Adding capabilities to objects

## Methods

We know the width, length & height of the desk, so we can calculate its volume. Like the word `calculate` implies, this is a computation based on other properties, so it's good practice to not also make this a property. Otherwise, once you update the length for example, the volume would not update itself automatically.

So let's add a method to calculate the `Volume` of the desk :
```cs
        public double Volume()
        {
            return this.Width * this.Length * this.Height;
        }
```

Similarly, we can add methods for `Area` and `Price`.
```cs
        public double Area()
        {
            return this.Width * this.Length;
        }

        public double PriceForNumberOfDays(int days)
        {
            return days * this.PricePerDay;
        }
```

What about if we want to change things ? Let's add a method to assign a desk to a person, adding it to the `Person` class :
```cs
        public void AssignDesk(Desk desk)
        {
            this.AssignedDesk = desk.Id;
            desk.Occupier = this;
        }
```
Similarly, we can change a person's membership :
```cs
        public bool ChangeMembership(Membership newMembership)
        {
            try
            {
                this.Membership = newMembership ?? throw new ArgumentNullException(nameof(newMembership));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
```

The `try`+`catch` blocks are used to `catch` any `Exceptions` throw by the code contained within. The `Membership` class constructor could throw exceptions, but here we are throwing ourselves when input is bad.

We also use this construct to figure out if the membership was indeed changed or if it failed, otherwise we'd have no feedback.

## Constructors

Constructors are special methods that can be used to create objects.

Add the special constructor method to the `Desk` class :
```cs
        public Desk()
        {
            
        }
```

Inside this constructor, we initialise the properties of `Desk` :
```cs
this.Id = Guid.NewGuid();
            this.PricePerDay = 35;
            this.Origin = Point.ByCoordinates(0,0,0);
```

For `Membership`, add a constructor :
```cs
        public Membership()
        {
            this.Number = Guid.NewGuid().GetHashCode();
            this.Type = MembershipType.None;
        }
```

For `People`, add a constructor :
```cs
        public Person()
        {
            this.Name = "John Doe";
            this.Membership = new Membership();
        }
```
We initialise the membership property so it's never an invalid option, such as `null`.

## Overloads

We'd also like to be able to create a person using their name, so we add a second constructor that has a parameter :

```cs
        public Person(string name) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
        }
```

We're doing a few things here :
- we pass in a name using a `string` parameter : this is the mechanism to provide input to a method
- `:this()` calls the constructor with no parameters above
- `throw new ArgumentNullException` will raise an error (`Exception`) in our code. Dynamo will catch this and make the node yellow, but remember that uncaught exceptions crash apps.
- setting the `Name` property with the one from the parameter

Because this method has the same name as another one, but a different signature (parameters), it's called an `overload` method.

## Static methods

If we wanted to, we could add a third constructor, that can take in a name, email and membership at the same time :

```cs
        public static Person ByNameEmailMembership(string name, string email, Membership membership)
        {

        }
```

Notice this method is `static`, meaning it is not associated with any instance of the `Person` type, but with the `Person` type itself. Static method that return the same type as their class can be used to create new objects, without using the `new` keyword. Most Dynamo `+` nodes are static methods like this.

To create a new object, simply initialise one and return it :
```cs
            var person = new Person(name)
            {
                Email = email,
                Membership = membership
            };
            return person;
```

Ideally though, you want to validate the name & email provided, as they could be empty or just whitespace, so add this to the top of the method to make sure we don't create an object if those inputs are invalid.
```cs
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (membership is null)
            {
                throw new ArgumentNullException(nameof(membership));
            }}
```

## Inheritance

We notice that many of our classes have an `Id`, so wouldn't it be nice if we didn't have to repeat ourselves ? In true OOP fashion, we can represent the fact that something has an id as an object, an `Identifiable` class :

```cs
    public class Identifiable
    {
        /// <summary>
        /// The unique identifier of the person.
        /// </summary>
        public Guid Id { get; set; }

        public Identifiable()
        {
            this.Id = Guid.NewGuid();
        }
    }
```

How do we now link a `Person` to be `Identifiable` ?
We can make `Person` inherit all properties, methods, etc of Identifiable directly :
```cs
    public class Person : Identifiable
```

Note however that in C#, you can only inherit from one other class. Also, whilst inheritance can be chained, such as when a class `C` inherits from `B`, which itself inherits from `A`, try to not make deeply nested inheritance structures as a best practice!

# Exercise 3 : doing things with objects

Let's now add some smarts to this library and actually layout a space with desks!

Make a new class called `DeskArchitect`, which will hold our layouting logic & capabilities. Notice we don't add this to the `Desk` or `Person` class as, just like in the real world, it's something that's external to those entities.

There can be only one desk architect, so make it static.
```cs
    public static class DeskArchitect
```

Add a method to the class that, given a surface, can layout & return desks :
```cs
        public static List<Desk> LayoutDesksOnSurface(Surface surface)
```

We can call this method by using `DeskArchitect.LayoutDesksOnSurface()` - that reads nicely and is quite descriptive of what it will do. Try to name your classes & methods so that they easily make sense when used.

We first check some stuff and initialise a list to hold our results
```cs
            if (surface is null)
                throw new ArgumentNullException(nameof(surface));

            // initialise a list that will hold the created desks
            var desks = new List<Desk>();

```

Then we make a grid of points for our surface
```cs
            var points = GeometryUtils.GeneratePointGridForSurface(surface);
```

Finally, we make a desk at each point and return them :
```cs
            // make a new Desk instance for each point
            foreach (var point in points)
            {
                var desk = new Desk();
                desk.Origin = point;
            }

            return desks;
```

Remember those `Origin` points ? They came in handy, no ?

# Exercise 4 : results as data structures

One aspect that is not ideal is that our layout method simply returns a bunch of desks, with no information about the arrangement overall.

Let's fix that by modelling the result as....an object.

## Namespaces

Before diving in, notice we have now added folders to our solution and the `namespace` at the top of code files reflects this new organisation. This is a great way of grouping & separating functionality into discrete `spaces`.

## Properties

Let's make a new class :
```cs
    public class DeskArrangement : Identifiable
```

We add some properties and make note of which properties are calculated by calling as-yet written methods.
```cs
        public List<Desk> Desks { get; }
        public DateTime CreatedAt { get; }

        public int DeskCount => this.Desks is null ? this.Desks.Count : 0;

        public double ActualYield => CalculateYield();

        public double TargetYield { get; set; }

        public double Efficiency => CalculateEfficiency();

        public double Revenue { get; set; }
```

## Methods

The calculation methods are easy, performing some basic data validation & math:
```cs
        private double CalculateYield()
        {
            return this.DeskCount / this.sourceSurfaceArea;
        }
```
The efficiency is a bit more special as there's the possibility of dividing by zero, so we handle that case.
```cs
        private double CalculateEfficiency()
        {
            if (this.TargetYield == 0) throw new DivideByZeroException("Target yield cannot be 0.");
            return this.ActualYield / this.TargetYield * 100;
        }
```

## Constructor

But how is the `DeskArrangement` created ? Well, all we need to make an arragement is a bunch of `Desks` and the `Surface` they're being placed on, so we can record its area, etc.

These become the inputs to our constructor and we then check for nulls and record these inputs into properties.

```cs
        public DeskArrangement(List<Desk> desks, Surface surface) : base()
```

Add a surface field to the class:
```cs
        private const double DEFAULT_EFFICIENCY_TARGET = 0.8;
        private readonly double sourceSurfaceArea;
```
Notice that we don't expose the `surface` property. This is because users of the `DeskArrangement` class shouldn't be told about the surface as it's not the arrangement's job to keep track of surfaces. This is a good practice principle often referred to as `Encapsulation`.

```cs
        public DeskArrangement(List<Desk> desks, Surface surface) : base()
        {
            if (surface is null)
                throw new ArgumentNullException(nameof(surface));

            this.Desks = desks ?? throw new ArgumentNullException(nameof(desks));

            this.sourceSurfaceArea = surface.Area;
            this.TargetYield = DEFAULT_EFFICIENCY_TARGET * this.sourceSurfaceArea;
        }
```

Also note we created a constant, so that we can initialise the `TargetYield` to a default value, instead of it being 0.

The `DeskArchitect` class now needs to be updated, with its method now returning a `DeskArrangement`
```cs
        public static DeskArrangement LayoutDesksOnSurface(Surface surface)
```

At the bottom of the method, create a new `DeskArrangement` from the available variables :
```cs
            return new DeskArrangement(desks, surface);
```

