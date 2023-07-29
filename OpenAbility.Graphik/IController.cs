namespace OpenAbility.Graphik;

public interface IController
{
	/// <summary>
	/// If the controller is plugged in/accessible to the program
	/// </summary>
	public bool Plugged { get; }
	/// <summary>
	/// If the controller is a gamepad(required for GLFW), and thus has gamepad mappings
	/// </summary>
	public bool Gamepad { get; }
	/// <summary>
	/// The name of the controller
	/// </summary>
	public string Name { get; }
	/// <summary>
	/// The controller mapping name
	/// </summary>
	public string Mapping { get; }
	
	/// <summary>
	/// Get a controller axis
	/// </summary>
	/// <param name="axis">The axis to get</param>
	/// <returns>The axis value between -1 and 1</returns>
	public float GetAxis(ControllerAxis axis);
	/// <summary>
	/// Get if a button is down on the controller
	/// </summary>
	/// <param name="button">The button to check</param>
	/// <returns>The current press-down-ness of the button</returns>
	public bool GetButton(ControllerButton button);
	/// <summary>
	/// Update the current controller state, in case it is needed
	/// </summary>
	public void UpdateState();
}


public enum ControllerAxis
{
	LeftX = 0,
	LeftY = 1,
	RightX = 2,
	RightY = 3,
	L2 = 4,
	R2 = 5,
	Last = R2
}

public enum ControllerButton
{
	A = 0,
	B = 1,
	X = 2,
	Y = 3,
	L1 = 4,
	R1 = 5,
	Back = 6,
	Start = 7,
	Guide = 8,
	L3 = 9,
	R3 = 10,
	Up = 11,
	Right = 12,
	Down = 13,
	Left = 14,
	
	Cross = A,
	Circle = B,
	Square = X,
	Triangle = Y,
	
	Last = Left
}
