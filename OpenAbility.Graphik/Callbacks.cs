namespace OpenAbility.Graphik;

public delegate void ResizeCallback(int width, int height);
public delegate void KeyCallback(Key key, InputAction action);
public delegate void TypeCallback(char character);
public delegate void MouseCallback(MouseButton mouseButton, InputAction action);
public delegate void CursorCallback(float cursorX, float cursorY);