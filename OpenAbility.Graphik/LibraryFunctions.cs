namespace OpenAbility.Graphik;

/// <summary>
/// A collection of common library function names for InvokeLibraryFunction
/// </summary>
public static class LibraryFunctions
{
	/// <summary>
	/// No arguments, should complete the GPU queue.
	/// </summary>
	public const string FinishProcessing = "finish_processing";
	/// <summary>
	/// No arguments, should get the clipboard contents as a string.
	/// </summary>
	public const string GetClipboardString = "get_clipboard_str";
	/// <summary>
	/// One argument, string, should set the clipboard contents.
	/// </summary>
	public const string SetClipboardString = "set_clipboard_str";
}
