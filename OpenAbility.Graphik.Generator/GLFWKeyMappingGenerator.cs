using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenAbility.Graphik.Generator;

public class GLFWKeyMappingGenerator
{
	public static void Generate()
	{
		string[] names = Enum.GetNames(typeof(Key));

		for (int i = 0; i < names.Length; i++)
		{
			names[i] = "\t{Keys." + names[i] + ", Key." + names[i] + "}";
		}
		
		Console.WriteLine("private static readonly Dictionary<Keys, Key> KeyMappings = new Dictionary<Keys, Key>()");
		Console.WriteLine("{");
		Console.WriteLine(String.Join(",\n", names));
		Console.WriteLine("}");
	}
}
