namespace OpenAbility.Graphik;

public interface IShaderObject
{
	/// <summary>
	/// Build a shader object from the compiled shader
	/// </summary>
	/// <param name="compiledShader">The compiled shader</param>
	public ShaderBuildResult Build(CompiledShader compiledShader);
	public void Dispose();
}
