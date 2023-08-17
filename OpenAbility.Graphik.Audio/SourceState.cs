namespace OpenAbility.Graphik.Audio;

/// <summary>
/// Represents the state of a source
/// </summary>
public enum SourceState
{
	/// <summary>
	/// The source is uninitialized
	/// </summary>
	Initial,
	/// <summary>
	/// The source is playing
	/// </summary>
	Playing,
	/// <summary>
	/// The source is paused
	/// </summary>
	Paused,
	/// <summary>
	/// The source is stopped
	/// </summary>
	Stopped
}
