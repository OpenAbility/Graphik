using Silk.NET.Shaderc;

namespace OpenAbility.Graphik.OpenGL;

public unsafe class SPIRVResult
{
	public readonly CompilationResult* Result;
	public readonly ShaderType ShaderType;
	public readonly IntPtr Binary;
	public readonly uint BinaryLength;
	public readonly string Entry;
	

	public SPIRVResult(CompilationResult* result, Shaderc shaderc, ShaderType shaderType, string entry)
	{
		Result = result;
		ShaderType = shaderType;
		Binary = new IntPtr(shaderc.ResultGetBytes(result));
		BinaryLength = (uint)shaderc.ResultGetLength(result);
		Entry = entry;
	}
}
