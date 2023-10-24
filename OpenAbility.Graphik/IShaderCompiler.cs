namespace OpenAbility.Graphik;

public interface IShaderCompiler
{
	public CompiledShader Compile(string shader, string filename, ShaderType type, string entry);
}
