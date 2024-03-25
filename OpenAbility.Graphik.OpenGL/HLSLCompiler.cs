using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Shaderc;
using Silk.NET.SPIRV;
using Silk.NET.SPIRV.Cross;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Compiler = Silk.NET.Shaderc.Compiler;
using SourceLanguage = Silk.NET.Shaderc.SourceLanguage;

namespace OpenAbility.Graphik.OpenGL;

internal unsafe class HLSLCompiler : IShaderCompiler
{
	private readonly Compiler* compiler;
	private readonly CompileOptions* options;
	private readonly Shaderc shaderc;

	private readonly Cross spirvCross;
	private readonly Context* spirvContext;
	internal static IncludeCallback IncludeResolve = null!;
	internal static IncludeReleaser Releaser = null!;

	private static readonly PfnIncludeResolveFn IncludeResolveFn =
		new ((arg0, b, i, arg3, arg4) => 
			IncludeResolve(arg0, b, i, arg3, arg4));
	private static readonly PfnIncludeResultReleaseFn ResultReleaseFn =
		new ((arg0, result) => Releaser(arg0, result));
	
	private readonly Silk.NET.SPIRV.Cross.ErrorCallback errorCallback = (_, b) =>
	{
		if (CallbackHandler.ErrorCallback != null)
			CallbackHandler.ErrorCallback("SPIRV ERROR", SilkMarshal.PtrToString(new IntPtr(b)) ?? "");
	};

	public HLSLCompiler()
	{
		shaderc = Shaderc.GetApi();
		compiler = shaderc.CompilerInitialize();
		options = shaderc.CompileOptionsInitialize();
		
		
		shaderc.CompileOptionsSetSourceLanguage(options, SourceLanguage.Hlsl);
		shaderc.CompileOptionsSetTargetEnv(options, TargetEnv.Opengl, (int)EnvVersion.Opengl45);
		shaderc.CompileOptionsSetPreserveBindings(options, new Bool32(true));
		shaderc.CompileOptionsSetOptimizationLevel(options, OptimizationLevel.Performance);
		shaderc.CompileOptionsSetGenerateDebugInfo(options);
		shaderc.CompileOptionsSetAutoMapLocations(options, new Bool32(true));
		shaderc.CompileOptionsSetHlslFunctionality1(options, new Bool32(true));
		shaderc.CompileOptionsSetAutoCombinedImageSampler(options, new Bool32(true));
		shaderc.CompileOptionsSetHlslIoMapping(options, new Bool32(true));
		shaderc.CompileOptionsSetIncludeCallbacks(options, IncludeResolveFn,
			ResultReleaseFn, null);
		
		AddMacro("BACKEND", "OPENGL");
		AddMacro("OPENGL", "true");

		spirvCross = Cross.GetApi();
		spirvCross.ContextCreate(ref spirvContext);
		spirvCross.ContextSetErrorCallback(spirvContext, new PfnErrorCallback(errorCallback), null);
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

	public CompiledShader Compile(string language, string shader, string filename, ShaderType type, string entry)
	{
		if (language == "hlsl")
			return CompileHLSL(shader, filename, type, entry);
		
		
		GLSLResult glslResult = new GLSLResult(shader, type);
		CompiledShader compiledShader = new CompiledShader(true, 
			"", glslResult, delegate {  });

		return compiledShader;
	}
	
	
	private static byte[] GetHash(string inputString)
	{
		using (HashAlgorithm algorithm = SHA256.Create())
			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
	}
	
	private static string GetHashString(string inputString)
	{
		StringBuilder sb = new StringBuilder();
		foreach (byte b in GetHash(inputString))
			sb.Append(b.ToString("X2"));

		return sb.ToString();
	}
	

	public CompiledShader CompileHLSL(string shader, string filename, ShaderType type, string entry)
	{
		
		CallbackHandler.DebugCallback?
			.Invoke("GRAPHIK WARNING", 
				"HLSL is not fully supported(erroring file: " + filename + ")");
		
		ShaderKind shaderKind = type switch
		{
			ShaderType.CompleteShader => 
				throw new NotSupportedException("Complete shaders are not GL compatible"),
			ShaderType.ComputeShader => ShaderKind.ComputeShader,
			ShaderType.FragmentShader => ShaderKind.FragmentShader,
			ShaderType.GeometryShader => ShaderKind.GeometryShader,
			ShaderType.VertexShader => ShaderKind.VertexShader,
			_ => 0
		};

		CompilationResult* result = shaderc.CompileIntoSpv(compiler, (byte*)SilkMarshal.StringToPtr(shader),
			new UIntPtr((uint)shader.Length), shaderKind, 
			(byte*)SilkMarshal.StringToPtr(filename), 
			(byte*)SilkMarshal.StringToPtr(entry), options);

		string glsl = "";
		bool success = shaderc.ResultGetCompilationStatus(result) == CompilationStatus.Success;
		string message = SilkMarshal.PtrToString(new IntPtr(shaderc.ResultGetErrorMessage(result))) ?? "";
		
		if (success)
		{
			ParsedIr* parsedIr;
			Silk.NET.SPIRV.Cross.Compiler* spirvCompiler;

			CompilerOptions* compilerOptions;
			
			
			uint words = (uint)shaderc.ResultGetLength(result) / sizeof(uint);
			uint* spirV = (uint*)shaderc.ResultGetBytes(result);
			string[] strings = new string[1];
		
			Result spirvResult = spirvCross.ContextParseSpirv(spirvContext, spirV, 
				new UIntPtr(words), &parsedIr);

			if (spirvResult != Result.Success)
			{
				message += "\nSPIRV-CROSS Parse Error: " + spirvResult;
				success = false;
				goto spirv_done;
			}
			
			Result createResult = spirvCross.ContextCreateCompiler(spirvContext, Backend.Glsl,
				parsedIr, CaptureMode.Copy, &spirvCompiler);

			if (createResult != Result.Success)
			{
				message += "\nSPIRV-CROSS Compiler Creation Error: " + createResult;
				success = false;
				goto spirv_done;
			}

			Result optionsCreateResult = 
				spirvCross.CompilerCreateCompilerOptions(spirvCompiler, &compilerOptions);
			if (optionsCreateResult != Result.Success)
			{
				message += "\nSPIRV-CROSS Options Creation Error: " + createResult;
				success = false;
				goto spirv_done;
			}
			spirvCross.CompilerOptionsSetBool(compilerOptions, 
				CompilerOption.GlslEnable420PackExtension, 1);
			spirvCross.CompilerOptionsSetUint(compilerOptions, CompilerOption.GlslVersion, 450);
			spirvCross.CompilerOptionsSetBool(compilerOptions, CompilerOption.EmitLineDirectives, 1);
			spirvCross.CompilerOptionsSetBool(compilerOptions, 
				CompilerOption.GlslEmitUniformBufferAsPlainUniforms, 1);
			spirvCross.CompilerOptionsSetBool(compilerOptions, CompilerOption.GlslVulkanSemantics, 0);

			ExecutionModel executionModel = type switch
			{
				ShaderType.ComputeShader => ExecutionModel.Kernel,
				ShaderType.FragmentShader => ExecutionModel.Fragment,
				ShaderType.GeometryShader => ExecutionModel.Geometry,
				ShaderType.VertexShader => ExecutionModel.Vertex,
				_ => 0
			};
            
			Result entryPointResult = spirvCross.CompilerSetEntryPoint(spirvCompiler, entry, executionModel);
			
			if (entryPointResult != Result.Success)
			{
				message += "\nSPIRV-CROSS Entry Point Error: " + entryPointResult;
				success = false;
			}

			spirvCross.CompilerBuildCombinedImageSamplers(spirvCompiler);

			CombinedImageSampler* combinedImageSamplers;
			UIntPtr combinedImageSamplerCount;

			spirvCross.CompilerGetCombinedImageSamplers(spirvCompiler, 
				&combinedImageSamplers, &combinedImageSamplerCount);

			for (int i = 0; i < (uint)combinedImageSamplerCount; i++)
			{
				CombinedImageSampler sampler = combinedImageSamplers[i];
				
				spirvCross.CompilerSetName(spirvCompiler, sampler.CombinedId, 
					spirvCross.CompilerGetName(spirvCompiler, sampler.SamplerId));
			}

			Result compileResult = spirvCross.CompilerCompile(spirvCompiler, strings);
			
			if (compileResult != Result.Success)
			{
				message += "\nSPIRV-CROSS Compile Error: " + compileResult;
				success = false;
			}

			glsl = strings[0];
			
			// Move everything OUT of _Global
			Regex globalRegex = new Regex(@"layout\(binding = 0, std140\) uniform _Global\n{\n((?:.|\n|)*)\n} (_\d*);");
			if (globalRegex.IsMatch(glsl))
			{
				Match match = globalRegex.Match(glsl);

				string[] declarations = match.Groups[1].Value.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
				string replacement = "";
				foreach (var declaration in declarations)
				{
					replacement += "uniform " + declaration.Replace("row_major", "").Replace("column_major", "").Replace("layout()", "") + "\n";
				}
				
				glsl = glsl.Replace(match.Value, replacement);
				glsl = glsl.Replace(match.Groups[2].Value + ".", "");
			}
			
			
			// Uniform redeclaration
			Regex uniformDeclarationRegex = new Regex(@"layout\(.*\) uniform (.*)\n{\n(.*)\n} (_\d*);", RegexOptions.Multiline);

			while (uniformDeclarationRegex.IsMatch(glsl))
			{
				Match match = uniformDeclarationRegex.Match(glsl);
				string typeHash = GetHashString(match.Groups[1].Value);
				string structName = "_struct_" + typeHash;
				string replacement = "struct " + structName + "\n{\n" + match.Groups[2] + 
				                     "\n};\n\nuniform _struct_" + typeHash + " " + match.Groups[1] + 
				                     ";" + "\n#define " + match.Groups[3] + " " + match.Groups[1];

				glsl = glsl.Replace(match.Value, replacement);
				
			}
			
			/*
			Regex ssboDeclarationRegex = new Regex(@"layout\((.*)\) buffer (.*)\n{\n(.*)\n} (.*);", RegexOptions.Multiline);

			while (ssboDeclarationRegex.IsMatch(glsl))
			{
				Match match = ssboDeclarationRegex.Match(glsl);
				
				string replacement = "layout(" + match.Groups[1] + ") buffer " +
				                     match.Groups[2] + "\n{\n" + match.Groups[3] + "\n};";
				
				glsl = glsl.Replace(match.Value, replacement);
				glsl = glsl.Replace(match.Groups[4].Value,  match.Groups[2].Value);
			}
			*/
		}
		spirv_done:
		
		
		return new CompiledShader( success, message, new GLSLResult(glsl, type), _ => { });

	}
}

public unsafe delegate void IncludeReleaser(void* userData, IncludeResult* includeResult);
public unsafe delegate IncludeResult* IncludeCallback(void* userData, byte* requestedSource, 
	int type, byte* requestingSource, UIntPtr includeDepth);