namespace OpenAbility.Graphik.Selection;

/// <summary>
/// An API request
/// </summary>
public struct APIRequest
{
	/// <summary>
	/// What's the priority? Performance or stability?
	/// </summary>
	public PerformancePriority PerformancePriority;
	/// <summary>
	/// What's the target platform?
	/// </summary>
	public TargetPlatform Platform;
	/// <summary>
	/// The target platform, if <see cref="Platform"/> is set to <see cref="TargetPlatform.Other"/>
	/// </summary>
	public string PlatformOther;
}

/// <summary>
/// Common target platforms
/// </summary>
public enum TargetPlatform
{
	/// <summary>
	/// Generic Desktop
	/// </summary>
	Desktop,
	/// <summary>
	/// The Windows OS(I'd say 7+)
	/// </summary>
	Windows,
	/// <summary>
	/// Linux
	/// </summary>
	Linux,
	/// <summary>
	/// MacOS(i.e, Metal backends preferred)
	/// </summary>
	Mac,
	/// <summary>
	/// IOS backends(iPad/iPhone)
	/// </summary>
	Ios,
	/// <summary>
	/// Android backends
	/// </summary>
	Android,
	/// <summary>
	/// Playstation 3
	/// </summary>
	Ps3,
	/// <summary>
	/// Playstation 4
	/// </summary>
	Ps4,
	/// <summary>
	/// Playstation 5
	/// </summary>
	Ps5,
	/// <summary>
	/// Xbox One
	/// </summary>
	XboxOne,
	/// <summary>
	/// Xbox Series X
	/// </summary>
	XboxX,
	/// <summary>
	/// Xbox Series S
	/// </summary>
	XboxS,
	/// <summary>
	/// Nintendo Switch
	/// </summary>
	Switch,
	/// <summary>
	/// Other platform(specify in <see cref="APIRequest.PlatformOther"/>)
	/// </summary>
	Other
}

/// <summary>
/// What's more important to you
/// </summary>
public enum PerformancePriority
{
	/// <summary>
	/// We want stuff to be fast, and are willing to sacrifice stability
	/// </summary>
	Performance,
	/// <summary>
	/// We want stuff to be stable.
	/// </summary>
	Stability
}