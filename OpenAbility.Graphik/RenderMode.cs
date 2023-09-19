namespace OpenAbility.Graphik;

/// <summary>
/// Specifies the way in which indices are rendered. The valid values are:
/// <list type="bullet">
///	<item><see cref="Triangle"/></item>
///	<item><see cref="TriangleFan"/></item>
///	<item><see cref="ConvexPolygon"/></item>
/// <item><see cref="Point"/></item>
/// <item><see cref="Lines"/></item>
/// <item><see cref="Line"/></item>
///	<item><see cref="LineLoop"/></item>
/// </list>
/// </summary>
public enum RenderMode
{
	/// <summary>
	/// Render each set of 3 indices as a single triangle
	/// </summary>
	Triangle,
	/// <summary>
	/// Index 0 is treated as a "base", it then constructs a triangle from
	/// 0, i-1, 1
	/// This is useful for drawing convex shapes
	/// </summary>
	TriangleFan,
	/// <summary>
	/// This is for drawing a convex polygon from a set of 3 or more indices.
	/// The same as a <see cref="TriangleFan"/>
	/// </summary>
	ConvexPolygon = TriangleFan,
	/// <summary>
	/// Each index specifies a single point
	/// </summary>
	Point,
	/// <summary>
	/// Draw a set of lines, where each pair of indices is treated as its own line
	/// </summary>
	Lines,
	/// <summary>
	/// Draw a set of lines, where each index is connected to the one before.
	/// </summary>
	Line,
	/// <summary>
	/// Draw a set of lines, like in <see cref="Lines"/>, but the last index is connected to the first.
	/// Combine with <see cref="ConvexPolygon"/> to draw an outlined convex shape
	/// </summary>
	LineLoop
	
}
