namespace OpenAbility.Graphik.Audio;

public static partial class GraphikAudio
{
	private static IGraphikAudioAPI audioAPI = null!;

	public static void SetAudioAPI(IGraphikAudioAPI api)
	{
		audioAPI = api;
	}
}
