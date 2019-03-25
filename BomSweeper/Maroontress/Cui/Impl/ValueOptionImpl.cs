#pragma warning disable SA1004

namespace Maroontress.Cui.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    /// <summary>
    /// The default implementation of <see cref="ValueOption"/> interface.
    /// </summary>
    public sealed class ValueOptionImpl : ValueOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueOptionImpl"/>
        /// class.
        /// </summary>
        /// <param name="spec">
        /// The specification of the option.
        /// </param>
        /// <param name="schema">
        /// The schema of the option.
        /// </param>
        /// <param name="values">
        /// The argument values of the option.
        /// </param>
        /// <seealso cref="OptionSchema.Add(string, char?, Action{ValueOption},
        /// string, string)"/>
        public ValueOptionImpl(
            ValueOptionSpec spec,
            OptionSchema schema,
            IEnumerable<string> values)
        {
            Spec = spec;
            Schema = schema;
            Values = values.ToImmutableArray();
        }

        /// <inheritdoc/>
        public string Name => Spec.Name;

        /// <inheritdoc/>
        public char? ShortName => Spec.ShortName;

        /// <inheritdoc/>
        public string? ArgumentName => Spec.ArgumentName;

        /// <inheritdoc/>
        public string Description => Spec.Description;

        /// <inheritdoc/>
        public IEnumerable<string> Values { get; }

        /// <inheritdoc/>
        public string Value => Values.Last();

        /// <inheritdoc/>
        public OptionSchema Schema { get; }

        private ValueOptionSpec Spec { get; }
    }
}
