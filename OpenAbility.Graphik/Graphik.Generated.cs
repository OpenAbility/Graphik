// Auto-generated IGraphikAPI bindings!
// These should not be modified
using OpenAbility.Graphik;
namespace OpenAbility.Graphik;
        
public static partial class Graphik
{
	public static void InitializeWindow(string title, int width, int height) => api.InitializeWindow(title, width, height);
	public static void InitializeSystems() => api.InitializeSystems();
	public static bool WindowShouldClose() => api.WindowShouldClose();
	public static void InitializeFrame() => api.InitializeFrame();
	public static void FinishFrame() => api.FinishFrame();
	public static void Clear(ClearFlags clearFlags) => api.Clear(clearFlags);
	public static ITexture CreateTexture() => api.CreateTexture();
	public static IMesh CreateMesh() => api.CreateMesh();
	public static IShader CreateShader() => api.CreateShader();
}
