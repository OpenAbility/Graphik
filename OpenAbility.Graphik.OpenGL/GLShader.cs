using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GLShaderType = OpenTK.Graphics.OpenGL.ShaderType;

namespace OpenAbility.Graphik.OpenGL;

public class GLShader : IShader
{
	private ProgramHandle handle;
	private List<ShaderHandle> shaders = new List<ShaderHandle>();
	private List<ShaderSource> shaderSources = new List<ShaderSource>();
	public GLShader()
	{
		handle = GL.CreateProgram();
	}
	
	public void AttachSource(string source, ShaderType shaderType)
	{

		GLShaderType type = shaderType switch
		{
			ShaderType.ComputeShader => GLShaderType.ComputeShader,
			ShaderType.FragmentShader => GLShaderType.FragmentShader,
			ShaderType.VertexShader => GLShaderType.VertexShader,
			ShaderType.GeometryShader => GLShaderType.GeometryShader,
			_ => 0
		};
		
		shaderSources.Add(new ShaderSource()
		{
			Source = source,
			ShaderType = type
		});
		
		
	}
	
	public string Compile()
	{
		foreach (var source in shaderSources)
		{
			ShaderHandle shaderHandle = GL.CreateShader(source.ShaderType);
			GL.ShaderSource(shaderHandle, source.Source);
			GL.CompileShader(shaderHandle);
			GL.GetShaderInfoLog(shaderHandle, out string info);
			if (!String.IsNullOrEmpty(info))
			{
				return info;
			}
			shaders.Add(shaderHandle);
		}
		return String.Empty;
	}
	
	public string Link()
	{
		foreach (var shader in shaders)
		{
			GL.AttachShader(handle, shader);
		}
		GL.LinkProgram(handle);
		GL.GetProgramInfoLog(handle, out string log);
		foreach (var shader in shaders)
		{
			GL.DetachShader(handle, shader);
			GL.DeleteShader(shader);
		}

		if (!String.IsNullOrEmpty(log))
			return log;
		return String.Empty;
	}

	public void Use()
	{
		GL.UseProgram(handle);
	}
	
	private struct ShaderSource
	{
		public string Source;
		public GLShaderType ShaderType;
	}
}
