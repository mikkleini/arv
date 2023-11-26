namespace CalcBase
{
    /// <summary>
    /// Abstract singleton class.
    /// From: https://stackoverflow.com/a/953441
    /// </summary>
    /// <typeparam name="T">Type of class</typeparam>
    public abstract class Singleton<T> where T : class, new()
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static T Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            internal static readonly T instance = new();
        }
    }
}
