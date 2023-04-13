namespace OpenAbility.Graphik.Generator;

public class GeneratorShared
{
	private List<string> usedTypeNames = new List<string>();
	private List<string> usings = new List<string>();
	public string GetTypeText(Type type)
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
		if (type == typeof(bool))
			return "bool";
        
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

	public string InlineUsings(string code)
	{
		return code.Replace("{using}", string.Join("\n", usings));
	}
}
