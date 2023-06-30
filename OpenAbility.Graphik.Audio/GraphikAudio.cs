namespace OpenAbility.Graphik.Audio;

public static partial class GraphikAudio
{
	private static IGraphikAudioAPI audioAPI = null!;

	public static void SetAudioAPI(IGraphikAudioAPI api)
	{
		audioAPI = api;
	}

	public static void EnqueueData(float[] data, int sampleFrequency) => audioAPI.EnqueueData(data, sampleFrequency);
	public static void PausePlayback() => audioAPI.PausePlayback();
	public static void ResumePlayback() => audioAPI.ResumePlayback();
	public static bool RequiresNewData() => audioAPI.RequiresNewData();
	public static void Close() => audioAPI.Close();
}
