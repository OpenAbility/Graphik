using OpenAbility.Graphik.Generator;


public class Program
{

	private static readonly Generator[] Generators = new Generator[]
	{
		new Generator(GraphikWrapperGenerator.Generate, "wrapper", "Generate the Graphik class"),
		new Generator(GraphikGLApiGenerator.Generate, "glapi", "GraphikAPI based on the GL implementation"),
		new Generator(GraphikGLApiGenerator.Generate, "keylist", "Generate the Key enum based off the OpenTK keys enum"),
		new Generator(GraphikGLApiGenerator.Generate, "glfwKeymapping", "Generate a mapping for GLFW->Graphik keys"),
		new Generator(GraphikGLApiGenerator.Generate, "glBlendFactor", "Generate a mapping for Graphik blend factor->OpenGL blend factor"),
	};
	
	
	public static int Main(string[] args)
	{
		Console.WriteLine("The Graphik Generator");
		
		Console.WriteLine("Valid generation types:");
		foreach (var generator in Generators)
		{
			Console.WriteLine($"\t{generator.Name} - {generator.Description}");	
		}
		
		
		if (args.Length == 0)
		{
			Console.Error.WriteLine("Nothing to generate!");
			return 1;
		}

		foreach (var generator in Generators)
		{
			if (generator.Name == args[0].Trim())
			{
				generator.Callback();
				return 0;
			}
		}
		
		Console.Error.WriteLine("Invalid generation target: " + args[0]);
		return 1;
	}
}

