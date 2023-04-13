namespace OpenAbility.Graphik;

/// <summary>
/// The type of a shader source, whether it'd be a complete HLSL shader, or a small piece of an OpenGL vertex shader.
/// </summary>
public enum ShaderType
{
	/// <summary>
	/// A shader which contains both vertex and fragment. Possibly geometry shader code too.
	/// This is here in case of HLSL
	/// </summary>
	CompleteShader,
	/// <summary>
	/// The first stage to any shader. This specifies vertex data based on mesh information
	/// </summary>
	VertexShader,
	/// <summary>
	/// The last stage to any shader. This tells us what colour to give to a pixel
	/// </summary>
	FragmentShader,
	/// <summary>
	/// Possibly the second stage to any shader. This transforms vertices, and can create new ones
	/// </summary>
	GeometryShader,
	/// <summary>
	/// A shader type that might not be fully supported. It is used to calculate stuff in parallel on the GPU.
	/// </summary>
	ComputeShader
}
