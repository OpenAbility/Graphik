// Auto-generated IGraphikAPI bindings!
// These should not be modified
using OpenAbility.Graphik;
namespace OpenAbility.Graphik;
        
public static partial class Graphik
{
	public static void InitializeSystems() => api.InitializeSystems();
	public static void InitializeWindow(string title, int width, int height) => api.InitializeWindow(title, width, height);
	public static void SetErrorCallback(ErrorCallback errorCallback) => api.SetErrorCallback(errorCallback);
	public static void SetResizeCallback(ResizeCallback resizeCallback) => api.SetResizeCallback(resizeCallback);
	public static void SetKeyCallback(KeyCallback keyCallback) => api.SetKeyCallback(keyCallback);
	public static void SetMouseCallback(MouseCallback mouseCallback) => api.SetMouseCallback(mouseCallback);
	public static void SetCursorCallback(CursorCallback cursorCallback) => api.SetCursorCallback(cursorCallback);
	public static void SetTypeCallback(TypeCallback typeCallback) => api.SetTypeCallback(typeCallback);
	public static bool WindowShouldClose() => api.WindowShouldClose();
	public static void InitializeFrame() => api.InitializeFrame();
	public static void FinishFrame() => api.FinishFrame();
	public static void Clear(ClearFlags clearFlags) => api.Clear(clearFlags);
	public static ITexture CreateTexture() => api.CreateTexture();
	public static IMesh CreateMesh() => api.CreateMesh();
	public static IShader CreateShader() => api.CreateShader();
	public static IRenderTexture CreateRenderTexture() => api.CreateRenderTexture();
	public static void ResetTarget() => api.ResetTarget();
	public static void SetMouseState(MouseState state) => api.SetMouseState(state);
	public static IShaderObject CreateShaderObject() => api.CreateShaderObject();
}
