# Mapping Object

In several cases object mappings are useful. When a mapping needs to be done 
in several places, it'd be nice to write mapping code only once - or not even 
at all. This is where the Mapping Object library (sustainably grown) may help 
you.

I know about the famous (and well done) 
[AutoMapper](https://github.com/AutoMapper/AutoMapper) library already, which 
could do everything you could do with my Mapping Object library, too - and 
even more. But in some szenarios AutoMapper may be just too much, while the 
Mapping Object library offers a simple and slender tool character, which may 
be more than enough in many cases already, I'd say.

While I use the Mapping Object library in my own projects, and it fits all of 
my requirements, I'd prefer to switch to AutoMapper in case it would make 
sense, rather than adding new features to the Mapping Object library. However, 
this doesn't mean that this version of the library is the final version 
already! But it means that the current feature set may not be extended so much 
in the future.

## How to get it

### Download the sources

You may download the sources and compile the projects by yourself. The Mapping 
Object Async library depends on the Mapping Object library. You may also 
include the source files into your project and compile them within your app.

### Install the NuGet package

You'll need to create a personal read-only access token for your GitHub 
account in order to be able to authenticate against the GitHub NuGet package 
source (add the `read:packages` scope for the token). Then you can execute 
these commands (replace `USERNAME` and `TOKEN` with your GitHub username and 
your personal read-only access token):

```bash
dotnet nuget add source "https://nuget.pkg.github.com/nd1012/index.json" --name "GitHub nd1012" --username "USERNAME" --password "TOKEN"
dotnet add package Mapping-Object
dotnet add package Mapping-Object-Async
```

For more details and help, please refer to the [GitHub documentation for installing a package](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#installing-a-package) 
and the [GitHub documentation for creating a personal access token](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token).

## Usage

**NOTE**: The main properties need a public getter/setter, while the source 
object properties need only a public getter (if you don't use reverse mapping, 
too).

### General

Often a mapping is being done in one way only - in this case there's a main 
object and a source object. The source objects properties will be mapped to 
the main object properties.

In a bi-directional mapping it's also required to map the main object 
properties back to the source object properties (reverse mapping).

In the best case, you don't need any mapping configuration, and all the work 
can be done using a single line of code:

```cs
using wan24.MappingObject;

// Map property values of sourceType to mainType properties
Mappings.MapFrom(sourceType, mainType);
```

Or the reverse mapping:

```cs
// Map mainType property values back to sourceType properties
Mappings.MapTo(mainType, sourceType);
```

The types of these examples may be:

```cs
public class MainType
{
	public bool MappedProperty { get; set; }
	
	[SkipMapping]
	public int NotMappedProperty { get; set; } = 1;
}

public class SourceType
{
	public bool MappedProperty { get; set; } = true;

	public int NotMappedProperty { get; set; }

	public int NotMappedProperty2 { get; set; }
}
```

**NOTE**: The mapping configuration needs to be done in the main type, only. 
Any mapping configuration in the source type doesn't have any effect.

You want to map `SourceType` to `MainType`, so you may register the mapping 
(which is optional and can be done automatic, if no customizations are being 
used):

```cs
Mappings.Add(typeof(SourceType), typeof(MainType));
```

**NOTE**: A type can also be an interface, an abstract type, or a generic type 
definition. The `Mappings.Find` method fill find the best matching mapping for 
a type pair, where the main object type has the first priority.

This will create a mapping for all public instance properties of `MainType` 
which have a public getter/setter and do not have the attribute `SkipMapping`. 
Properties that don't exist in the source type will be skipped.

This mapping can now be applied:

```cs
MainType main = new();
Mappings.MapFrom(new SourceType(), main);
Assert.IsTrue(main.MappedProperty);
Assert.AreEqual(1, main.NotMappedProperty);
```

In this case a reverse mapping is possible already, too:

```cs
SourceType source = new();
Mappings.MapTo(new MainType(), source);
Assert.IsFalse(source.MappedProperty);
Assert.AreEqual(0, source.NotMappedProperty);
```

**NOTE**: In general `Map*From` maps a source type to the main type, while 
`Map*To` maps a main type to a source type (reverse mapping).

### Use a different source property

The automated mapping uses the same property names for the main and the 
source types. In case you want to use different property names, you can use 
the `wan24.MappingObject.MapAttribute` attribute in the main type to specify 
another source object property name:

```cs
public class MainType
{
	// Use "nameof" to be safe for future modifications here
	[Map(nameof(SourceType.MappedProperty2))]
	public bool MappedProperty { get; set; }
}

public class SourceType
{
	public bool MappedProperty2 { get; set; } = true;
}

Mappings.Add(typeof(SourceType), typeof(MainType));// Optional - can still be done automatic

MainType main = new();
Mappings.MapFrom(new SourceType(), main);
Assert.IsTrue(main.MappedProperty);
```

This also affects the reverse mapping.

You could also skip the `Map` attribute and define the source type property 
name during the mapping registration:

```cs
Mappings.Add(
	typeof(SourceType), 
	typeof(MainType), 
	new Mapping(nameof(MainType.MappedProperty), nameof(SourceType.MappedProperty2))
);
```

**NOTE**: If a main type property wasn't found in the source type (during the 
automated mapping registration), it won't be mapped automatic.

### Convert the mapped value

You can define value converters in case the source value type needs to be 
converted before setting it in the main type property:

```cs
public class MainType
{
	public bool MappedProperty { get; set; }
}

public class SourceType
{
	public string MappedProperty { get; set; } = true.ToString();
}

Mappings.Add(
	typeof(SourceType), 
	typeof(MainType), 
	new Mapping(
		nameof(MainType.MappedProperty), 
		// Converts the source value for the main type property
		(v) => bool.Parse((string)v!), 
		// Converts the main value for the source type property (for reverse mapping)
		(v) => v!.ToString()
	)
);

MainType main = new();
Mappings.MapFrom(new SourceType(), main);
Assert.IsTrue(main.MappedProperty);
```

### Full mapping customization

To create a fully customized mapping using getter delegates:

```cs
public class MainType
{
	public string Name { get; set; } = null!;
}

public class SourceType
{
	public string FirstName { get; set; } = "John";
	
	public string LastName { get; set; } = "Doe";
}

Mappings.Add(
	typeof(SourceType), 
	typeof(MainType), 
	new Mapping(
		nameof(MainType.Name), 
		nameof(SourceType.FirstName), 
		// Converts the source value for the main type property
		sourceGetter: (source, main) => 
		{
			SourceType sourceType = (SourceType)source;
			return $"{sourceType.FirstName} {sourceType.LastName}";
		}, 
		// Converts the main value for the source type property (for reverse mapping)
		mainGetter: (main, source) => 
		{
			MainType mainType = (MainType)main;
			SourceType sourceType = (SourceType)source;
			string[] name = mainType.Name.Split(' ');
			sourceType.LastName = name[1];
			return name[0];// Will be set to "sourceType.FirstName", 'cause it's this mappings target property
		}
	)
);


// "sourceGetter" will be applied:
MainType main = new();
Mappings.MapFrom(new SourceType(), main);
Assert.AreEqual("John Doe", main.Name);

// "mainGetter" will be applied:
main.Name = "John Smith";
SourceType source = new();
Mappings.MapTo(main, source);
Assert.AreEqual("John", source.FirstName);
Assert.AreEqual("Smith", source.LastName);
```

Or using mapping delegates:

```cs
Mappings.Add(
	typeof(SourceType), 
	typeof(MainType), 
	new Mapping(
		// ID of the custom mapping
		nameof(MainType.Name),
		// Map source -> main
		(source, main) => 
		{
			SourceType sourceType = (SourceType)source;
			MainType mainType = (MainType)main;
			main.Name = $"{sourceType.FirstName} {sourceType.LastName}";
		},
		// Map main -> source (reverse mapping)
		(main, source) => 
		{
			MainType mainType = (MainType)main;
			SourceType sourceType = (SourceType)source;
			string[] name = mainType.Name.Split(' ');
			sourceType.FirstName = name[0];
			sourceType.LastName = name[1];
		}
	)
);
```

The difference between the two `Mappings.Add` calls is: The first call uses 
getter delegates, while the second call uses mapper delegates. A getter 
delegate returns the value to map, a mapper delegate does the whole mapping 
for one or multiple values (and returns nothing).

When using mapper delegates, the ID has to be unique for one mapping, and it 
doesn't need to match a property name (but if it does, it'd overwrite a 
generated property mapping). You could perform the whole object mapping within 
the mapping delegates or extend the automatic mappings with customizations. In 
this example nothing can be mapped automatic, and only the mapping delegates 
will do the work.

### The `MappingObjectBase` base type

Using the `MappingObjectBase` type allows to customize the mapping by 
overriding the `Map*` methods in your implementing type:

```cs
public class MainType : MappingObjectBase<SourceType>
{
	public MainType() : base() { }
	
	public MainType(SourceType source) : base(source) { }
	
	// Mapped properties may be here
	
	public override void MapFrom(SourceType source)
	{
		base.MapFrom(source);// Optional
		// Your custom source to main object mapping logic
	}
	
	public override void MapTo(SourceType source)
	{
		base.MapTo(source);// Optional
		// Your custom main to source object mapping logic
	}
}
```

You can configure the `MainType` properties as usual (using attributes) and 
implement custom mapping logic as required. The mappings will be created 
automatic for this type as for any other type which is being used as the main 
type for a mapping. To avoid that, you can register a custom mapping in the 
static constructor of your implementing type, for example.

The same functionality including casting you'll get using the 
`MappingObjectCastableBase` base class. Then you could cast like this:

```cs
public sealed class MainType : MappingObjectCastableBase<SourceType, MainType>
{
	...
}

MainType main = (MainType)source;// One explicit cast is still required :(
```

### The `IMappingObject` interface

The `IMappingObject` comes as generic and as non-generic type. Usually you 
want to use the generic interface, but you could also implement only the non-
generic interface, also.

For the implementation and the usage please have a look at the `TestType4.cs` 
code in the tests project. You can use the interface for objects that can't 
extend the `MappingObjectBase` type.

### Enumerable mapping extensions

You can use the `MapAllFrom` and `MapAllTo` enumerable extension methods for 
mapping a list of main and/or source objects:

```cs
// Convert source type objects to main type objects
MainType[] mainObjects = sourceObjects.MapAllFrom<SourceType, MainType>().ToArray();

// Convert main type objects to source type objects
SourceType[] sourceObjects = mainObjects.MapAllTo<MainType, SourceType>().ToArray();
```

The methods which accept a list of key/value pairs require the key to be the 
source object, while the value requires to be the target object for the 
mapping.

If required, you can use a factory method for creating the mapping target type 
object instances, too - otherwise instances will be created using the empty 
constructor of the target type.

## Mapping registration

I'd suggest to perform a mapping registration in the static constructor of the 
main type, which would be a kind of lazy loading. You don't have to register 
a mapping at all, if you don't use custom `Mapping` instances and all mapping 
configuration can be done in the main type (or isn't required at all).

If you use the `Mappings.Add` method which consumes a configuration, no 
automatic mapping will be created. To create the automatic mappings, use the 
`Mappings.Create` method.

## Asynchronous object mapping

Using the Mapping Object Async library the whole thing will be extended by 
asynchronous mapping delegates and methods which allow fully asynchronous 
work with continuous cancellation token support and asynchronous enumerables, 
too - an example using the `MappingObjectAsyncBase` base type:

```cs
public class MainType : MappingObjectAsyncBase<SourceType>
{
	...
	
	public override async Task MapFromAsync(
		SourceType source,
		CancellationToken cancellationToken = default
	)
	{
		await base.MapFromAsync(source, cancellationToken);// Optional
		// Apply your custom asynchronous mappings here
	}
	
	public override async Task MapToAsync(
		SourceType source,
		CancellationToken cancellationToken = default
	)
	{
		await base.MapToAsync(source, cancellationToken);// Optional
		// Apply your custom asynchronous mappings here
	}
}

public class SourceType
{
	...
}

MainType main = new();
SourceType source = new();

// Using "*Async" here - the synchronous methods will refer to the 
// asynchronous methods in the base type, which may optional be overridden, 
// too
await main.MapFromAsync(source);
await main.MapToAsync(source);

// Or using the static mapping methods:
await AsyncMappings.MapFromAsync(source, main);
await AsyncMappings.MapToAsync(main, source);
```

In case you need to use customized `Mapping` instances, use `AsyncMapping` 
instead (supported by `Mappings.Add*`, too). However, mixing synchronous 
`Mapping` and `AsyncMapping` works, when calling a `AsyncMapings.Map*` 
method. The `Mapping` constructors are not supported in the `AsyncMapping` 
type - instead use the similar constructors which use asynchronous delegates.

**NOTE**: Don't use the `Mappings.Map*` methods with types that are using 
asynchronous mappings. If the synchronous mapping methods are being used, a 
`NotSupportedException` would be thrown!

To adapt to asynchronous mapping, simply replace the used types:

- `Mappings` -> `AsyncMappings` (where possible!)
- `Mapping` -> `AsyncMapping` (where required)
- `MappingObjectBase` -> `MappingObjectAsyncBase`
- `IMappingObject` -> `IMappingObjectAsync`

The asynchronous enumerable mapping extensions have support for `IEnumerable` 
and `IAsyncEnumerable`. Also an asynchronous object factory method may be 
used, if applicable.

## Execute handler after/before mapping

When using the `Mappings.Add` method for a mapping registration, you'll get a 
`MappingConfig` as return value. In this mapping configuration you can set 
handler delegates for 

- handling objects before mapping
- handling objects after mapping
- handling objects before reverse mapping
- handling objects after reverse mapping

Those handlers will be executed when

- using any `Mappings.Map*` method
- (reverse) mapping a `MappingObjectBase`

but not when calling a `Map*` method of an `IMappingObject` directly. Within a 
method in an `IMappingObject` you can decide to load and execute possible 
handlers by evaluating the `applyDefaultMappings` parameter: If the value is 
`false`, handlers will be executed from the `Mappings.Map*` methods. Otherwise 
you may process as the `MappingObjectBase.Map*` do.

## Nested object mapping

In case

- the value isn't `null`, not a value type and can't be set to the target 
property and 
- the target property type isn't a value type and can be instanced (f.e. using 
a factory method (required for interface types)) and 

it's possible to map deep objects - in the best case without any manual 
mapping configuration.

Example:

```cs
public class A
{
	public C Value { get; set; } = new();
}

public class B
{
	public D Value { get; set; } = new();
}

public class C
{
	...
}

public class D
{
	...
}
```

For mapping `A` <-> `B`, the value of `Value` needs to be converted. If it's 
possible to map `C` <-> `D` (automatic or using a manual mapping 
configuration), the `A` <-> `B` mapping can be done automatic: Because the 
mapper can see that `C` can't be assigned to `D`, it tries automatic type 
conversion.

For the type conversion, the mapper will instance the required target value 
type, if the target property has no value yet (optional using a factory method 
(see `SourceInstanceFactory` and `MainInstanceFactory` properties of a 
`Mapping`)).

**WARNING**: It's possible to nest synchronous mapped objects into an 
asynchronous mapped object, but not the opposite (the parent object needs to 
be mapped asynchronous when mixing mapping methods!).
