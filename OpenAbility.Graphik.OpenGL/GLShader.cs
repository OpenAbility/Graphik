using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenAbility.Graphik.OpenGL;



public class GLShader : IShader
{
	private ProgramHandle handle;
	private List<GLShaderObject> shaders = new List<GLShaderObject>();
	public GLShader()
	{
		handle = GL.CreateProgram();
	}

	public void Attach(IShaderObject shaderObject)
	{
		GLShaderObject glShaderObject = (GLShaderObject)shaderObject;
		shaders.Add(glShaderObject);
		GL.AttachShader(handle, glShaderObject.ShaderHandle);
	}

	public string Link()
	{
		GL.LinkProgram(handle);

		GL.GetProgramInfoLog(handle, out string log);
		foreach (var shader in shaders)
		{
			GL.DetachShader(handle, shader.ShaderHandle);
		}

		int activeCount = 0;
		GL.GetProgrami(handle, ProgramPropertyARB.ActiveUniforms, ref activeCount);
		//Console.WriteLine($"Program has {activeCount} active uniforms!");

		int bufSize = 64;
		int length = 0;
		int size = 0;
		UniformType type = 0;

		for (int i = 0; i < activeCount; i++)
		{
			string name = GL.GetActiveUniform(handle, (uint)i, bufSize, ref length, ref size, ref type);
			//Console.WriteLine($"| {name}: ID: {i}, Type: {type}, Size: {size}");
		}
		if (!String.IsNullOrEmpty(log))
			return log;
		return String.Empty;
	}

	public void Use()
	{
		GL.UseProgram(handle);
	}
	private int GetUniformLocation(string name)
	{
		return GL.GetUniformLocation(handle, name);
	}

	public void BindInt(string name, int value) => GL.Uniform1i(GetUniformLocation(name), value);
	public void BindInt2(string name, int x, int y) => GL.Uniform2i(GetUniformLocation(name), x, y);
	public void BindInt3(string name, int x, int y, int z) => GL.Uniform3i(GetUniformLocation(name), x, y, z);
	public void BindInt4(string name, int x, int y, int z, int w) => GL.Uniform4i(GetUniformLocation(name), x, y, z, w);
	public void BindFloat(string name, float value)  => GL.Uniform1f(GetUniformLocation(name), value);
	public void BindFloat2(string name, float x, float y) => GL.Uniform2f(GetUniformLocation(name), x, y);
	public void BindFloat3(string name, float x, float y, float z) => GL.Uniform3f(GetUniformLocation(name), x, y, z);
	public void BindFloat4(string name, float x, float y, float z, float w) => GL.Uniform4f(GetUniformLocation(name), x, y, z, w);
	public void BindDouble(string name, double value)  => GL.Uniform1d(GetUniformLocation(name), value);
	public void BindDouble2(string name, double x, double y) => GL.Uniform2d(GetUniformLocation(name), x, y);
	public void BindDouble3(string name, double x, double y, double z) => GL.Uniform3d(GetUniformLocation(name), x, y, z);
	public void BindDouble4(string name, double x, double y, double z, double w) => GL.Uniform4d(GetUniformLocation(name), x, y, z, w);
	public void BindMatrix4(string name, bool transpose, float[] matrix)
	{
		if (matrix.Length < 16)
			throw new ArgumentException("Matrix data provided is less that 16 in length(4x4=16)", nameof(matrix));
		
		GL.UniformMatrix4f(GetUniformLocation(name), transpose, new Matrix4(
			matrix[0], matrix[1], matrix[2], matrix[3],
			matrix[4], matrix[5], matrix[6], matrix[7],
			matrix[8], matrix[9], matrix[10], matrix[11],
			matrix[12], matrix[13], matrix[14], matrix[15]));
	}
	
	public void Dispose()
	{
		GL.DeleteProgram(handle);
	}

	public void BindAttribute(string name, int index)
	{
		GL.BindAttribLocation(handle, (uint)index, name);
	}
		
}