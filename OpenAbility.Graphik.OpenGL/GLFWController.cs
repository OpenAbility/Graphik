using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenAbility.Graphik.OpenGL;

// NOTE: This entire system *will* have to be ported for whenever we decide to, you know, not use GLFW.
public unsafe class GLFWController : IController
{
	private readonly int id;
	private GamepadState gamepadState;

	private float[] joystickAxes = Array.Empty<float>();
	private JoystickInputAction[] inputActions = Array.Empty<JoystickInputAction>();
	
	public bool Plugged
	{
		get
		{
			return GLFW.JoystickPresent(id);
		}
	}
	
	public bool Gamepad
	{
		get
		{
			return GLFW.JoystickIsGamepad(id);
		}
	}
	
	public string Name
	{
		get
		{
			return GLFW.GetJoystickName(id);
		}
	}
	
	public string Mapping
	{
		get
		{
			return GLFW.GetGamepadName(id);
		}
	}
	
	public GLFWController(int id)
	{
		this.id = id;
	}

	public float GetAxis(ControllerAxis axis)
	{
		
		if (!Plugged)
			return 0;
		
		if (Gamepad)
		{
			return gamepadState.Axes[(int)axis];
		}

		if ((int)axis >= joystickAxes.Length)
			return 0;
		
		return joystickAxes[(int)axis];
	}
	
	public bool GetButton(ControllerButton button)
	{
		
		if (!Plugged)
			return false;
		
		if (Gamepad)
		{
			return gamepadState.Buttons[(int)button] > 0;
		}

		if ((int)button >= inputActions.Length)
			return false;
		
		return inputActions[(int)button] == JoystickInputAction.Press;
	}

	public void UpdateState()
	{
		if (!GLFW.GetGamepadState(id, out gamepadState)) {
			joystickAxes = GLFW.GetJoystickAxes(id).ToArray();
			inputActions = GLFW.GetJoystickButtons(id);
		}
	}
}