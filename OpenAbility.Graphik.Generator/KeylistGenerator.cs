using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenAbility.Graphik.Generator;

public static class KeylistGenerator
{
	public static void GenerateKeys()
	{
		string[] names = Enum.GetNames(typeof(Keys));

		for (int i = 0; i < names.Length; i++)
		{
			names[i] = "\t" + names[i];
		}
		
		Console.WriteLine("public enum Key");
		Console.WriteLine("{");
		Console.WriteLine(String.Join(",\n", names));
		Console.WriteLine("}");
	}
}