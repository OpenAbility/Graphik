namespace OpenAbility.Graphik;

public enum UniformType
{
	Unknown,
	Int,
	UInt,
	Float,
	Double,
	Bool,
	Vector2,
	Vector3,
	Vector4,
	Vector2Int,
	Vector3Int,
	Vector4Int,
	Vector2Bool,
	Vector3Bool,
	Vector4Bool,
	Matrix2x2,
	Matrix3x3,
	Matrix4x4,
	Texture1D,
	Texture2D,
	Texture3D,
	Cubemap,
	Matrix2x3,
	Matrix2x4,
	Matrix3x2,
	Matrix3x4,
	Matrix4x2,
	Matrix4x3,
	Texture1DArray,
	Texture2DArray,
	CubemapArray,
	Vector2Uint,
	Vector3Uint,
	Vector4Uint,
	Vector2Double,
	Vector3Double,
	Vector4Double,
	Matrix2x2Double,
	Matrix3x3Double,
	Matrix4x4Double,
	Matrix2x3Double,
	Matrix2x4Double,
	Matrix3x2Double,
	Matrix3x4Double,
	Matrix4x2Double,
	Matrix4x3Double,
}

public static class UniformTypeHelpers
{
	/// <summary>
	/// Get the size of all Vector[X][Type] UniformTypes
	/// </summary>
	/// <returns>The size, or 0 if UniformType is not a vector type</returns>
	public static int GetVectorSize(UniformType uniformType)
	{
		return uniformType switch
		{
			UniformType.Int => 1,
			UniformType.UInt => 1,
			UniformType.Float => 1,
			UniformType.Double => 1,
			UniformType.Bool => 1,
			UniformType.Vector2 => 2,
			UniformType.Vector3 => 3,
			UniformType.Vector4 => 4,
			UniformType.Vector2Int => 2,
			UniformType.Vector3Int => 3,
			UniformType.Vector4Int => 4,
			UniformType.Vector2Bool => 2,
			UniformType.Vector3Bool => 3,
			UniformType.Vector4Bool => 4,
			UniformType.Vector2Uint => 2,
			UniformType.Vector3Uint => 3,
			UniformType.Vector4Uint => 4,
			UniformType.Vector2Double => 2,
			UniformType.Vector3Double => 3,
			UniformType.Vector4Double => 4,
			_ => 0
		};
	}

	/// <summary>
	/// Gets if a UniformType is a vector
	/// </summary>
	public static bool IsVector(UniformType uniformType)
	{
		return uniformType switch
		{
			UniformType.Vector2 => true,
			UniformType.Vector3 => true,
			UniformType.Vector4 => true,
			UniformType.Vector2Int => true,
			UniformType.Vector3Int => true,
			UniformType.Vector4Int => true,
			UniformType.Vector2Bool => true,
			UniformType.Vector3Bool => true,
			UniformType.Vector4Bool => true,
			UniformType.Vector2Uint => true,
			UniformType.Vector3Uint => true,
			UniformType.Vector4Uint => true,
			UniformType.Vector2Double => true,
			UniformType.Vector3Double => true,
			UniformType.Vector4Double => true,
			_ => false
		};
	}

	/// <summary>
	/// Gets the underlying "base" type, e.g. float, int, uint or bool
	/// </summary>
	/// <returns>The base type, or Unknown if not possible</returns>
	/// <remarks>Does not work on non-array Textures</remarks>
	public static UniformType GetBaseType(UniformType uniformType)
	{
		return uniformType switch
		{
			UniformType.Int => UniformType.Int,
			UniformType.UInt => UniformType.UInt,
			UniformType.Float => UniformType.Float,
			UniformType.Double => UniformType.Double,
			UniformType.Bool => UniformType.Bool,
			UniformType.Vector2 => UniformType.Float,
			UniformType.Vector3 => UniformType.Float,
			UniformType.Vector4 => UniformType.Float,
			UniformType.Vector2Int => UniformType.Int,
			UniformType.Vector3Int => UniformType.Int,
			UniformType.Vector4Int => UniformType.Int,
			UniformType.Vector2Bool => UniformType.Bool,
			UniformType.Vector3Bool => UniformType.Bool,
			UniformType.Vector4Bool => UniformType.Bool,
			UniformType.Matrix2x2 => UniformType.Float,
			UniformType.Matrix3x3 => UniformType.Float,
			UniformType.Matrix4x4 => UniformType.Float,
			UniformType.Matrix2x3 => UniformType.Float,
			UniformType.Matrix2x4 => UniformType.Float,
			UniformType.Matrix3x2 => UniformType.Float,
			UniformType.Matrix3x4 => UniformType.Float,
			UniformType.Matrix4x2 => UniformType.Float,
			UniformType.Matrix4x3 => UniformType.Float,
			UniformType.Texture1DArray => UniformType.Texture1D,
			UniformType.Texture2DArray => UniformType.Texture2D,
			UniformType.CubemapArray => UniformType.CubemapArray,
			UniformType.Vector2Uint => UniformType.UInt,
			UniformType.Vector3Uint => UniformType.UInt,
			UniformType.Vector4Uint => UniformType.UInt,
			UniformType.Vector2Double => UniformType.Double,
			UniformType.Vector3Double => UniformType.Double,
			UniformType.Vector4Double => UniformType.Double,
			UniformType.Matrix2x2Double => UniformType.Double,
			UniformType.Matrix3x3Double => UniformType.Double,
			UniformType.Matrix4x4Double => UniformType.Double,
			UniformType.Matrix2x3Double => UniformType.Double,
			UniformType.Matrix2x4Double => UniformType.Double,
			UniformType.Matrix3x2Double => UniformType.Double,
			UniformType.Matrix3x4Double => UniformType.Double,
			UniformType.Matrix4x2Double => UniformType.Double,
			UniformType.Matrix4x3Double => UniformType.Double,
			_ => UniformType.Unknown
		};
	}

	/// <summary>
	/// Gets if a type is a texture
	/// </summary>
	public static bool IsTexture(UniformType uniformType)
	{
		return uniformType switch
		{
			UniformType.Texture1D => true,
			UniformType.Texture2D => true,
			UniformType.Texture3D => true,
			UniformType.Cubemap => true,
			UniformType.Texture1DArray => true,
			UniformType.Texture2DArray => true,
			UniformType.CubemapArray => true,
			_ => false,
		};
	}
	
	/// <summary>
	/// Gets if a type is a texture array
	/// </summary>
	public static bool IsTextureArray(UniformType uniformType)
	{
		return uniformType switch
		{
			UniformType.Texture1DArray => true,
			UniformType.Texture2DArray => true,
			UniformType.CubemapArray => true,
			_ => false,
		};
	}

	/// <summary>
	/// Gets the number of dimensions to a texture type
	/// </summary>
	/// <returns>The number of dimensions, or 0 if not valid</returns>
	/// <remarks>Does not work on cubemaps or on arrays</remarks>
	public static int GetTextureDimensions(UniformType uniformType)
	{
		return uniformType switch
		{
			UniformType.Texture1D => 1,
			UniformType.Texture2D => 2,
			UniformType.Texture3D => 3,
			_ => 0
		};
	}

	/// <summary>
	/// Gets the dimensions of a matrix type
	/// </summary>
	/// <returns>The dimensions, or (0, 0) if not valid</returns>
	public static (int width, int height) GetMatrixDimensions(UniformType uniformType)
	{
		return uniformType switch
		{
			UniformType.Matrix2x2 => (2, 2),
			UniformType.Matrix3x3 => (3, 3),
			UniformType.Matrix4x4 => (4, 4),
			UniformType.Matrix2x3 => (2, 3),
			UniformType.Matrix2x4 => (2, 4),
			UniformType.Matrix3x2 => (3, 2),
			UniformType.Matrix3x4 => (3, 4),
			UniformType.Matrix4x2 => (4, 2),
			UniformType.Matrix4x3 => (4, 3),
			UniformType.Matrix2x2Double => (2, 2),
			UniformType.Matrix3x3Double => (3, 3),
			UniformType.Matrix4x4Double => (4, 4),
			UniformType.Matrix2x3Double => (2, 3),
			UniformType.Matrix2x4Double => (2, 4),
			UniformType.Matrix3x2Double => (3, 2),
			UniformType.Matrix3x4Double => (3, 4),
			UniformType.Matrix4x2Double => (4, 2),
			UniformType.Matrix4x3Double => (4, 3),
			_ => (0, 0)
		};
	}
}
