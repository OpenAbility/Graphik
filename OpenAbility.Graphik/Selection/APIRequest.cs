namespace OpenAbility.Graphik.Selection;

/// <summary>
/// An API request
/// </summary>
public struct APIRequest
{
	public PerformancePriority PerformancePriority;
	public string Platform;
}

public enum PerformancePriority
{
	Performance,
	Stability
}