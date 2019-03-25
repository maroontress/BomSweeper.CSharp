namespace Maroontress.Cui.Impl
{
    using System;

    /// <summary>
    /// The factory of options.
    /// </summary>
    public sealed class Factory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Factory"/> class.
        /// </summary>
        /// <param name="newOption">
        /// The action that consumes an <see cref="OptionSpec"/> object.
        /// </param>
        /// <param name="newValueOption">
        /// The action that consumes an <see cref="ValueOptionSpec"/> object
        /// and its argument.
        /// </param>
        public Factory(
            Action<OptionSpec> newOption,
            Action<ValueOptionSpec, string> newValueOption)
        {
            NewOption = newOption;
            NewValueOption = newValueOption;
        }

        /// <summary>
        /// Gets the action that consumes an <see cref="OptionSpec"/> object.
        /// </summary>
        public Action<OptionSpec> NewOption { get; }

        /// <summary>
        /// Gets the action that consumes an <see cref="ValueOptionSpec"/>
        /// object and its argument.
        /// </summary>
        public Action<ValueOptionSpec, string> NewValueOption { get; }
    }
}
