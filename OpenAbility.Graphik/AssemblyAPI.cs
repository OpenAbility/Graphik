using OpenAbility.Graphik.Selection;

namespace OpenAbility.Graphik;

[AttributeUsage(AttributeTargets.Assembly)]
public class AssemblyAPIAttribute : Attribute
{
	public Type API;

	public AssemblyAPIAttribute(Type type)
	{
		API = type;
	}
}

public interface IAssemblyAPI
{
	public GraphikAPIProvider Build();
}