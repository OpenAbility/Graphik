using OpenAbility.Graphik;
using OpenAbility.Graphik.OpenGL;
using OpenAbility.Graphik.Selection;

[assembly: AssemblyAPI(typeof(GLProvider))]

namespace OpenAbility.Graphik.OpenGL;

internal class GLProvider : IAssemblyAPI
{

	public GraphikAPIProvider Build()
	{
		return new GraphikAPIProvider(GLAPI.Create, GLAPI.Rate, GLAPI.Specifier, 20);
	}
}