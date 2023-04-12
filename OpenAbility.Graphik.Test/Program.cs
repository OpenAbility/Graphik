using OpenAbility.Graphik;
using OpenAbility.Graphik.OpenGL;
using StbImageSharp;
using System;
using System.IO;

GLAPI graphik = new GLAPI();

graphik.SetErrorCallback((id, message) =>
{
	Console.WriteLine(id + ": " + message);
});

graphik.InitializeSystems();
graphik.InitializeWindow("Hello, World Graphik!", 1280, 720);

float[] vertices =
{
	// x, y, z, u, v
	-0.5f, -0.5f, 0.0f, 0, 0,
	-0.5f,  0.5f, 0.0f, 0, 1,
	 0.5f, -0.5f, 1.0f, 1, 0,
	 0.5f,  0.5f, 1.0f, 1, 1,
};

IMesh mesh = graphik.CreateMesh();
mesh.PrepareModifications();
mesh.SetVertexData(vertices);
mesh.SetVertexAttrib(0, 3, VertexAttribType.Float, 5 * sizeof(float), 0);
mesh.SetVertexAttrib(1, 2, VertexAttribType.Float, 5 * sizeof(float), 3 * sizeof(float));
mesh.SetIndices(new uint[] {0, 1, 2, 1, 3, 2});

IShader shader = graphik.CreateShader();
shader.AttachSource(File.ReadAllText("assets/test.vert"), "test.vert", ShaderType.VertexShader);
Thread.Sleep(100);
shader.AttachSource(File.ReadAllText("assets/test.frag"), "test.frag", ShaderType.FragmentShader);

var compileResults = shader.Compile();

Console.WriteLine($"Compilation status: {compileResults.Status}, with {compileResults.ErrorCount} errors and {compileResults.WarningCount} warnings. Log:\n{compileResults.Log}");

if (compileResults.Status == ShaderCompilationStatus.Failure)
{
	//return 1;
}

Console.WriteLine("Linking log: " + shader.Link());

StbImage.stbi_set_flip_vertically_on_load(1);
ImageResult imageResult = ImageResult.FromMemory(File.ReadAllBytes("assets/logo.png"), ColorComponents.RedGreenBlueAlpha);

byte[] imageData = imageResult.Data;

ITexture texture = graphik.CreateTexture();
texture.PrepareModifications();
texture.SetData(TextureFormat.Rgba8, imageData, imageResult.Width, imageResult.Height);

GLRenderTexture renderTexture = graphik.CreateRenderTexture();
renderTexture.Build(512, 512);

while (!graphik.WindowShouldClose())
{
	graphik.InitializeFrame();
	graphik.Clear(ClearFlags.Colour | ClearFlags.Depth);
	
	renderTexture.Target();
	graphik.Clear(ClearFlags.Colour | ClearFlags.Depth);
	shader.Use();
	texture.Bind();
	shader.BindInt("tex", 0);
	mesh.Render(6);
	
	graphik.ResetTarget();
	graphik.Clear(ClearFlags.Colour | ClearFlags.Depth);
	shader.Use();
	renderTexture.Bind(RenderTextureComponent.Colour);
	shader.BindInt("tex", 0);
	mesh.Render(6);
	
	graphik.FinishFrame();
}

return 0;