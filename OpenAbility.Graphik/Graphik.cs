namespace OpenAbility.Graphik;

public static class Graphik
{
	private static IGraphikAPI api = null!;

	/// <summary>
	/// Set the API for Graphik to use
	/// </summary>
	/// <param name="newAPI">The new API</param>
	public static void SetAPI(IGraphikAPI newAPI)
	{
		api = newAPI;
	}
}