# Backend Selection
Backend selection allows the end user to easily integrate cross-api graphics using 
Graphik, without having to worry about stuff like compiler flags and whatnot.

## APIRequest
The APIRequest struct is useful for giving a list of criteria that various 
backends can use to rate how well they'd fit in a given environment.

These criteria are/should be to some extent documented in source using XML docs,
and shouldn't need to be specified here as of right now.

## APISpecification
The `APISpecification` type acts as a big lookup table, you give it a string key and
it returns a string value.

There are a number of recommended keys that should be defined.  

|name|description|extra info|
|----|-----------|----------|
|name|The backend name|See a list of known possibilities later|
|description|A simple description for debug purposes|-|
|version|The backend version|Should follow major.minor.subminor at ALL times|
|api|The graphics API used|Should be lowercase, see list of common names|
|stability|The stability of backends|See list later|
|priority|The priority of backends|See rating section|

### Known names
Here's a list of known backends, please open a PR to have your added
- OpenAbility.Graphik.OpenGL - The official OpenGL backend

### Stability Declaration
Stability declaration is how stable the backend perceives itself to be.
The levels are:
- `stable` - It conforms to the API 99-100% of the times without hassle
- `conforms` - All API methods are to some extent implemented, with little to no issues.
- `functional` - One or two methods might silently error
- `unstable` - It has a large amount of silent errors or maybe even NotSupportedException

There isn't currently a golden standard, just go with what most libraries do.

### Common Graphics APIs
- `opengl` - OpenGL, you know, the library
  - `opengl4` - Uses OpenGL 4.0 or newer
  - `opengl4.x` - Uses OpenGL version 4.x
  - `opengl3` - Uses OpenGL 3.0 or newer
  - `opengl-legacy` - Uses OpenGL 2.1 or older
  - `opengl-es` - Uses OpenGL ES
    - `opengl-es1` - Uses OpenGL ES 1.x
    - `opengl-es2` - Uses OpenGL ES 2.x
    - `opengl-es3` - Uses OpenGL ES 3.x
- `vulkan` - The Vulkan library
  - `vulkan1.0` - Uses Vulkan 1.0
  - `vulkan1.1` - Uses Vulkan 1.1
  - `vulkan1.2` - Uses Vulkan 1.2
  - `vulkan1.3` - Uses Vulkan 1.3
- `directx` - DirectX/Direct3D
  - `directx9` - Uses DirectX 9
  - `directx10` - Uses DirectX 10
  - `directx11` - Uses DirectX 11
  - `directx12` - Uses DirectX 12
- `metal` - The Apple Metal API
- `ps4` - The PS4 Gnm API
  - `ps4-x` - The PS4 Gnmx API

## Self-Score Tips and Tricks
When you make a backend, and you want to add it to the
selection service, there are a couple things that need to be
done!

First off, what we recommend you do is to create a so-called
*Module Initializer*. This is a function that runs when the
library is loaded.

This can be done by adding this to one of your classes:
```csharp
#pragma warning disable CA2255
[ModuleInitializer]
#pragma warning restore CA2255

internal static void InitializeModule()
{
	//...
}
```
The `#pragma` lines can be removed.

Inside `InitializeModule`, you should then register 
your backend.

Here's how to do that:
```csharp
// Constructor parameters:
// creator, rater, specifier, priority
GraphikAPIProvider graphikAPIProvider = new GraphikAPIProvider(Create, Rate, Specifier, 0);
GraphikAPISelector.RegisterProvider(graphikAPIProvider);
```
Here's what the different constructor parameters do:
- `creator` - Function which returns an `IGraphikAPI`(instance of your API)
- `rater` - Function which returns an `ulong` with the backend rating(we'll cover this one later)
- `specifier` - Function which returns the [package specification](#apispecification)
- `priority` - Used to prioritise your API over others(covered alongside ratings)

### Rating your backend
Rating of your backend is done by taking in a `APIRequest`, and deeming
how suitable the backend is for that request. A simple way to do this
would be to rate on a scale from 1-1000 how many per mille(10th of percent, ‰)
of criteria are satisfied.

There are of course many ways to do this, but we'd recommend staying in the
1-1000 range for your score(and no 1000's only, that's a bad idea).

#### Priorities
You may also prioritise your backend over someone else's, or do the opposite,
and this is set via the `priority` parameter. This is request independent, and
should only be used to mark stuff like backend completeness.

Try to keep this below 100, 50 if possible. Negative values can go as low
as you want, but in order to prevent competitions, keep it above -100 if possible.

## A final note
Backend selection works fine for most cases, however we do not
recommend using it if you have multiple backends that might compete.

It should realistically only be used to do stuff like automatically use
DirectX on windows.

If you truly want the best experience, try to use `Select`, as it will
provide you with manual control over ratings.