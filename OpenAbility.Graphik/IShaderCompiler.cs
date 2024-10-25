namespace OpenAbility.Graphik;

public interface IShaderCompiler
{
	public CompiledShader Compile(string language, string shader, string filename, ShaderType type, string entry, Dictionary<string, string>? defines = null);
	public CompiledShader? LoadCache(byte[] cached);
}
