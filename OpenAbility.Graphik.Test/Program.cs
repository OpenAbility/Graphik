using OpenAbility.Graphik;
using OpenAbility.Graphik.OpenGL;

IGraphikAPI graphik = new GLAPI();

graphik.InitializeSystems();
graphik.InitializeWindow("Hello, World Graphik!", 1280, 720);

float[] vertices =
{
	// x, y, z
	-0.5f, -0.5f, 0.5f,
	 0.0f,  0.5f, 0.5f,
	 0.5f, -0.5f, 0.5f
};

IMesh mesh = graphik.CreateMesh();
mesh.PrepareModifications();
mesh.SetVertexData(vertices);
mesh.SetVertexAttrib(0, 3, VertexAttribType.Float, 3 * sizeof(float), 0);
mesh.SetIndices(new uint[] {0, 1, 2});

IShader shader = graphik.CreateShader();
shader.AttachSource(@"
#version 330 core

layout(location=0) in vec3 pos;

void main()
{
    gl_Position = vec4(pos, 1.0);
}
", ShaderType.VertexShader);
shader.AttachSource(@"
#version 330 core
out vec4 FragColor;
void main()
{
    FragColor = vec4(255, 145, 77, 1);
}
", ShaderType.FragmentShader);


Console.WriteLine(shader.Compile());
Console.WriteLine(shader.Link());

while (!graphik.WindowShouldClose())
{
	graphik.InitializeFrame();
	graphik.Clear(ClearFlags.Colour | ClearFlags.Depth);
	
	shader.Use();
	mesh.Render(3);
	
	graphik.FinishFrame();
}