namespace OpenAbility.Graphik.OpenGL;

[AttributeUsage(AttributeTargets.Method)]
internal class LibraryFunctionAttribute : Attribute
{
	public readonly string Name;
	public LibraryFunctionAttribute(string name)
	{
		Name = name;
	}
}
