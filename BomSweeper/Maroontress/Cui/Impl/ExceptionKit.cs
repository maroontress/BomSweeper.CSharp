namespace Maroontress.Cui.Impl
{
    using System;

    /// <summary>
    /// The factory of <see cref="OptionParsingException"/> objects.
    /// </summary>
    public sealed class ExceptionKit
    {
        private readonly OptionSchema schema;
        private readonly Func<string> option;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionKit"/> class.
        /// </summary>
        /// <param name="schema">
        /// The option schema.
        /// </param>
        /// <param name="option">
        /// The long name of the option.
        /// </param>
        public ExceptionKit(OptionSchema schema, string option)
        {
            this.schema = schema;
            this.option = () => option;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionKit"/> class.
        /// </summary>
        /// <param name="schema">
        /// The option schema.
        /// </param>
        /// <param name="option">
        /// The short name of the option.
        /// </param>
        public ExceptionKit(OptionSchema schema, char option)
        {
            this.schema = schema;
            this.option = () => $"-{option}";
        }

        /// <summary>
        /// Gets a new <see cref="OptionParsingException"/> representing
        /// that the option which requires an argument has no argument.
        /// </summary>
        /// <returns>
        /// The new exception.
        /// </returns>
        public Exception NoArgument()
        {
            return new OptionParsingException(
                schema, $"no argument of option: {option()}");
        }

        /// <summary>
        /// Gets a new <see cref="OptionParsingException"/> representing
        /// that the option which requires no argument has an extra argument.
        /// </summary>
        /// <returns>
        /// The new exception.
        /// </returns>
        public Exception UnableToGetArgument()
        {
            throw new OptionParsingException(
                schema, $"unable to get argument of option: {option()}");
        }

        /// <summary>
        /// Gets a new <see cref="OptionParsingException"/> representing
        /// that the option is not found in the schema.
        /// </summary>
        /// <returns>
        /// The new exception.
        /// </returns>
        public Exception UnknownOption()
        {
            throw new OptionParsingException(
                schema, $"unknown option: {option()}");
        }
    }
}
