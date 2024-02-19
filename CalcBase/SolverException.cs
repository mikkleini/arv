namespace CalcBase
{
    /// <summary>
    /// Solver exception
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="message">Message</param>
    public class SolverException(string message) : Exception(message)
    {
    }
}
