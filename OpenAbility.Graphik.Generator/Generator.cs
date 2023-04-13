namespace OpenAbility.Graphik.Generator;

public struct Generator
{
	public readonly Action Callback;
	public readonly string Name;
	public readonly string Description;
	
	public Generator(Action callback, string name, string description)
	{
		Callback = callback;
		Name = name;
		Description = description;
	}
}