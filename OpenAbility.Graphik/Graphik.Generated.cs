// Auto-generated IGraphikAPI bindings!
// These should not be modified
using OpenAbility.Graphik;
using System.Numerics;

namespace OpenAbility.Graphik;
        
public static partial class Graphik
{
	public static void InitializeSystems() => api.InitializeSystems();
	public static void InitializeWindow(string title, int width, int height) => api.InitializeWindow(title, width, height);
	public static void SetErrorCallback(ErrorCallback errorCallback) => api.SetErrorCallback(errorCallback);
	public static void SetDebugCallback(DebugCallback debugCallback) => api.SetDebugCallback(debugCallback);
	public static void SetResizeCallback(ResizeCallback resizeCallback) => api.SetResizeCallback(resizeCallback);
	public static void SetKeyCallback(KeyCallback keyCallback) => api.SetKeyCallback(keyCallback);
	public static void SetMouseCallback(MouseCallback mouseCallback) => api.SetMouseCallback(mouseCallback);
	public static void SetCursorCallback(CursorCallback cursorCallback) => api.SetCursorCallback(cursorCallback);
	public static void SetTypeCallback(TypeCallback typeCallback) => api.SetTypeCallback(typeCallback);
	public static bool WindowShouldClose() => api.WindowShouldClose();
	public static void InitializeFrame() => api.InitializeFrame();
	public static void FinishFrame() => api.FinishFrame();
	public static void Clear(ClearFlags clearFlags) => api.Clear(clearFlags);
	public static ITexture2D CreateTexture() => api.CreateTexture();
	public static IMesh CreateMesh() => api.CreateMesh();
	public static IShader CreateShader() => api.CreateShader();
	public static IRenderTexture CreateRenderTexture() => api.CreateRenderTexture();
	public static void ResetTarget() => api.ResetTarget();
	public static void SetMouseState(MouseState state) => api.SetMouseState(state);
	public static IShaderObject CreateShaderObject() => api.CreateShaderObject();
	public static void SetFeature(Feature feature, bool enabled) => api.SetFeature(feature, enabled);
	public static void SetCullMode(CullFace cullFace) => api.SetCullMode(cullFace);
	public static void SetTexturePixelAlignment(int alignment) => api.SetTexturePixelAlignment(alignment);
	public static void SetWindowTitle(string title) => api.SetWindowTitle(title);
	public static void SetScissorArea(int x, int y, int width, int height) => api.SetScissorArea(x, y, width, height);
	public static ITexture2D FromNative(uint handle) => api.FromNative(handle);
	public static void SetBlending(BlendMode blendMode) => api.SetBlending(blendMode);
	public static void SetBlendFunction(BlendFactor a, BlendFactor b) => api.SetBlendFunction(a, b);
	public static void SetDepthFunction(DepthFunction depthFunction) => api.SetDepthFunction(depthFunction);
	public static void SetScrollCallback(ScrollCallback scrollCallback) => api.SetScrollCallback(scrollCallback);
	public static IController GetController(int controllerID) => api.GetController(controllerID);
	public static void UnbindTextures() => api.UnbindTextures();
	public static Vector2 ContentScale() => api.ContentScale();
	public static void LineWidth(float width) => api.LineWidth(width);
	public static void PointSize(float size) => api.PointSize(size);
	public static IShaderBuffer CreateShaderBuffer() => api.CreateShaderBuffer();
	public static ICubemapTexture CreateCubemap() => api.CreateCubemap();
#nullable enable
	public static IRenderTexture? GetBoundTarget() => api.GetBoundTarget();
#nullable disable
	public static void SetWindowIcons(params WindowIcon[] icons) => api.SetWindowIcons(icons);
}
