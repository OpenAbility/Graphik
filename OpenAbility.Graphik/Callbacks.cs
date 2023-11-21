namespace OpenAbility.Graphik;

public delegate void ResizeCallback(int width, int height);
public delegate void KeyCallback(Key key, InputAction action);
public delegate void TypeCallback(uint character);
public delegate void MouseCallback(MouseButton mouseButton, InputAction action);
public delegate void CursorCallback(float cursorX, float cursorY);
public delegate void ScrollCallback(float cursorX, float cursorY);
public delegate void ErrorCallback(string errorID, string errorMessage);
public delegate void DebugCallback(string debugID, string message);
public delegate Include IncludeCallback(string requested, string requester);

public struct Include
{
	public string Code;
	public string ResultingPath;

	public Include(string code, string resultingPath)
	{
		Code = code;
		ResultingPath = resultingPath;
	}
}