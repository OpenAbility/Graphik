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
	 IShaderObject CreateShaderObject();
	 void SetFeature(Feature feature, bool enabled);
	 void SetCullMode(CullFace cullFace);
	 void SetTexturePixelAlignment(int alignment);
	 void SetWindowTitle(string title);
	 void SetScissorArea(int x, int y, int width, int height);
	 ITexture2D FromNative(uint handle);
	 void SetBlending(BlendMode blendMode);
	 void SetBlendFunction(BlendFactor a, BlendFactor b);
	void SetDepthFunction(DepthFunction depthFunction);
	IController GetController(int controllerID);
	void UnbindTextures();
}
