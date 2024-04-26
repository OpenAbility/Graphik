using System.Reflection;

namespace OpenAbility.Graphik.Generator;

public static class GraphikAPILayerGenerator
{
	public static void Generate()
	{

		GeneratorShared generatorShared = new GeneratorShared();

		string output = @"
// Auto-generated IGraphikAPI bindings!
// These should not be modified
{using}
namespace OpenAbility.Graphik;
        
public partial class APILayer : IGraphikAPI
{
";
        
        Type apiInterface = typeof(IGraphikAPI);
        
        var interfaceMethods = apiInterface.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
        
        foreach (var method in interfaceMethods)
        {
        	string line = "\tpublic virtual ";
        
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

	        line += ") => ";

	        if (method.ReturnType != typeof(void))
		        line += "(" + generatorShared.GetTypeText(method.ReturnType) + ")";
	        
	        line += "Intercept(";
        	
        	parameterStrings.Clear();
	        parameterStrings.Add("\"" + method.Name + "\"");
	        parameterStrings.Add("underlying." + method.Name);
        	foreach (var parameter in method.GetParameters())
        	{
        		parameterStrings.Add(parameter.Name ?? "");	
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
