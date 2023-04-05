using OpenAbility.Graphik.Generator;


public class Program
{
	public static int Main(string[] args)
	{
		Console.WriteLine("The Graphik Generator");
		Console.WriteLine("Valid generation types:\n\twrapper - The Graphik class\n\tglapi - IGraphikAPI.cs based on the GL implementation");
		if (args.Length == 0)
		{
			Console.Error.WriteLine("Nothing to generate!");
			return 1;
		}

		if (args[0] == "wrapper")
		{
			GraphikWrapperGenerator.Generate();
			return 0;
		} else if (args[0] == "glapi")
		{
			GraphikGLApiGenerator.Generate();
			return 0;
		}
		
		Console.Error.WriteLine("Invalid generation target: " + args[0]);
		return 1;
	}
}

