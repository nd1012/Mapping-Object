# Mapping Object

In several cases object mappings are useful. When a mapping needs to be done 
in several places, it'd be nice to write mapping code only once - or not even 
at all. This is where the Mapping Object library may help you.

I know about the well done (and famous) 
[AutoMapper](https://github.com/AutoMapper/AutoMapper) library already, which 
could do everything you could do with my Mapping Object library, too - and 
even more. But in some szenarios AutoMapper may be just too much, while the 
Mapping Object library offers a simple and slender tool character, which may 
be more than enough in many cases already, I'd say.

While I use the Mapping Object library in my own projects, and it fits all of 
my requirements, I'd prefer to switch to AutoMapper in case it would make 
sense, rather than adding new features to the Mapping Object library. However, 
this doesn't mean that this version of the library is the final version 
already! But it means that the current feature set may not be extended in the 
future too much.

## Usage

**NOTE**: The main properties need a public getter/setter, while the source 
object properties need only a public getter (if you don't use reverse mapping, 
too). The mappings are _not_ thread safe at the moment.

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
MainType main = new();
SourceType source = new();
Mappings.MapTo(main, source);
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

To create a fully customized mapping:

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
			string[] name = main.Name.Split(' ');
			sourceType.Lastname = name[1];
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

You can use the `Mappings.AddExplicit` method for registering a mapping which 
does not create `Mapping` instances automatic. Then you have to create a 
`Mapping` instance for each mapped property by yourself. If you don't give any 
`Mapping` instances, a previously registered mapping will be removed.
