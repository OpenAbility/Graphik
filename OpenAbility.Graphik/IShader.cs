namespace OpenAbility.Graphik;

public interface IShader
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
	public void BindInt(string name, int value);
	public void BindInt2(string name, int x, int y);
	public void BindInt3(string name, int x, int y, int z);
	public void BindInt4(string name, int x, int y, int z, int w);
	public void BindFloat(string name, float value);
	public void BindFloat2(string name, float x, float y);
	public void BindFloat3(string name, float x, float y, float z);
	public void BindFloat4(string name, float x, float y, float z, float w);
	public void BindDouble(string name, double value);
	public void BindDouble2(string name, double x, double y);
	public void BindDouble3(string name, double x, double y, double z);
	public void BindDouble4(string name, double x, double y, double z, double w);
	public void BindMatrix4(string name, bool transpose, float[] matrix);
	public void BindMatrix4(string name, bool transpose, float[,] matrix)
	{
		if (matrix.GetLength(0) < 4)
			throw new ArgumentException("Matrix is less than 4 wide", nameof(matrix));
		if (matrix.GetLength(1) < 4)
			throw new ArgumentException("Matrix is less than 4 height", nameof(matrix));

		float[] data =
		{
			matrix[0, 0], matrix[0, 1], matrix[0, 2], matrix[0, 3],
			matrix[1, 0], matrix[1, 1], matrix[1, 2], matrix[1, 3],
			matrix[2, 0], matrix[2, 1], matrix[2, 2], matrix[2, 3],
			matrix[3, 0], matrix[3, 1], matrix[3, 2], matrix[3, 3]
		};
		
		BindMatrix4(name, transpose, data);

	}
}
