namespace OpenAbility.Graphik;

public interface IShaderCompiler
{
	public CompiledShader Compile(string language, string shader, string filename, ShaderType type, string entry);
}
