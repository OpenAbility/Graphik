using System.Text;

namespace OpenAbility.Graphik.OpenGL;

internal static class Utility
{
	public static T[] Wrap<T>(T value)
	{
		return new []
		{
			value
		};
	}

	public static unsafe void EncodeInto(this string s, Span<byte> buffer, bool terminated = true)
	{
		for (int i = 0; i < s.Length; i++)
		{
			buffer[i] = (byte)s[i];
		}
		if (terminated)
			buffer[s.Length] = 0;
	}

	public static unsafe string GetString(byte* pointer)
	{
		StringBuilder result = new StringBuilder();
		while (*pointer != 0)
		{
			result.Append((char)*pointer);
			pointer++;
		}
		return result.ToString();
	}
}
