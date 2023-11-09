# Graphik
*Grafik + Graphite = Graphik*  

Graphik is a simple cross-platform rendering API designed for work in an internal
game engine, alongside future porting of said engine.

The API is designed to simply abstract away stuff, and be similar to OpenGL, without
being bound to OpenGL-only code.

This means implementing an API for e.g DirectX can be a little odd at times.

## Getting Started
To setup a basic graphik process, all you need is the following code:
```csharp
// Set the backend, e.g GLAPI
Graphik.SetAPI(new MyAPI());
// Initialize any API systems
Graphik.InitializeSystems();

// Create a 1280x720 window with the title "Hello World, Graphik!"
Graphik.InitializeWindow("Hello World, Graphik!", 1280, 720);

// While the window shouldn't close, keep on running.
while (!Graphik.WindowShouldClose())
{
    // Start the new frame
    Graphik.InitializeFrame();
    // Clear the window
    Graphik.Clear(ClearFlags.Colour | ClearFlags.Depth);
	
    // Do rendering
	
    // Finish the frame
    Graphik.FinishFrame();
}
```
Most objects needed, e.g IMesh, IShader or ITexture can be created using
the `Graphik.Create` APIs, and should be fairly well-documented.

## Backend Selection
We recently added a new way of selecting your API of preference, accessible
via the `GraphikAPISelector` class. This class features methods of interest to most users.
```csharp
GraphikAPISelector.CreateSelection(APIRequest request); // Select a backend based on a request
GraphikAPISelector.Select(APISelector selector); // Let yourself decide upon a backed based on specs it provides 
```
Documentation for how to implement and use selection is available in [SELECTION.md](SELECTION.md)

# Shaders
Shaders in Graphik are(as of right now) written in the native shader language, however
we are aiming to change this.

Shaders are loaded as following:
- You feed your shader code and filename etc into ShaderCompiler
- Certain filenames are supported by the backend
  - For OpenGL, it's `.glsl`, `.frag` and `.vert`
  - These will be directly given to the compiler at hand.
- Others aren't, in which case we expect a json file
  - This json is a simple string-string dictionary
    - The string is a shader language without extension,
      e.g `hlsl`, `glsl` or `msl`.
    - The key is the relative path to the shader in that language
- The backend will be queried for supported languages, in a priority
  order
- It will then load the shader of that language(using `Graphik.FileReadHandler`, make sure
  you set it!)

# Graphik.Audio
Graphik.Audio is a system to play back sound through whatever backend the system may support.  

The library aims to provide an OOP approach to an OpenAL-like API.

## Heads-up
Currently the only implementation of Graphik.Audio is for OpenAL, and it uses `AL_EXT_float32`, which only appears to 
be available if you have OpenAL Soft installed.  
This means that by default, it doesn't work on windows.
