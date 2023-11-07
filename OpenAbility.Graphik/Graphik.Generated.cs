using OpenAbility.Graphik;
using System.Numerics;

namespace OpenAbility.Graphik;
        
public static partial class Graphik
{
	/// <summary>
	/// Initialize any required systems
	/// </summary>
	public static void InitializeSystems() => api.InitializeSystems();
	/// <summary>
	/// Create a window
	/// </summary>
	/// <param name="title">The title of the window</param>
	/// <param name="width">The (client) width</param>
	/// <param name="height">The (client) height</param>
	public static void InitializeWindow(string title, int width, int height) => api.InitializeWindow(title, width, height);
	/// <summary>
	/// Set the error message callback
	/// </summary>
	/// <param name="errorCallback">Callback, gets stored on API end</param>
	public static void SetErrorCallback(ErrorCallback errorCallback) => api.SetErrorCallback(errorCallback);
	/// <summary>
	/// Set the debug message callback
	/// </summary>
	/// <remarks>Callback recieves errors if no error handler is set</remarks>
	/// <param name="debugCallback">Callback, stored on API end</param>
	public static void SetDebugCallback(DebugCallback debugCallback) => api.SetDebugCallback(debugCallback);
	/// <summary>
	/// Set the resize callback
	/// </summary>
	/// <param name="resizeCallback">The resize callback, stored on the API side</param>
	public static void SetResizeCallback(ResizeCallback resizeCallback) => api.SetResizeCallback(resizeCallback);
	/// <summary>
	/// Set the key input callback
	/// </summary>
	/// <param name="keyCallback">Callback, stored on API end</param>
	public static void SetKeyCallback(KeyCallback keyCallback) => api.SetKeyCallback(keyCallback);
	/// <summary>
	/// Set the mouse input callback
	/// </summary>
	/// <param name="mouseCallback">Callback, stored on API end</param>
	public static void SetMouseCallback(MouseCallback mouseCallback) => api.SetMouseCallback(mouseCallback);
	/// <summary>
	/// Set the cursor move callback
	/// </summary>
	/// <param name="cursorCallback">Callback, stored on API end</param>
	public static void SetCursorCallback(CursorCallback cursorCallback) => api.SetCursorCallback(cursorCallback);
	/// <summary>
	/// Set the typing callback
	/// </summary>
	/// <param name="typeCallback">Callback, stored on API end</param>
	public static void SetTypeCallback(TypeCallback typeCallback) => api.SetTypeCallback(typeCallback);
	/// <summary>
	/// Get if the window should close
	/// </summary>
	/// <returns></returns>
	public static bool WindowShouldClose() => api.WindowShouldClose();
	/// <summary>
	/// Begin a new window frame
	/// </summary>
	public static void InitializeFrame() => api.InitializeFrame();
	/// <summary>
	/// Finish a window frame(swaps buffers etc)
	/// </summary>
	public static void FinishFrame() => api.FinishFrame();
	/// <summary>
	/// Clear the window buffer
	/// </summary>
	/// <param name="clearFlags">What to clear</param>
	public static void Clear(ClearFlags clearFlags) => api.Clear(clearFlags);
	/// <summary>
	/// Create a 2D texture
	/// </summary>
	/// <returns>The texture object</returns>
	public static ITexture2D CreateTexture() => api.CreateTexture();
	/// <summary>
	/// Create a 3D mesh
	/// </summary>
	/// <returns>The mesh object</returns>
	public static IMesh CreateMesh() => api.CreateMesh();
	/// <summary>
	/// Create a shader program
	/// </summary>
	/// <returns>The shader program object</returns>
	public static IShader CreateShader() => api.CreateShader();
	/// <summary>
	/// Create a render texture
	/// </summary>
	/// <returns>The render texture object</returns>
	public static IRenderTexture CreateRenderTexture() => api.CreateRenderTexture();
	/// <summary>
	/// Reset the render target
	/// </summary>
	public static void ResetTarget() => api.ResetTarget();
	/// <summary>
	/// Set the mouse state(e.g captured)
	/// </summary>
	/// <param name="state">The new mouse state</param>
	public static void SetMouseState(MouseState state) => api.SetMouseState(state);
	/// <summary>
	/// Create a shader obect(for program linking)
	/// </summary>
	/// <returns>The shader object object</returns>
	public static IShaderObject CreateShaderObject() => api.CreateShaderObject();
	/// <summary>
	/// Set a graphics feature usage
	/// </summary>
	/// <param name="feature">The feature to set</param>
	/// <param name="enabled">If the feature is enabled or not</param>
	public static void SetFeature(Feature feature, bool enabled) => api.SetFeature(feature, enabled);
	/// <summary>
	/// Set the face to cull
	/// </summary>
	/// <param name="cullFace">The face to cull</param>
	public static void SetCullMode(CullFace cullFace) => api.SetCullMode(cullFace);
	/// <summary>
	/// Set the pixel alignment of textures(useful for font rendering)
	/// </summary>
	/// <param name="alignment">The new alignment</param>
	public static void SetTexturePixelAlignment(int alignment) => api.SetTexturePixelAlignment(alignment);
	/// <summary>
	/// Set a new window title
	/// </summary>
	/// <param name="title">The title</param>
	public static void SetWindowTitle(string title) => api.SetWindowTitle(title);
	
	/// <summary>
	/// Set the scissor/clipping rect/area
	/// </summary>
	/// <param name="x">The top left X coords</param>
	/// <param name="y">The top left Y coords</param>
	/// <param name="width">The width of the rect</param>
	/// <param name="height">The height of the rect</param>
	public static void SetScissorArea(int x, int y, int width, int height) => api.SetScissorArea(x, y, width, height);
	/// <summary>
	/// Get a texutre from a native handle
	/// </summary>
	/// <param name="handle">The texture handle</param>
	/// <returns>The texture object</returns>
	public static ITexture2D FromNative(uint handle) => api.FromNative(handle);
	/// <summary>
	/// Set the blend mode
	/// </summary>
	/// <param name="blendMode"></param>
	public static void SetBlending(BlendMode blendMode) => api.SetBlending(blendMode);
	/// <summary>
	/// Set the blending function
	/// </summary>
	/// <param name="a">Factor A</param>
	/// <param name="b">Factor B</param>
	public static void SetBlendFunction(BlendFactor a, BlendFactor b) => api.SetBlendFunction(a, b);
	/// <summary>
	/// Set the depth test function
	/// </summary>
	/// <param name="depthFunction">The depth test function</param>
	public static void SetDepthFunction(DepthFunction depthFunction) => api.SetDepthFunction(depthFunction);
	/// <summary>
	/// Set the scroll callback
	/// </summary>
	/// <param name="scrollCallback">The callback</param>
	public static void SetScrollCallback(ScrollCallback scrollCallback) => api.SetScrollCallback(scrollCallback);
	/// <summary>
	/// Get a controller
	/// </summary>
	/// <param name="controllerID">The controller ID(0-whatever)</param>
	/// <returns>The controller object</returns>
	public static IController GetController(int controllerID) => api.GetController(controllerID);
	/// <summary>
	/// Unbind textures(?)
	/// </summary>
	public static void UnbindTextures() => api.UnbindTextures();
	/// <summary>
	/// Get the content scale for e.g HiDpi support
	/// </summary>
	/// <returns>The content scale</returns>
	public static Vector2 ContentScale() => api.ContentScale();
	/// <summary>
	/// Set the line render width
	/// </summary>
	/// <param name="width">The width of lines</param>
	public static void LineWidth(float width) => api.LineWidth(width);
	/// <summary>
	/// Set the base point side
	/// </summary>
	/// <param name="size">The point size</param>
	public static void PointSize(float size) => api.PointSize(size);
	/// <summary>
	/// Create a shader buffer(SSBO)
	/// </summary>
	/// <returns>The shader buffer object</returns>
	public static IShaderBuffer CreateShaderBuffer() => api.CreateShaderBuffer();
	/// <summary>
	/// Create a cubemap texture
	/// </summary>
	/// <returns>The cubemap texture object</returns>
	public static ICubemapTexture CreateCubemap() => api.CreateCubemap();
#nullable enable
	/// <summary>
	/// Get the currently bound render texutre
	/// </summary>
	/// <returns>The current render texture, or null if the screen buffer is bound</returns>
	public static IRenderTexture? GetBoundTarget() => api.GetBoundTarget();
#nullable disable
	/// <summary>
	/// Set the window icons if possible
	/// </summary>
	/// <param name="icons">The window icons</param>
	public static void SetWindowIcons(params WindowIcon[] icons) => api.SetWindowIcons(icons);
}
