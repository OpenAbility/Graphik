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

	public ShaderBuildResult Build(CompiledShader compiledShader)
	{

		GLSLResult compilationResult = (GLSLResult)compiledShader.Data;
		
		ShaderBuildResult shaderBuildResult = new ShaderBuildResult();
		shaderBuildResult.Status = ShaderCompilationStatus.Success;
		
		ShaderHandle = GL.CreateShader(compilationResult.ShaderType switch
		{
			ShaderType.ComputeShader => GLShaderType.ComputeShader,
			ShaderType.FragmentShader => GLShaderType.FragmentShader,
			ShaderType.VertexShader => GLShaderType.VertexShader,
			ShaderType.GeometryShader => GLShaderType.GeometryShader,
			_ => 0
		});
		
		GL.ShaderSource(ShaderHandle, compilationResult.GLSL);
		GL.CompileShader(ShaderHandle);
		
		GL.GetShaderInfoLog(ShaderHandle, out string infoLog);
		if (!String.IsNullOrEmpty(infoLog))
		{
			shaderBuildResult.Log = infoLog;
			shaderBuildResult.Status = ShaderCompilationStatus.Failure;
		}
		return shaderBuildResult;
	}
	
	public void Dispose()
	{
		GL.DeleteShader(ShaderHandle);
	}
}
