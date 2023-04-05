namespace OpenAbility.Graphik;

public interface IGraphikAPI
{
	void InitializeWindow(string title, int width, int height);
	void InitializeSystems();
	bool WindowShouldClose();
	void InitializeFrame();
	void FinishFrame();
	void Clear(ClearFlags clearFlags);
	ITexture CreateTexture();
	IMesh CreateMesh();
	IShader CreateShader();
}