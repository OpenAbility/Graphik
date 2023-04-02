using OpenAbility.Graphik;
using OpenAbility.Graphik.OpenGL;

IGraphikAPI graphik = new GLAPI();

graphik.InitializeSystems();
graphik.InitializeWindow("Hello, World Graphik!", 1280, 720);

while (!graphik.WindowShouldClose())
{
	graphik.InitializeFrame();
	
	graphik.FinishFrame();
}