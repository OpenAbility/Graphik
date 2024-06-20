namespace OpenAbility.Graphik;

public interface IShaderObject : IDisposable
{
	/// <summary>
	/// Build a shader object from the compiled shader
	/// </summary>
	/// <param name="compiledShader">The compiled shader</param>
	public ShaderBuildResult Build(CompiledShader compiledShader);
	public void SetName(string name);
}
