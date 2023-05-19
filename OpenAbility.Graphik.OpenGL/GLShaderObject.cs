using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAbility.Graphik.OpenGL;

using GLShaderType = OpenTK.Graphics.OpenGL.ShaderType;
public class GLShaderObject : IShaderObject
{
	private string source = "";
	private string filename = "";
	private GLShaderType shaderType;
	public ShaderHandle ShaderHandle;
	public void AttachSource(string source, string filename, ShaderType shaderType)
	{
		
		GLShaderType glType = shaderType switch
		{
			ShaderType.ComputeShader => GLShaderType.ComputeShader,
			ShaderType.FragmentShader => GLShaderType.FragmentShader,
			ShaderType.VertexShader => GLShaderType.VertexShader,
			ShaderType.GeometryShader => GLShaderType.GeometryShader,
			_ => 0
		};

		this.source = source;
		this.filename = filename;
		this.shaderType = glType;

	}
	public ShaderCompilationResult Compile()
	{
		ShaderCompilationResult shaderResult = new ShaderCompilationResult();
		shaderResult.Status = ShaderCompilationStatus.Success;
		
		// Then we create the shader and attach the binary
		ShaderHandle = GL.CreateShader(shaderType);
		GL.ShaderSource(ShaderHandle, source);
		GL.CompileShader(ShaderHandle);

		// Finally we check for errors
		int isCompiled = -1;
		GL.GetShaderi(ShaderHandle, ShaderParameterName.CompileStatus, ref isCompiled);
		if (isCompiled == 0)
		{
				
			int infoLogLength = -1;
			GL.GetShaderi(ShaderHandle, ShaderParameterName.InfoLogLength, ref infoLogLength);
				
			GL.GetShaderInfoLog(ShaderHandle, out string infoLog);
			shaderResult.Log = infoLog + "\n";
				
			shaderResult.Status = ShaderCompilationStatus.Failure;
		}

		return shaderResult;
	}
	public void Dispose()
	{
		GL.DeleteShader(ShaderHandle);
	}
}
