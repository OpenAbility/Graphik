namespace OpenAbility.Graphik.Audio;

public interface IGraphikAudioAPI
{
	public void EnqueueData(float[] data, int sampleFrequency);
	public void PausePlayback();
	public void ResumePlayback();
	public bool RequiresNewData();
	public void Close();
}
