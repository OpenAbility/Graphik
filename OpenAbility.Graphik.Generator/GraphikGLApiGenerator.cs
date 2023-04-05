using System.Reflection;

namespace OpenAbility.Graphik.Generator;

public class GraphikGLApiGenerator
{
	public static void Generate()
	{
		List<string> usedTypeNames = new List<string>();
        List<string> usings = new List<string>();
        
        string GetTypeText(Type type)
        {
        	// TODO: Find all type keywords
        	if (type == typeof(void))
        		return "void";
        	if (type == typeof(int))
        		return "int";
        	if (type == typeof(long))
        		return "long";
        	if (type == typeof(float))
        		return "float";
        	if (type == typeof(string))
        		return "string";
        
        	if (usedTypeNames.Contains(type.Name))
        	{
        		return type.FullName!;
        	}
        
        
        	string useText = "using " + type.Namespace! + ";";
        	if (!usings.Contains(useText))
        		usings.Add(useText);
        	
        	usedTypeNames.Add(type.Name);
        
        	return type.Name;
        
        }
        
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
        	string line = "\tpublic static ";
        
        	line += GetTypeText(method.ReturnType) + " ";
        	line += method.Name + "(";
        
        	List<string> parameterStrings = new List<string>();
        	foreach (var parameter in method.GetParameters())
        	{
        		string parameterString = "";
        
        		parameterString += GetTypeText(parameter.ParameterType) + " ";
        		parameterString += parameter.Name;
        
        		parameterStrings.Add(parameterString);
        	}
        	line += string.Join(", ", parameterStrings);
        
        	line += ");\n";
            output += line;
        }
        
        output += "}";
        output = output.Replace("{using}", string.Join("\n", usings));
        
        Console.WriteLine(output);
	}
}
