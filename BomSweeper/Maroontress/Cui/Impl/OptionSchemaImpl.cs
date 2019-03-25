namespace Maroontress.Cui.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The default implementation of <see cref="OptionSchema"/> interface.
    /// </summary>
    public sealed partial class OptionSchemaImpl : OptionSchema
    {
        /// <summary>
        /// The empty option schema.
        /// </summary>
        public static readonly OptionSchema Empty = new OptionSchemaImpl();

        private const int HeadingSeparatorLength = 2;
        private const int MaxHeadingLength = 32;
        private const int IndentUnit = 4;

        private static readonly Action<Option> NoAction = o => { };

        private readonly ImmutableList<Spec> all;

        private readonly ImmutableDictionary<string, Spec> nameMap;

        private readonly ImmutableDictionary<char, Spec> shortNameMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionSchemaImpl"/>
        /// class.
        /// </summary>
        public OptionSchemaImpl()
        {
            all = ImmutableList<Spec>.Empty;
            nameMap = ImmutableDictionary<string, Spec>.Empty;
            shortNameMap = ImmutableDictionary<char, Spec>.Empty;
        }

        private OptionSchemaImpl(IEnumerable<Spec> p)
        {
            all = p.ToImmutableList();
            nameMap = p.ToImmutableDictionary(o => o.Name);
            shortNameMap = p.Where(o => o.ShortName.HasValue)
                .ToImmutableDictionary(o => o.ShortName.GetValueOrDefault());
        }

        private delegate string HeadingProvier(string n, string a);

        /// <inheritdoc/>
        public OptionSchema Add(
            string name,
            char? shortName,
            Action<ValueOption> action,
            string argumentName,
            string description)
        {
            CheckShortName(shortName);
            CheckDuplication(name, shortName);
            var spec = new ValueOptionSpec(
                name, shortName, action, argumentName, description);
            return new OptionSchemaImpl(all.Append(spec));
        }

        /// <inheritdoc/>
        public OptionSchema Add(
            string name,
            char? shortName,
            Action<Option> action,
            string description)
        {
            CheckShortName(shortName);
            CheckDuplication(name, shortName);
            var spec = new OptionSpec(
                name, shortName, action, description);
            return new OptionSchemaImpl(all.Append(spec));
        }

        /// <inheritdoc/>
        public OptionSchema Add(
            string name, char? shortName, string description)
        {
            return Add(name, shortName, NoAction, description);
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetHelpMessage()
        {
            /*
                |<---- heading length ---->|
                |<-- short heading -->|     description
                                            description (continued)
                |<-------- long heading -------->|
                                            description
                                            description (continued)
            */
            string GetHeading(Spec s)
            {
                var shortName = s.ShortName;
                var (@long, @short) = s.GetHelpHeading();

                if (shortName.HasValue)
                {
                    var tuples = new[] { @short, @long };
                    return string.Join(", ", tuples.Select(f => f()));
                }
                return $"    {@long()}";
            }

            int Comparator(Spec s1, Spec s2)
                => string.CompareOrdinal(s1.GetSortName(), s2.GetSortName());

            var allHeadings = all.Sort(Comparator)
                .Select(s => (spec: s, heading: GetHeading(s)));
            var headingLength = allHeadings
                .Select(h => h.heading.Length + HeadingSeparatorLength)
                .Max();
            if (headingLength > MaxHeadingLength)
            {
                headingLength = MaxHeadingLength;
            }
            var u = IndentUnit - 1;
            headingLength = (headingLength + u) & ~u;

            var b = new StringBuilder();
            var list = new List<string>();
            foreach (var (o, h) in allHeadings)
            {
                var d = o.Description.Split('\n');
                var first = d.First();
                var remaining = d.Skip(1);
                var n = h.Length + HeadingSeparatorLength;
                if (n > headingLength)
                {
                    list.Add(h);
                    b.Clear();
                    b.Append(' ', headingLength);
                    b.Append(first);
                    list.Add(b.ToString());
                }
                else
                {
                    b.Clear();
                    b.Append(h);
                    b.Append(' ', headingLength - h.Length);
                    b.Append(first);
                    list.Add(b.ToString());
                }
                foreach (var r in remaining)
                {
                    b.Clear();
                    b.Append(' ', headingLength);
                    b.Append(r);
                    list.Add(b.ToString());
                }
            }
            return list;
        }

        /// <inheritdoc/>
        public Setting Parse(string[] args)
        {
            var queue = new Queue<string>(args);
            var list = new List<Option>(args.Length);
            var map = new Dictionary<ValueOptionSpec, IEnumerable<string>>();

            IEnumerable<string> GetValues(ValueOptionSpec s)
                => map.TryGetValue(s, out var a)
                    ? a : Enumerable.Empty<string>();

            void NewOption(OptionSpec s)
            {
                var o = new OptionImpl(s, this);
                s.Fire(o);
                list.Add(o);
            }

            void NewValueOption(ValueOptionSpec s, string v)
            {
                var a = GetValues(s).Append(v);
                var o = new ValueOptionImpl(s, this, a);
                s.Fire(o);
                list.Add(o);
                map[s] = a.ToImmutableArray();
            }

            var factory = new Factory(NewOption, NewValueOption);
            while (queue.Count > 0 && ParseOption(queue, factory))
            {
                continue;
            }
            return new SettingImpl(this, queue, list);
        }

        private static void CheckShortName(char? shortName)
        {
            if (!shortName.HasValue)
            {
                return;
            }
            var c = shortName.Value;
            if (c == '-')
            {
                throw new InvalidOptionSchemaException(
                    $"The short name '-' is an invalid character.");
            }
        }

        private void CheckDuplication(string name, char? shortName)
        {
            if (nameMap.ContainsKey(name))
            {
                throw new InvalidOptionSchemaException(
                    $"The name of the Option '{name}' is already added.");
            }
            if (shortName.HasValue
                && shortNameMap.ContainsKey(shortName.Value))
            {
                throw new InvalidOptionSchemaException(
                    $"The short name of the Option '{shortName.Value}' "
                    + "is already added.");
            }
        }

        private bool ParseOption(Queue<string> queue, Factory factory)
        {
            var s = queue.Peek();
            if (s is "-")
            {
                queue.Dequeue();
                return false;
            }
            if (s.StartsWith("--"))
            {
                queue.Dequeue();
                ParseLongOption(s, factory);
                return true;
            }
            if (!s.StartsWith("-", StringComparison.InvariantCulture))
            {
                return false;
            }
            queue.Dequeue();
            var shortNameQueue = new Queue<char>(s);
            shortNameQueue.Dequeue();
            while (shortNameQueue.Count > 1)
            {
                var c = shortNameQueue.Dequeue();
                ParseNoArgumentShortOption(c, factory);
            }
            var last = shortNameQueue.Dequeue();
            ParseShortOption(last, queue, factory);
            return true;
        }

        private void ParseNoArgumentShortOption(char c, Factory factory)
        {
            var exceptionOf = new ExceptionKit(this, c);
            if (!shortNameMap.TryGetValue(c, out var spec))
            {
                throw exceptionOf.UnknownOption();
            }
            spec.VisitNoArgShortOption(exceptionOf, factory);
        }

        private void ParseShortOption(
            char c, Queue<string> queue, Factory factory)
        {
            var exceptionOf = new ExceptionKit(this, c);
            if (!shortNameMap.TryGetValue(c, out var spec))
            {
                throw exceptionOf.UnknownOption();
            }
            spec.VisitShortOption(exceptionOf, factory, queue);
        }

        private void ParseLongOption(string s, Factory factory)
        {
            var exceptionOf = new ExceptionKit(this, s);
            var n = s.IndexOf('=');
            (var name, string? value) = (n < 0)
                ? (s.Substring(2), null)
                : (s.Substring(2, n - 2), s.Substring(n + 1));

            if (!nameMap.TryGetValue(name, out var spec))
            {
                throw exceptionOf.UnknownOption();
            }
            spec.VisitLongOption(exceptionOf, factory, value);
        }
    }
}
