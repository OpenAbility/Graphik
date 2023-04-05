using OpenAbility.Graphik;
using OpenAbility.Graphik.OpenGL;
using StbImageSharp;

GLAPI graphik = new GLAPI();

graphik.InitializeSystems();
graphik.InitializeWindow("Hello, World Graphik!", 1280, 720);

float[] vertices =
{
	// x, y, z, u, v
	-0.5f, -0.5f, 0.5f, 0, 0,
	-0.5f,  0.5f, 0.5f, 0, 1,
	 0.5f, -0.5f, 0.5f, 1, 0,
	 0.5f,  0.5f, 0.5f, 1, 1,
};

IMesh mesh = graphik.CreateMesh();
mesh.PrepareModifications();
mesh.SetVertexData(vertices);
mesh.SetVertexAttrib(0, 3, VertexAttribType.Float, 5 * sizeof(float), 0);
mesh.SetVertexAttrib(1, 2, VertexAttribType.Float, 5 * sizeof(float), 3 * sizeof(float));
mesh.SetIndices(new uint[] {0, 1, 2, 1, 3, 2});

IShader shader = graphik.CreateShader();
shader.AttachSource(@"
#version 330 core

layout(location=0) in vec3 pos;
layout(location=1) in vec2 uv;
out vec2 TexCoord;

void main()
{
    gl_Position = vec4(pos, 1.0);
	TexCoord = uv;
}
", ShaderType.VertexShader);
shader.AttachSource(@"
#version 330 core
out vec4 FragColor;
in vec2 TexCoord;

uniform sampler2D tex;

void main()
{
    FragColor = texture(tex, TexCoord);
}
", ShaderType.FragmentShader);


Console.WriteLine(shader.Compile());
Console.WriteLine(shader.Link());

StbImage.stbi_set_flip_vertically_on_load(1);
ImageResult imageResult = ImageResult.FromMemory(File.ReadAllBytes("assets/logo.png"), ColorComponents.RedGreenBlueAlpha);

byte[] imageData = imageResult.Data;

ITexture texture = graphik.CreateTexture();
texture.PrepareModifications();
texture.SetData(TextureFormat.Rgba8, imageData, imageResult.Width, imageResult.Height);

while (!graphik.WindowShouldClose())
{
	graphik.InitializeFrame();
	graphik.Clear(ClearFlags.Colour | ClearFlags.Depth);
	
	shader.Use();
	texture.Bind();
	shader.BindInt("tex", 0);
	mesh.Render(6);
	
	graphik.FinishFrame();
}