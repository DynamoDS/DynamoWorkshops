
# Exercise 4 - References to Dynamo objects, snippets

## Properties

```cs
        private ViewLoadedParams viewLoadedParams;
        private DynamoViewModel dynamoViewModel;
        private DynamoModel dynamoModel;
```

## Retain references

In `Loaded` method, add

**ViewLoadedParams**

```cs
            viewLoadedParams = vlp;
```

**DynamoViewModel**
```cs
dynamoViewModel = vlp.DynamoWindow.DataContext as DynamoViewModel;
```

**DynamoModel**

Let's use the DynamoViewModel reference we've already set.
```cs
dynamoModel = dynamoViewModel.Model as DynamoModel;
```

Now we can use these references in our `extension` or `view extension`.