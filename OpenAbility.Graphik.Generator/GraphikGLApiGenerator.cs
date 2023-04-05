using System.Reflection;

namespace OpenAbility.Graphik.Generator;

public class GraphikGLApiGenerator
{
	public static void Generate()
	{
		GeneratorShared generatorShared = new GeneratorShared();

        string output = @"
// Auto-generated from GLAPI from OpenAbility.Graphik.OpenGL
// These should not be modified
namespace OpenAbility.Graphik;
{using}
public interface IGraphikAPI
{
";
        
        Type apiInterface = typeof(IGraphikAPI);
        
        var interfaceMethods = apiInterface.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
        
        foreach (var method in interfaceMethods)
        {
        	string line = "\t";
        
        	line += generatorShared.GetTypeText(method.ReturnType) + " ";
        	line += method.Name + "(";
        
        	List<string> parameterStrings = new List<string>();
        	foreach (var parameter in method.GetParameters())
        	{
        		string parameterString = "";
        
        		parameterString += generatorShared.GetTypeText(parameter.ParameterType) + " ";
        		parameterString += parameter.Name;
        
        		parameterStrings.Add(parameterString);
        	}
        	line += string.Join(", ", parameterStrings);
        
        	line += ");\n";
            output += line;
        }
        
        output += "}";
        output = generatorShared.InlineUsings(output);
        
        Console.WriteLine(output);
	}
}
