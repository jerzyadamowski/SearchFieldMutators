# Search Field Mutators - library to support mixing repository data with dynamic filter rules

Idea was borrow long time ago from https://gist.github.com/Grinderofl/2767155

Here we fix all error and we were extensively using this soultion in our projects.

## How does it work?

First you to have to create your repository or just create simple collection.
Our lib support IQueryable interface and all your data filters you can do on expresions rather than real data. This save you a lot of performance and remove unnecessary sql calls.

```C#
List<User> userRepository = new List<User>()
{
    new User()
    {
        Name = "Joe",
        Age = 18,
        Department = "HR"
    },
    new User()
    {
        Name = "Jay",
        Age = 21,
        Department = "Administration"
    },
    new User()
    {
        Name = "Nicole",
        Age = 18,
        Department = "Finance"
    },
};
```

We got our data collection - next step is setup filter rules.
Filter rules behave as object so code is open to extensions - we totally avoid if or switch syntax.
You can mix unlimited numbers of rules.

```C#
var searchRules = new SearchFieldMutators<User, SearchFieldVM>();

searchRules.Add(
    test => !string.IsNullOrWhiteSpace(test.InputText),
    (query, search) => query.Where(x => x.Name.Contains(search.InputText)));

searchRules.Add(
    test => test.MinAge > 0,
    (query, search) => query.Where(x => x.Age >= search.MinAge));

searchRules.Add(
    test => test.MaxAge > 0,
    (query, search) => query.Where(x => x.Age <= search.MaxAge));

```

First we setup collection filters. We need to decide which type of model we want to interferee. Here i used model `User`. Then we need to decide about filter model. I choose `SearchFieldVM`. Normally you can use whatever model for data and for filter.
```C#
var searchRules = new SearchFieldMutators<User, SearchFieldVM>();
```

Now we store usefull rules in collection:
```C#
searchRules.Add(
    test => !string.IsNullOrWhiteSpace(test.InputText),
    (query, search) => query.Where(x => x.Name.Contains(search.InputText)));
```

What does it mean? Well first part of parameters in function `Add` control about do we have to take this rule under consideration. It works as operator OR ||. If `InputText` from `SearchFieldVM` model will be not empty that entire predicate active second part of our search rule. If `InputText` will be empty that second part will be skiped.

In second part we combine query expresion from model `User` with search expresion from `SearchFieldVM`. If we look closer our filter rule will be true only if `User.Name` contains `SearchFieldVM.InputText`. So here we mix potential data with potential filter.

Next example:
```C#
searchRules.Add(
    test => test.MinAge > 0,
    (query, search) => query.Where(x => x.Age >= search.MinAge));
```

With this case we check if our user had at least minmum required age. MinAge is int but could be int? so this case could be change to `test => test.MinAge.HasValue`

Lets go to nex part.
What is great about this you dont need to specify data, numbers, strings. Because if you set rule you could change real filter data every time when use change input for instance.

Lets define some data:

```C#
SearchFieldVM modelSearch = new SearchFieldVM()
{
    InputText = "J",
    MinAge = 17,
    MaxAge = 19
};
```

So we are looking for people which have capital J in `Name` and they greater or equal than 17 years old and lesser or equal than 19 years old.

Lets mix it

```C#
var collection = userRepository.AsQueryable();
            collection = collection.FilterMutator<User, SearchFieldVM>(searchRules, modelSearch);

```

Our repository stays in memory but this example could be with linq query combined. 
`FilterMutator` is extension method which expand features to our collection. If we run it we mix current `searchRules` with current `modelSearch`.

Collection result will be only with single person:
```C#
new User()
{
    Name = "Joe",
    Age = 18,
    Department = "HR"
},
```
Only Joe Fullfill our requirements.

Lets change filter data
```C#
SearchFieldVM modelSearch = new SearchFieldVM()
{
    InputText = null,
    MinAge = 17,
    MaxAge = 19
};

var collection = userRepository.AsQueryable();
collection = collection.FilterMutator<User, SearchFieldVM>(searchRules, modelSearch);
```

In this case with same rules:
```C#
new User()
{
    Name = "Joe",
    Age = 18,
    Department = "HR"
},
new User()
{
    Name = "Nicole",
    Age = 18,
    Department = "Finance"
},
```

Winner is Joe and Nicole