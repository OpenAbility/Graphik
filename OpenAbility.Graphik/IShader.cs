namespace OpenAbility.Graphik;

public interface IShader
{
	/// <summary>
	/// Attach a shader source for compilation. This is the first stage to building the shader.
	/// </summary>
	/// <param name="source">The source to attach</param>
	/// <param name="shaderType">The requested type of the source</param>
	public void AttachSource(string source, ShaderType shaderType);
	/// <summary>
	/// Compile whatever sources we have attached, this is the second step in building the shader.  
	/// Please note that this does not complete the shader, as
	/// you still need to link the program.
	/// </summary>
	/// <returns>The shader error log, or nothing if no error happened</returns>
	public string Compile();
	/// <summary>
	/// Link the shader, this is the final step in building a shader.
	/// If this is called before compiling, the end result is undefined
	/// </summary>
	/// <returns>The shader error log, or nothing if no error happened</returns>
	public string Link();
	/// <summary>
	/// Bind a shader for usage
	/// </summary>
	public void Use();
}
