namespace CalcBase
{
    /// <summary>
    /// Solver exception
    /// </summary>
    internal class SolverException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="position">Errornous expression position</param>
        /// <param name="length">Errornous expression length</param>
        public SolverException(string message)
            : base(message)
        {
        }
    }
}
