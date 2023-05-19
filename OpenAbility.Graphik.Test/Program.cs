using OpenAbility.Graphik;
using OpenAbility.Graphik.OpenGL;
using StbImageSharp;

Graphik.SetAPI(new GLAPI());
Graphik.InitializeSystems();

Graphik.SetErrorCallback((id, message) =>
{
	Console.WriteLine(id + ": " + message);
});

Graphik.InitializeWindow("Hello, World Graphik!", 1280, 720);

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
vertex.AttachSource(File.ReadAllText("assets/test.vert"), "test.vert", ShaderType.VertexShader);
vertex.Compile();
fragment.AttachSource(File.ReadAllText("assets/test.frag"), "test.frag", ShaderType.FragmentShader);
fragment.Compile();

shader.Attach(vertex);
shader.Attach(fragment);
shader.Link();

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
	
	renderTexture.Target();
	Graphik.Clear(ClearFlags.Colour | ClearFlags.Depth);
	shader.Use();
	texture.Bind();
	shader.BindInt("tex", 0);
	mesh.Render(6);
	
	Graphik.ResetTarget();
	Graphik.Clear(ClearFlags.Colour | ClearFlags.Depth);
	shader.Use();
	renderTexture.Bind(RenderTextureComponent.Colour);
	shader.BindInt("tex", 0);
	mesh.Render(6);
	
	Graphik.FinishFrame();
}

return 0;