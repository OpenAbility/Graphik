using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Shaderc;

namespace OpenAbility.Graphik.OpenGL;

internal unsafe class HLSLCompiler : IShaderCompiler
{
	private Compiler* compiler;
	private CompileOptions* options;
	private Shaderc shaderc;

	public HLSLCompiler()
	{
		shaderc = Shaderc.GetApi();
		compiler = shaderc.CompilerInitialize();
		options = shaderc.CompileOptionsInitialize();
		
		
		shaderc.CompileOptionsSetSourceLanguage(options, SourceLanguage.Hlsl);
		shaderc.CompileOptionsSetTargetEnv(options, TargetEnv.Opengl, (int)EnvVersion.Opengl45);
		shaderc.CompileOptionsSetPreserveBindings(options, new Bool32(true));
		shaderc.CompileOptionsSetOptimizationLevel(options, OptimizationLevel.Zero);
		shaderc.CompileOptionsSetGenerateDebugInfo(options);
		
		AddMacro("BACKEND", "OPENGL");
		AddMacro("OPENGL", "true");
	}

	private void AddMacro(string macro, string value)
	{
		shaderc.CompileOptionsAddMacroDefinition(options, macro, (UIntPtr)macro.Length, value, 
			(UIntPtr)value.Length);
	}

	~HLSLCompiler()
	{
		shaderc.CompileOptionsRelease(options);
		shaderc.CompilerRelease(compiler);
		shaderc.Dispose();
	}
	

	public CompiledShader Compile(string shader, string filename, ShaderType type, string entry)
	{

		ShaderKind shaderKind = type switch
		{
			ShaderType.CompleteShader => throw new NotSupportedException("Complete shaders are not GL compaible"),
			ShaderType.ComputeShader => ShaderKind.ComputeShader,
			ShaderType.FragmentShader => ShaderKind.FragmentShader,
			ShaderType.GeometryShader => ShaderKind.GeometryShader,
			ShaderType.VertexShader => ShaderKind.VertexShader,
			_ => 0
		};

		CompilationResult* result = shaderc.CompileIntoSpv(compiler, (byte*)SilkMarshal.StringToPtr(shader),
			new UIntPtr((uint)shader.Length), shaderKind, (byte*)SilkMarshal.StringToPtr(filename), (byte*)SilkMarshal.StringToPtr(entry), options);
		
		return new CompiledShader( shaderc.ResultGetCompilationStatus(result) == CompilationStatus.Success,
			SilkMarshal.PtrToString(new IntPtr(shaderc.ResultGetErrorMessage(result))) ?? "", 
			new SPIRVResult(result, shaderc, type, entry), (s) =>
		{
			shaderc.ResultRelease(((SPIRVResult)s.Data).Result);

		});

	}
}
