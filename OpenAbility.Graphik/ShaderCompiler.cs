using shaderc;

namespace OpenAbility.Graphik;

public static class ShaderCompiler
{

	private static readonly Compiler compiler;
	static ShaderCompiler()
	{
		Options options = new Options();
		options.Optimization = OptimizationLevel.Zero;
		options.NanClamp = true;
		options.TargetSpirVVersion = new SpirVVersion(1, 0);
		options.AutoBindUniforms = false;
		options.AutoMapLocations = true;
		options.SetTargetEnvironment(TargetEnvironment.OpenGL, 0);
		compiler = new Compiler(options);
		
	}

	public static CompiledShader Compile(string shader, string filename, ShaderType type, string entry = "main")
	{
		CompiledShader compiledShader = new CompiledShader();

		compiledShader.Success = true;
		compiledShader.RawSource = shader;
		compiledShader.EntryPoint = entry;
		compiledShader.ShaderType = type;

		return compiledShader;
	}
	
	private static CompiledShader CompileSPIRV(string shader, string filename, ShaderType type, string entry = "main")
	{
		
		ShaderKind kind = type switch
		{
			ShaderType.ComputeShader => ShaderKind.GlslComputeShader,
			ShaderType.FragmentShader => ShaderKind.GlslFragmentShader,
			ShaderType.VertexShader => ShaderKind.GlslVertexShader,
			ShaderType.GeometryShader => ShaderKind.GlslGeometryShader,
			_ => 0
		};

		Result result = compiler.Compile(shader, filename, kind, entry);

		CompiledShader compiledShader = new CompiledShader();
		
		compiledShader.Success = result.Status == Status.Success;
		compiledShader.DataPointer = result.CodePointer;
		compiledShader.DataLength = result.CodeLength;
		compiledShader.WarningCount = result.WarningCount;
		compiledShader.ErrorCount = result.ErrorCount;
		compiledShader.Message = result.ErrorMessage.Trim();
		compiledShader.EntryPoint = entry;
		compiledShader.ShaderType = type;

		return compiledShader;
	}
}


public struct CompiledShader
{
	public bool Success;
	public IntPtr DataPointer;
	public uint DataLength;

	public string RawSource;

	public uint ErrorCount;
	public uint WarningCount;

	public string Message;

	public string EntryPoint;
	public ShaderType ShaderType;
}