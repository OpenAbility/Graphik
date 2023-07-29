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
		
		ShaderBuildResult shaderBuildResult = new ShaderBuildResult();
		
		ShaderHandle = GL.CreateShader(compiledShader.ShaderType switch
		{
			ShaderType.ComputeShader => GLShaderType.ComputeShader,
			ShaderType.FragmentShader => GLShaderType.FragmentShader,
			ShaderType.VertexShader => GLShaderType.VertexShader,
			ShaderType.GeometryShader => GLShaderType.GeometryShader,
			_ => 0
		});
		
		GL.ShaderSource(ShaderHandle, compiledShader.RawSource);
		GL.CompileShader(ShaderHandle);
		
		int isCompiled = -1;
		GL.GetShaderi(ShaderHandle, ShaderParameterName.CompileStatus, ref isCompiled);
		if (isCompiled == 0)
		{
				
			int infoLogLength = -1;
			GL.GetShaderi(ShaderHandle, ShaderParameterName.InfoLogLength, ref infoLogLength);
				
			GL.GetShaderInfoLog(ShaderHandle, out string infoLog);
			shaderBuildResult.Log = infoLog;
			shaderBuildResult.Status = ShaderCompilationStatus.Failure;
		}
		shaderBuildResult.Status = ShaderCompilationStatus.Success;
		return shaderBuildResult;
	}
	
	private ShaderBuildResult BuildSPIRV(CompiledShader compiledShader)
	{

		ShaderBuildResult shaderBuildResult = new ShaderBuildResult();
		
		if (compiledShader.Success == false)
			throw new ArgumentException("Cannot build shader object from non-compiled shader!", nameof(compiledShader));
		ShaderHandle = GL.CreateShader(compiledShader.ShaderType switch
		{
			ShaderType.ComputeShader => GLShaderType.ComputeShader,
			ShaderType.FragmentShader => GLShaderType.FragmentShader,
			ShaderType.VertexShader => GLShaderType.VertexShader,
			ShaderType.GeometryShader => GLShaderType.GeometryShader,
			_ => 0
		});
		
		GL.ShaderBinary(1, ShaderHandle, ShaderBinaryFormat.ShaderBinaryFormatSpirV, compiledShader.DataPointer, (int)compiledShader.DataLength);
		GL.SpecializeShader(ShaderHandle, compiledShader.EntryPoint, 0, Array.Empty<uint>(), Array.Empty<uint>());
		
		int isCompiled = -1;
		GL.GetShaderi(ShaderHandle, ShaderParameterName.CompileStatus, ref isCompiled);
		if (isCompiled == 0)
		{
				
			int infoLogLength = -1;
			GL.GetShaderi(ShaderHandle, ShaderParameterName.InfoLogLength, ref infoLogLength);
				
			GL.GetShaderInfoLog(ShaderHandle, out string infoLog);
			shaderBuildResult.Log = infoLog;
			shaderBuildResult.Status = ShaderCompilationStatus.Failure;
		}
		shaderBuildResult.Status = ShaderCompilationStatus.Success;
		return shaderBuildResult;

	}
	public void Dispose()
	{
		GL.DeleteShader(ShaderHandle);
	}
}
