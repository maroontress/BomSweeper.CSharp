namespace Maroontress.Cui
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an Option with an argument of the command-line options.
    /// </summary>
    public interface ValueOption : Option
    {
        /// <summary>
        /// Gets the argument name of the option if any.
        /// </summary>
        string? ArgumentName { get; }

        /// <summary>
        /// Gets the values of the arguments if any,
        /// <c>Enumerable.Empty&lt;string&gt;()</c> otherwise.
        /// </summary>
        IEnumerable<string> Values { get; }

        /// <summary>
        /// Gets the value of the last argument.
        /// </summary>
        string Value { get; }
    }
}
