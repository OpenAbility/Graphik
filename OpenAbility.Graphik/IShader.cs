namespace OpenAbility.Graphik;

public interface IShader
{
	public void Attach(IShaderObject shaderObject);
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
	public void BindUInt(string name, uint value);
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

	private static readonly float[] PreAllocatedMatrixData = new float[16];
	
	public void BindMatrix4(string name, bool transpose, float[,] matrix)
	{
		if (matrix.GetLength(0) < 4)
			throw new ArgumentException("Matrix is less than 4 wide", nameof(matrix));
		if (matrix.GetLength(1) < 4)
			throw new ArgumentException("Matrix is less than 4 height", nameof(matrix));

		for (int i = 0; i < 4; i++)
		{
			PreAllocatedMatrixData[i * 4 + 0] = matrix[i, 0];
			PreAllocatedMatrixData[i * 4 + 1] = matrix[i, 1];
			PreAllocatedMatrixData[i * 4 + 2] = matrix[i, 2];
			PreAllocatedMatrixData[i * 4 + 3] = matrix[i, 3];
		}
		

		BindMatrix4(name, transpose, PreAllocatedMatrixData);
	}

	public void BindAttribute(string name, int index);
	public void DispatchCompute(int x, int y, int z);
	
	public void Dispose();
	/// <summary>
	/// Get if a uniform exists
	/// </summary>
	/// <param name="name">The name of the uniform</param>
	/// <returns>If it exists. Unsupported defaults to true</returns>
	public virtual bool UniformExists(string name)
	{
		return true;
	}
	public void SetName(string name);
}
