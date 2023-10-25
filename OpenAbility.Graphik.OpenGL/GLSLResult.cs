using Silk.NET.Shaderc;

namespace OpenAbility.Graphik.OpenGL;

public class GLSLResult
{
	public readonly string GLSL;
	public readonly ShaderType ShaderType;
	
	public GLSLResult(string glsl, ShaderType shaderType)
	{
		GLSL = glsl;
		ShaderType = shaderType;
	}

	public override string ToString()
	{
		return ShaderType + " shader, GLSL:\n" + GLSL;
	}
}
