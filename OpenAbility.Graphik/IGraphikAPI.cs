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
	bool WindowShouldClose();
	void InitializeFrame();
	void FinishFrame();
	void Clear(ClearFlags clearFlags);
	ITexture CreateTexture();
	IMesh CreateMesh();
	IShader CreateShader();
	IRenderTexture CreateRenderTexture();
	void ResetTarget();
	void SetMouseState(MouseState state);
	public IShaderObject CreateShaderObject();
}
