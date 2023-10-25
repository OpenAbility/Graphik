using System.Text.Json;

namespace OpenAbility.Graphik;

public static class ShaderCompiler
{

	public static CompiledShader Compile(string shader, string filename, ShaderType type, string entry = "main")
	{
		if (Graphik.GetAPI().IsExtensionSupported(Path.GetExtension(filename)))
		{
			return Graphik.GetAPI().GetCompiler().Compile(Path.GetExtension(filename)[1..], shader, filename, type, entry);
		}

		Dictionary<string, string> sources = JsonSerializer.Deserialize<Dictionary<string, string>>(shader) ?? new Dictionary<string, string>();

		string[] supported = Graphik.GetAPI().GetSupportedLanguages();
		string? sourceLanguage = supported.FirstOrDefault(s => sources.ContainsKey(s));

		if (sourceLanguage == null)
			throw new NotSupportedException(filename + " does not support " + sourceLanguage);
		
		string sourceFile = Graphik.FileReadHandler(sources[sourceLanguage], Path.GetDirectoryName(filename));
		
		return Graphik.GetAPI().GetCompiler().Compile(sourceLanguage, sourceFile, filename, type, entry);
	}
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

	public override string ToString()
	{
		return $"CompiledShader, Success: {Success}, Data: ''{Data}'', Message: ''{Message}''";
	}

	~CompiledShader()
	{
		onFree(this);
	}
}