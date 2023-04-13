namespace OpenAbility.Graphik.OpenGL;

internal class GLException : Exception
{
	private string infoLog;
	public GLException(GLLocation location, string message)
	{
		infoLog = $"At {location.ToString()}: {message}";
	}

	public override string Message { get => infoLog; }
}

internal enum GLLocation
{
	ShaderCompilation,
	General,
	ShaderLinking
} 