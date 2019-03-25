namespace Maroontress.Cui.Impl
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The abstraction of OptionSpec and subclasses.
    /// </summary>
    /// <typeparam name="T">
    /// The type of Option interface.
    /// </typeparam>
    public abstract class AbstractSpec<T> : Spec
        where T : Option
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSpec{T}"/>
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
        public AbstractSpec(
            string name,
            char? shortName,
            Action<T> action,
            string description)
        {
            Name = name;
            ShortName = shortName;
            Action = action;
            Description = description;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public char? ShortName { get; }

        /// <inheritdoc/>
        public string Description { get; }

        private Action<T> Action { get; }

        /// <summary>
        /// Invokes the callback function.
        /// </summary>
        /// <param name="option">
        /// The option to be consumed.
        /// </param>
        public void Fire(T option) => Action(option);

        /// <inheritdoc/>
        public abstract void VisitLongOption(
            ExceptionKit exceptionOf,
            Factory factory,
            string? value);

        /// <inheritdoc/>
        public abstract void VisitShortOption(
            ExceptionKit exceptionOf,
            Factory factory,
            Queue<string> queue);

        /// <inheritdoc/>
        public abstract void VisitNoArgShortOption(
            ExceptionKit exceptionOf,
            Factory factory);

        /// <inheritdoc/>
        public abstract (Func<string> longName, Func<string> shortName)
            GetHelpHeading();

        /// <inheritdoc/>
        public string GetSortName()
        {
            return ShortName.HasValue
                ? ShortName.ToString()
                : Name;
        }
    }
}
