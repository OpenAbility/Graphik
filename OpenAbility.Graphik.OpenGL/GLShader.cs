using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Reflection.Metadata;

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
		OpenTK.Graphics.OpenGL.UniformType type = 0;

		for (int i = 0; i < activeCount; i++)
		{
			string name = GL.GetActiveUniform(handle, (uint)i, bufSize, ref length, ref size, ref type);
			//Console.WriteLine($"| {name}: ID: {i}, Type: {type}, Size: {size}");
			uniforms[name] = GL.GetUniformLocation(handle, name);
		}
		
		return !String.IsNullOrEmpty(log) ? log : String.Empty;

	}

	public void Use()
	{
		GL.UseProgram(handle);
	}

	private readonly Dictionary<string, int> uniforms = new Dictionary<string, int>();

	private int GetUniformLocation(string name)
	{
		if (uniforms.TryGetValue(name, out int uniform))
			return uniform;
		uniform = GL.GetUniformLocation(handle, name);
		uniforms[name] = uniform;
		return uniform;
	}

	public bool UniformExists(string name)
	{
		return GetUniformLocation(name) != -1;
	}
	
	public void SetName(string name)
	{
		GLAPI.SetLabel(ObjectIdentifier.Program, handle.Handle, name);
	}
	
	public int ShaderBufferLocation(string name)
	{
		return GL.GetProgramResourceLocation(handle, ProgramInterface.ShaderStorageBlock, name);
	}
	
	public UniformType GetUniformType(int id)
	{
		int bufSize = 64;
		int length = 0;
		int size = 0;
		OpenTK.Graphics.OpenGL.UniformType type = 0;

		string name = GL.GetActiveUniform(handle, (uint)id, bufSize, ref length, ref size, ref type);

		return GetUniformType(type);
	}
	
	public int GetUniformCount()
	{
		int activeCount = 0;
		GL.GetProgrami(handle, ProgramPropertyARB.ActiveUniforms, ref activeCount);
		return activeCount;
	}
	
	public string GetUniformName(int id)
	{
		int bufSize = 64;
		int length = 0;
		int size = 0;
		OpenTK.Graphics.OpenGL.UniformType type = 0;

		string name = GL.GetActiveUniform(handle, (uint)id, bufSize, ref length, ref size, ref type);

		return name;
	}
	
	public void BindUInt(string name, uint value) => GL.ProgramUniform1ui(handle, GetUniformLocation(name), value);
	public void BindInt(string name, int value) => GL.ProgramUniform1i(handle, GetUniformLocation(name), value);
	public void BindInt2(string name, int x, int y) => GL.ProgramUniform2i(handle, GetUniformLocation(name), x, y);
	public void BindInt3(string name, int x, int y, int z) => GL.ProgramUniform3i(handle, GetUniformLocation(name), x, y, z);
	public void BindInt4(string name, int x, int y, int z, int w) => GL.ProgramUniform4i(handle, GetUniformLocation(name), x, y, z, w);
	public void BindFloat(string name, float value)  => GL.ProgramUniform1f(handle, GetUniformLocation(name), value);
	public void BindFloat2(string name, float x, float y) => GL.ProgramUniform2f(handle, GetUniformLocation(name), x, y);
	public void BindFloat3(string name, float x, float y, float z) => GL.ProgramUniform3f(handle, GetUniformLocation(name), x, y, z);
	public void BindFloat4(string name, float x, float y, float z, float w) => GL.ProgramUniform4f(handle, GetUniformLocation(name), x, y, z, w);
	public void BindDouble(string name, double value)  => GL.ProgramUniform1d(handle, GetUniformLocation(name), value);
	public void BindDouble2(string name, double x, double y) => GL.ProgramUniform2d(handle, GetUniformLocation(name), x, y);
	public void BindDouble3(string name, double x, double y, double z) => GL.ProgramUniform3d(handle, GetUniformLocation(name), x, y, z);
	public void BindDouble4(string name, double x, double y, double z, double w) => GL.ProgramUniform4d(handle, GetUniformLocation(name), x, y, z, w);
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

	public void DispatchCompute(int x, int y, int z)
	{
		Use();
		GL.DispatchCompute((uint)x, (uint)y, (uint)z);
	}
	public void Dispose()
	{
		GL.DeleteProgram(handle);
	}

	public void BindAttribute(string name, int index)
	{
		GL.BindAttribLocation(handle, (uint)index, name);
	}


	private static UniformType GetUniformType(OpenTK.Graphics.OpenGL.UniformType uniformType)
	{
		return uniformType switch
		{

			OpenTK.Graphics.OpenGL.UniformType.Int => UniformType.Int,
			OpenTK.Graphics.OpenGL.UniformType.UnsignedInt => UniformType.UInt,
			OpenTK.Graphics.OpenGL.UniformType.Float => UniformType.Float,
			OpenTK.Graphics.OpenGL.UniformType.Double => UniformType.Double,
			OpenTK.Graphics.OpenGL.UniformType.FloatVec2 => UniformType.Vector2,
			OpenTK.Graphics.OpenGL.UniformType.FloatVec3 => UniformType.Vector3,
			OpenTK.Graphics.OpenGL.UniformType.FloatVec4 => UniformType.Vector4,
			OpenTK.Graphics.OpenGL.UniformType.IntVec2 => UniformType.Vector2Int,
			OpenTK.Graphics.OpenGL.UniformType.IntVec3 => UniformType.Vector3Int,
			OpenTK.Graphics.OpenGL.UniformType.IntVec4 => UniformType.Vector4Int,
			OpenTK.Graphics.OpenGL.UniformType.Bool => UniformType.Bool,
			OpenTK.Graphics.OpenGL.UniformType.BoolVec2 => UniformType.Vector2Bool,
			OpenTK.Graphics.OpenGL.UniformType.BoolVec3 => UniformType.Vector3Bool,
			OpenTK.Graphics.OpenGL.UniformType.BoolVec4 => UniformType.Vector4Bool,
			OpenTK.Graphics.OpenGL.UniformType.FloatMat2 => UniformType.Matrix2x2,
			OpenTK.Graphics.OpenGL.UniformType.FloatMat3 => UniformType.Matrix3x3,
			OpenTK.Graphics.OpenGL.UniformType.FloatMat4 => UniformType.Matrix4x4,
			OpenTK.Graphics.OpenGL.UniformType.Sampler1d => UniformType.Texture1D,
			OpenTK.Graphics.OpenGL.UniformType.Sampler2d => UniformType.Texture2D,
			OpenTK.Graphics.OpenGL.UniformType.Sampler3d => UniformType.Texture3D,
			OpenTK.Graphics.OpenGL.UniformType.SamplerCube => UniformType.Cubemap,
			OpenTK.Graphics.OpenGL.UniformType.FloatMat2x3 => UniformType.Matrix2x3,
			OpenTK.Graphics.OpenGL.UniformType.FloatMat2x4 => UniformType.Matrix2x4,
			OpenTK.Graphics.OpenGL.UniformType.FloatMat3x2 => UniformType.Matrix3x2,
			OpenTK.Graphics.OpenGL.UniformType.FloatMat3x4 => UniformType.Matrix3x4,
			OpenTK.Graphics.OpenGL.UniformType.FloatMat4x2 => UniformType.Matrix4x2,
			OpenTK.Graphics.OpenGL.UniformType.FloatMat4x3 => UniformType.Matrix4x3,
			OpenTK.Graphics.OpenGL.UniformType.Sampler1dArray => UniformType.Texture1DArray,
			OpenTK.Graphics.OpenGL.UniformType.Sampler2dArray => UniformType.Texture2DArray,
			OpenTK.Graphics.OpenGL.UniformType.SamplerCubeMapArray => UniformType.CubemapArray,
			OpenTK.Graphics.OpenGL.UniformType.UnsignedIntVec2 => UniformType.Vector2Uint,
			OpenTK.Graphics.OpenGL.UniformType.UnsignedIntVec3 => UniformType.Vector3Uint,
			OpenTK.Graphics.OpenGL.UniformType.UnsignedIntVec4 => UniformType.Vector4Uint,
			OpenTK.Graphics.OpenGL.UniformType.DoubleVec2 => UniformType.Vector2Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleVec3 => UniformType.Vector3Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleVec4 => UniformType.Vector4Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleMat2 => UniformType.Matrix2x2Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleMat3 => UniformType.Matrix3x3Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleMat4 => UniformType.Matrix4x4Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleMat2x3 => UniformType.Matrix2x3Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleMat2x4 => UniformType.Matrix2x4Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleMat3x2 => UniformType.Matrix3x2Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleMat3x4 => UniformType.Matrix3x4Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleMat4x2 => UniformType.Matrix4x2Double,
			OpenTK.Graphics.OpenGL.UniformType.DoubleMat4x3 => UniformType.Matrix4x3Double,
			_ => UniformType.Unknown
		};
	}
		
}