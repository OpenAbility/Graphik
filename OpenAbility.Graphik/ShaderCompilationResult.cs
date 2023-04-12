namespace OpenAbility.Graphik;

public struct ShaderCompilationResult
{
	public ShaderCompilationStatus Status;
	public int WarningCount;
	public int ErrorCount;
	public string Log;
}

/// <summary>
/// The status of the compilation.
/// </summary>
public enum ShaderCompilationStatus
{
	Success = 0,
	Failure = 2
}