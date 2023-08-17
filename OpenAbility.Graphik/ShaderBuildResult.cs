namespace OpenAbility.Graphik;

public struct ShaderBuildResult
{
	public ShaderCompilationStatus Status;
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