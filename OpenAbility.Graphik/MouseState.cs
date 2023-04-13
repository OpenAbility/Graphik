namespace OpenAbility.Graphik;

/// <summary>
/// The state of the mouse
/// </summary>
public enum MouseState
{
	/// <summary>
	/// A free mouse is visible and can move as it wishes
	/// </summary>
	Free,
	/// <summary>
	/// A hidden mouse is not visible as long as it hovers the window
	/// </summary>
	Hidden,
	/// <summary>
	/// The mouse is hidden and will be automatically centered. Please note that
	/// this uses a virtual cursor position, and is only recommended for usage if you calculate cursor delta
	/// </summary>
	Captured
}
