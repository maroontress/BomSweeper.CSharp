namespace Maroontress.Cui.Impl
{
    using System;
    using System.Collections.Generic;
    using StyleChecker.Annotations;

    /// <summary>
    /// The specification part of ValueOptionImpl.
    /// </summary>
    public sealed class ValueOptionSpec : AbstractSpec<ValueOption>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueOptionSpec"/>
        /// class.
        /// </summary>
        /// <param name="name">
        /// The name of the option.
        /// </param>
        /// <param name="shortName">
        /// The short name of the option.
        /// </param>
        /// <param name="action">
        /// The action to be invoked.
        /// </param>
        /// <param name="argumentName">
        /// The argument name of the option.
        /// </param>
        /// <param name="description">
        /// The description of the option.
        /// </param>
        /// <seealso cref="OptionSchema.Add(string, char?,
        /// Action{ValueOption}, string, string)"/>
        public ValueOptionSpec(
            string name,
            char? shortName,
            Action<ValueOption> action,
            string argumentName,
            string description)
            : base(name, shortName, action, description)
        {
            ArgumentName = argumentName;
        }

        /// <summary>
        /// Gets the argument name.
        /// </summary>
        public string ArgumentName { get; }

        /// <inheritdoc/>
        public override void VisitLongOption(
            ExceptionKit exceptionOf,
            Factory factory,
            string? value)
        {
            if (value is null)
            {
                throw exceptionOf.NoArgument();
            }
            factory.NewValueOption(this, value);
        }

        /// <inheritdoc/>
        public override void VisitShortOption(
            ExceptionKit exceptionOf,
            Factory factory,
            Queue<string> queue)
        {
            if (queue.Count == 0)
            {
                throw exceptionOf.NoArgument();
            }
            var value = queue.Dequeue();
            factory.NewValueOption(this, value);
        }

        /// <inheritdoc/>
        public override void VisitNoArgShortOption(
            ExceptionKit exceptionOf,
            [Unused] Factory factory)
        {
            throw exceptionOf.NoArgument();
        }

        /// <inheritdoc/>
        public override (Func<string> longName, Func<string> shortName)
            GetHelpHeading()
        {
            return (
                () => $"--{Name}={ArgumentName}",
                () => $"-{ShortName} {ArgumentName}");
        }
    }
}
