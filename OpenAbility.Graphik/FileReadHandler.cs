namespace OpenAbility.Graphik;

/// <summary>
/// Handles file reading and writing
/// <param name="requested">The requested file</param>
/// <param name="relative">The file or directory it is relative to</param>
/// <remarks><c>relative</c> may be null, in which case the program resource root should be assumed</remarks>
/// </summary>
public delegate string FileReadHandler(string requested, string? relative);
