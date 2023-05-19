namespace OpenAbility.Graphik;

public interface IShaderObject
{
	/// <summary>
	/// Attach a shader source for compilation. This is the first stage to building the shader.
	/// </summary>
	/// <param name="source">The source to attach</param>
	/// <param name="shaderType">The requested type of the source</param>
	public void AttachSource(string source, string filename, ShaderType shaderType);
	/// <summary>
	/// Compile whatever sources we have attached, this is the second step in building the shader.  
	/// Please note that this does not complete the shader, as
	/// you still need to link the program.
	/// </summary>
	/// <returns>The shader error log, or nothing if no error happened</returns>
	public ShaderCompilationResult Compile();
	public void Dispose();
}
