namespace OpenAbility.Graphik.Generator;

public class GeneratorShared
{
	private List<string> usedTypeNames = new List<string>();
	private List<string> usings = new List<string>();
	public string GetTypeText(Type type)
	{
		if (type == typeof(void))
			return "void";
		if (type == typeof(sbyte))
			return "sbyte";
		if (type == typeof(byte))
			return "byte";
		if (type == typeof(short))
			return "short";
		if (type == typeof(ushort))
			return "ushort";
		if (type == typeof(int))
			return "int";
		if (type == typeof(uint))
			return "uint";
		if (type == typeof(long))
			return "long";
		if (type == typeof(ulong))
			return "ulong";
		if (type == typeof(char))
			return "char";
		if (type == typeof(float))
			return "float";
		if (type == typeof(double))
			return "double";
		if (type == typeof(bool))
			return "bool";
		if (type == typeof(decimal))
			return "decimal";

		if (type.IsArray)
		{
			return GetTypeText(type.GetElementType()!) + "[]";
		}
        
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
