namespace BomSweeper;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides The extension methods of <see cref="IEnumerable{T}"/> class.
/// </summary>
public static class Elements
{
    /// <summary>
    /// Inserts the specified <paramref name="separator"/> element between each
    /// element of the specified <see cref="IEnumerable{T}"/> object.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements.
    /// </typeparam>
    /// <param name="all">
    /// The elements to insert the <paramref name="separator"/> into.
    /// </param>
    /// <param name="separator">
    /// The separator to be inserted.
    /// </param>
    /// <returns>
    /// The new <see cref="IEnumerable{T}"/> object. If the length of <paramref
    /// name="all"/> is zero or one, it has the same sequence of the <paramref
    /// name="all"/> and does not contain the <paramref name="separator"/>.
    /// </returns>
    public static IEnumerable<T> Separate<T>(
        this IEnumerable<T> all, T separator)
    {
        return !all.Any()
            ? all
            : all.SelectMany(e => new[] { separator, e })
                .Skip(1);
    }
}
