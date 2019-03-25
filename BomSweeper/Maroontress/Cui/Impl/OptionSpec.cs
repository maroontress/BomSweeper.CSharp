namespace Maroontress.Cui.Impl
{
    using System;
    using System.Collections.Generic;
    using StyleChecker.Annotations;

    /// <summary>
    /// The specification part of OptionImpl.
    /// </summary>
    public sealed class OptionSpec : AbstractSpec<Option>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionSpec"/>
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
        /// <param name="description">
        /// The description of the option.
        /// </param>
        /// <seealso cref="OptionSchema.Add(string, char?,
        /// Action{Option}, string)"/>
        public OptionSpec(
            string name,
            char? shortName,
            Action<Option> action,
            string description)
            : base(name, shortName, action, description)
        {
        }

        /// <inheritdoc/>
        public override void VisitLongOption(
            ExceptionKit exceptionOf,
            Factory factory,
            string? value)
        {
            if (!(value is null))
            {
                throw exceptionOf.UnableToGetArgument();
            }
            factory.NewOption(this);
        }

        /// <inheritdoc/>
        public override void VisitShortOption(
            [Unused] ExceptionKit exceptionOf,
            Factory factory,
            [Unused] Queue<string> queue)
        {
            factory.NewOption(this);
        }

        /// <inheritdoc/>
        public override void VisitNoArgShortOption(
            [Unused] ExceptionKit exceptionOf,
            Factory factory)
        {
            factory.NewOption(this);
        }

        /// <inheritdoc/>
        public override (Func<string> longName, Func<string> shortName)
            GetHelpHeading()
        {
            return (() => $"--{Name}", () => $"-{ShortName}");
        }
    }
}
