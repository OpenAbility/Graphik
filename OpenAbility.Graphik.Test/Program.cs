using OpenAbility.Graphik;
using OpenAbility.Graphik.OpenGL;
using StbImageSharp;

Graphik.SetAPI(new GLAPI());
Graphik.InitializeSystems();

Graphik.SetErrorCallback((id, message) =>
{
	Console.Error.WriteLine(id + ": " + message);
});

Graphik.SetDebugCallback((id, message) =>
{
	Console.WriteLine(id + ": " + message);
});

Graphik.InitializeWindow("Hello, World Graphik!", 1280, 720);

Graphik.SetFeature(Feature.Culling, false);

float[] vertices =
{
	// x, y, z, u, v
	-0.5f, -0.5f, 0.0f, 0, 0,
	-0.5f,  0.5f, 0.0f, 0, 1,
	 0.5f, -0.5f, 1.0f, 1, 0,
	 0.5f,  0.5f, 1.0f, 1, 1,
};

IMesh mesh = Graphik.CreateMesh();
mesh.PrepareModifications();
mesh.SetVertexData(vertices);
mesh.SetVertexAttrib(0, 3, VertexAttribType.Float, 5 * sizeof(float), 0);
mesh.SetVertexAttrib(1, 2, VertexAttribType.Float, 5 * sizeof(float), 3 * sizeof(float));
mesh.SetIndices(new uint[] {0, 1, 2, 1, 3, 2});

IShader shader = Graphik.CreateShader();

IShaderObject vertex = Graphik.CreateShaderObject();
IShaderObject fragment = Graphik.CreateShaderObject();

var vertexResult = ShaderCompiler.Compile(File.ReadAllText("assets/test.hlsl"), "test.vert", 
	ShaderType.VertexShader, "vertex");

Console.WriteLine(vertexResult);

if (!vertexResult.Success)
{
	Console.Error.WriteLine("Vertex Error: " + vertexResult.Message);
	return 1;
}

var fragmentResult = ShaderCompiler.Compile(File.ReadAllText("assets/test.hlsl"), "test.frag", 
	ShaderType.FragmentShader, "fragment");

Console.WriteLine(fragmentResult);

if (!fragmentResult.Success)
{
	Console.Error.WriteLine("Fragment Error: " + fragmentResult.Message);
	return 1;
}
	
ShaderBuildResult vertexBuildResult = vertex.Build(vertexResult);

if (vertexBuildResult.Status != ShaderCompilationStatus.Success)
{
	Console.Error.WriteLine(vertexBuildResult.Log);
	return 1;
}

ShaderBuildResult fragmentBuildResult = fragment.Build(fragmentResult);

if (fragmentBuildResult.Status != ShaderCompilationStatus.Success)
{
	Console.Error.WriteLine(fragmentBuildResult.Log);
	return 1;
}

shader.Attach(vertex);
shader.Attach(fragment);

Console.WriteLine("Linking log: " + shader.Link());

StbImage.stbi_set_flip_vertically_on_load(1);
ImageResult imageResult = ImageResult.FromMemory(File.ReadAllBytes("assets/logo.png"), ColorComponents.RedGreenBlueAlpha);

byte[] imageData = imageResult.Data;

ITexture2D texture = Graphik.CreateTexture();
texture.PrepareModifications();
texture.SetData(TextureFormat.Rgba8, imageData, imageResult.Width, imageResult.Height);

IRenderTexture renderTexture = Graphik.CreateRenderTexture();
renderTexture.Build(512, 512);

while (!Graphik.WindowShouldClose())
{
	Graphik.InitializeFrame();
	Graphik.Clear(ClearFlags.Colour | ClearFlags.Depth);
	
	shader.Use();
	mesh.Render(6);
	
	Graphik.FinishFrame();
}

return 0;