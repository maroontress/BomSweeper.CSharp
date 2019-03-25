namespace Maroontress.Cui
{
    /// <summary>
    /// Represents an Option of the command-line options.
    /// </summary>
    public interface Option
    {
        /// <summary>
        /// Gets the name of the option.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the short name of the option.
        /// </summary>
        char? ShortName { get; }

        /// <summary>
        /// Gets the description of the option.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the schema of this option.
        /// </summary>
        OptionSchema Schema { get; }
    }
}
