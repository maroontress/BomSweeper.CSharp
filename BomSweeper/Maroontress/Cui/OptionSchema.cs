namespace Maroontress.Cui
{
    using System;
    using System.Collections.Generic;
    using StyleChecker.Annotations;

    /// <summary>
    /// The definition of the command line options.
    /// </summary>
    public interface OptionSchema
    {
        /// <summary>
        /// Adds an option that has the argument with the short name, the
        /// specified Action, the argument name and the specified description.
        /// </summary>
        /// <param name="name">
        /// The name of the option. In general, it includes lowercase letters
        /// and hyphens. The name must be unique in this object.
        /// </param>
        /// <param name="shortName">
        /// The short name of the option. The short name must be unique in this
        /// object.
        /// </param>
        /// <param name="action">
        /// The action to be invoked when the specified option and its argument
        /// are found while <see cref="OptionSchema.Parse(string[])"/> runs.
        /// </param>
        /// <param name="argumentName">
        /// The argument name of the option.
        /// </param>
        /// <param name="description">
        /// The description of the option. It can contain '\n', which
        /// represents a line separator.
        /// </param>
        /// <returns>
        /// The new Options object.
        /// </returns>
        [return: DoNotIgnore]
        OptionSchema Add(
            string name,
            char? shortName,
            Action<ValueOption> action,
            string argumentName,
            string description);

        /// <summary>
        /// Adds an option that has no argument with the short name, the
        /// specified Action and the specified description.
        /// </summary>
        /// <param name="name">
        /// The name of the option. It can include lowercase letters and
        /// hyphens. The name must be unique in this object.
        /// </param>
        /// <param name="shortName">
        /// The short name of the option. The short name must be unique in this
        /// object.
        /// </param>
        /// <param name="action">
        /// The action to be invoked when this option and its argument are
        /// found while <see cref="OptionSchema.Parse(string[])"/> runs.
        /// </param>
        /// <param name="description">
        /// The description of the option. It can contain '\n', which
        /// represents a line separator.
        /// </param>
        /// <returns>
        /// The new Options object.
        /// </returns>
        [return: DoNotIgnore]
        OptionSchema Add(
            string name,
            char? shortName,
            Action<Option> action,
            string description);

        /// <summary>
        /// Adds an option that has no argument with the short name, the
        /// specified description.
        /// </summary>
        /// <param name="name">
        /// The name of the option. It can include lowercase letters and
        /// hyphens. The name must be unique in this object.
        /// </param>
        /// <param name="shortName">
        /// The short name of the option. The short name must be unique in this
        /// object.
        /// </param>
        /// <param name="description">
        /// The description of the option. It can contain '\n', which
        /// represents a line separator.
        /// </param>
        /// <returns>
        /// The new Options object.
        /// </returns>
        [return: DoNotIgnore]
        OptionSchema Add(string name, char? shortName, string description);

        /// <summary>
        /// Get a new OptionParser object associated with this Options and
        /// the specified command-line options.
        /// </summary>
        /// <param name="args">
        /// The command-line options.
        /// </param>
        /// <returns>
        /// The new Setting object.
        /// </returns>
        Setting Parse(string[] args);

        /// <summary>
        /// Gets the description of the command-line options.
        /// </summary>
        /// <returns>
        /// The description of this Options.
        /// </returns>
        IEnumerable<string> GetHelpMessage();
    }
}
