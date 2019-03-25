namespace Maroontress.Cui.Impl
{
    using System;

    /// <summary>
    /// The default implementation of <see cref="Option"/> interface.
    /// </summary>
    public sealed class OptionImpl : Option
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionImpl"/> class.
        /// </summary>
        /// <param name="spec">
        /// The specification of the option.
        /// </param>
        /// <param name="schema">
        /// The schema of the option.
        /// </param>
        /// <seealso cref="OptionSchema.Add(string, char?,
        /// Action{Option}, string)"/>
        public OptionImpl(OptionSpec spec, OptionSchema schema)
        {
            Spec = spec;
            Schema = schema;
        }

        /// <inheritdoc/>
        public string Name => Spec.Name;

        /// <inheritdoc/>
        public char? ShortName => Spec.ShortName;

        /// <inheritdoc/>
        public string Description => Spec.Description;

        /// <inheritdoc/>
        public OptionSchema Schema { get; }

        private OptionSpec Spec { get; }
    }
}
