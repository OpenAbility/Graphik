namespace OpenAbility.Graphik.Generator;

public class GLBlendFactorGenerator
{
	public static void Generate()
	{
		string[] names = Enum.GetNames(typeof(BlendFactor));

		foreach (string t in names)
		{
			Console.WriteLine("\tBlendFactor." + t + " => BlendingFactor." + t + ",");
		}

	}
}
