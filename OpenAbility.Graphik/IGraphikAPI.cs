// Auto-generated from GLAPI from OpenAbility.Graphik.OpenGL
// These should not be modified
namespace OpenAbility.Graphik;
using OpenAbility.Graphik;
public interface IGraphikAPI
{
	void InitializeSystems();
	void InitializeWindow(string title, int width, int height);
	void SetErrorCallback(ErrorCallback errorCallback);
	void SetResizeCallback(ResizeCallback resizeCallback);
	void SetKeyCallback(KeyCallback keyCallback);
	void SetMouseCallback(MouseCallback mouseCallback);
	void SetCursorCallback(CursorCallback cursorCallback);
	void SetTypeCallback(TypeCallback typeCallback);
	void SetScrollCallback(ScrollCallback scrollCallback);
	bool WindowShouldClose();
	void InitializeFrame();
	void FinishFrame();
	void Clear(ClearFlags clearFlags);
	ITexture2D CreateTexture();
	IMesh CreateMesh();
	IShader CreateShader();
	IRenderTexture CreateRenderTexture();
	void ResetTarget();
	void SetMouseState(MouseState state);
	public IShaderObject CreateShaderObject();
	public void SetFeature(Feature feature, bool enabled);
	public void SetCullMode(CullFace cullFace);
	public void SetTexturePixelAlignment(int alignment);
	public void SetWindowTitle(string title);
	public void SetScissorArea(int x, int y, int width, int height);
	public ITexture2D FromNative(uint handle);
	public void SetBlending(BlendMode blendMode);
	public void SetBlendFunction(BlendFactor a, BlendFactor b);
	public void SetDepthFunction(DepthFunction depthFunction);
}
