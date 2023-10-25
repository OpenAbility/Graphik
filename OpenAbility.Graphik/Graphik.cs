namespace OpenAbility.Graphik;

public static partial class Graphik
{
	private static IGraphikAPI api = null!;
	/// <summary>
	/// The global file read handler and include handler
	/// </summary>
	public static FileReadHandler FileReadHandler 
		= (_, _) => throw new NotSupportedException("No file read handler has been defined!");

	/// <summary>
	/// Set the API for Graphik to use
	/// </summary>
	/// <param name="newAPI">The new API</param>
	public static void SetAPI(IGraphikAPI newAPI)
	{
		api = newAPI;
	}
	
	/// <summary>
	/// Get the API used by Graphik for e.v native calls etc
	/// </summary>
	/// <returns>The API used</returns>
	public static IGraphikAPI GetAPI()
	{
		return api;
	}
}