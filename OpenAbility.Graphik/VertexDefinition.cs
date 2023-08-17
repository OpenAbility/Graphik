namespace OpenAbility.Graphik;

/// <summary>
/// A basic helper class for building mesh data.
/// <remarks>Data is read in the order that attributes are added. Attribute indices are just shader-side!</remarks>
/// </summary>
public class VertexDefinition
{
	private int automatic;
	private readonly List<VertexAttrib> vertexAttribs = new List<VertexAttrib>();
	private uint currentIndex;

	/// <summary>
	/// Add an attribute.
	/// </summary>
	/// <param name="index">The index of the attribute</param>
	/// <param name="size">The size of the attribute in elements(vec3 = 3)</param>
	/// <param name="type">The type of the data</param>
	/// <param name="normalized">If the data should be normalized</param>
	/// <returns>This, for chain calls</returns>
    /// <exception cref="InvalidOperationException">You've used <see cref="AddAutomaticAttribute"/> before this call</exception>
	public VertexDefinition AddAttribute(uint index, int size, VertexAttribType type, bool normalized = false)
	{
		if (automatic == 1)
			throw new InvalidOperationException("Cannot add manual attributes post-automatic attributes!");

		automatic = -1;
		vertexAttribs.Add(new VertexAttrib(index, size, type, normalized));
		
		return this;
	}

	/// <summary>
	/// Add an attribute with an automatic index.
	/// </summary>
	/// <param name="size">The size of the attribute in elements(vec3 = 3)</param>
	/// <param name="type">The type of the data</param>
	/// <param name="normalized">If the data should be normalized</param>
	/// <returns>This, for chain calls</returns>
	/// <exception cref="InvalidOperationException">You've used <see cref="AddAttribute"/> before this call</exception>
	public VertexDefinition AddAutomaticAttribute(int size, VertexAttribType type, bool normalized = false)
	{
		if (automatic == -1)
			throw new InvalidOperationException("Cannot add automatic attributes post-manual attributes!");

		automatic = 1;
		vertexAttribs.Add(new VertexAttrib(currentIndex, size, type, normalized));
		currentIndex++;

		return this;
	}

	private int GetSize(VertexAttrib attrib)
	{
		if (attrib.VertexAttribType == VertexAttribType.UnsignedByte)
			return sizeof(byte) * attrib.Size;
		if (attrib.VertexAttribType == VertexAttribType.Byte)
			return sizeof(sbyte) * attrib.Size;
		if (attrib.VertexAttribType == VertexAttribType.Float)
			return sizeof(float) * attrib.Size;
		if (attrib.VertexAttribType == VertexAttribType.Int)
			return sizeof(int) * attrib.Size;
		if (attrib.VertexAttribType == VertexAttribType.Double)
			return sizeof(double) * attrib.Size;
		return 0;
	}

	/// <summary>
	/// Apply the attributes to a mesh
	/// </summary>
	/// <param name="mesh">The mesh to apply to</param>
	/// <returns>This, for chain calls</returns>
	public VertexDefinition Apply(IMesh mesh)
	{
		int totalSize = 0;
		foreach (var attrib in vertexAttribs)
		{
			totalSize += GetSize(attrib);
		}

		int stride = 0;
		foreach (var attrib in vertexAttribs)
		{
			mesh.SetVertexAttrib(attrib.Index, attrib.Size, attrib.VertexAttribType, stride, totalSize, attrib.Normalized);
			stride += GetSize(attrib);
		}

		return this;
	}
	
	
	private struct VertexAttrib
	{
		public readonly uint Index;
		public readonly int Size;
		public readonly VertexAttribType VertexAttribType;
		public readonly bool Normalized;

		public VertexAttrib(uint index, int size, VertexAttribType vertexAttribType, bool normalized)
		{
			Index = index;
			Size = size;
			VertexAttribType = vertexAttribType;
			Normalized = normalized;
		}
	}
}
