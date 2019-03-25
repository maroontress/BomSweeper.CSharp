namespace Maroontress.Cui.Impl
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The specification of the <see cref="OptionSpec"/>Option.
    /// </summary>
    public interface Spec
    {
        /// <summary>
        /// Gets the name of the option.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the short name of the option, if any.
        /// </summary>
        char? ShortName { get; }

        /// <summary>
        /// Gets the description of the option.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Visits the long option.
        /// </summary>
        /// <param name="exceptionOf">
        /// The exception kit.
        /// </param>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <param name="value">
        /// The argument if any.
        /// </param>
        void VisitLongOption(
            ExceptionKit exceptionOf,
            Factory factory,
            string? value);

        /// <summary>
        /// Visits the short option.
        /// </summary>
        /// <param name="exceptionOf">
        /// The exception kit.
        /// </param>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <param name="queue">
        /// The queue of the command line options. The first element
        /// may be the argument.
        /// </param>
        void VisitShortOption(
            ExceptionKit exceptionOf,
            Factory factory,
            Queue<string> queue);

        /// <summary>
        /// Visits the short option without an argument.
        /// </summary>
        /// <param name="exceptionOf">
        /// The exception kit.
        /// </param>
        /// <param name="factory">
        /// The factory.
        /// </param>
        void VisitNoArgShortOption(
            ExceptionKit exceptionOf,
            Factory factory);

        /// <summary>
        /// Gets the tuple of two functions. The first one returns
        /// a long name, the second one returns a short name.
        /// </summary>
        /// <returns>
        /// The tuple of two functions returning the long/short name,
        /// respectively.
        /// </returns>
        (Func<string> longName, Func<string> shortName) GetHelpHeading();

        /// <summary>
        /// Gets a name for sorting.
        /// </summary>
        /// <returns>
        /// The name for sorting.
        /// </returns>
        string GetSortName();
    }
}
