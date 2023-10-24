namespace OpenAbility.Graphik;

public static class ShaderCompiler
{
	public static CompiledShader Compile(string shader, string filename, ShaderType type, string entry = "main") =>
		Graphik.GetAPI().GetCompiler().Compile(shader, filename, type, entry);
}


public class CompiledShader
{
	public readonly object Data;
	public readonly bool Success;
	public readonly string Message;
	private readonly Action<CompiledShader> onFree;
	
	public CompiledShader(bool success, string message, object data, Action<CompiledShader> onFree)
	{
		Success = success;
		Message = message;
		Data = data;
		this.onFree = onFree;
	}

	~CompiledShader()
	{
		onFree(this);
	}
}