namespace FNAScreenSaver;
/// <summary>
/// программный класс
/// </summary>
public static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
  public static void Main()
    {
        using (var game = new Game())
        {
            game.Run();
        }
    }
}
