// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using MicroBenchmarks;

namespace System.Collections.Tests
{
    public class Perf_Dictionary_String_Base
    {
        public struct Wrapper
        {
            public IEqualityComparer<string> Value { get; }
            private readonly string _name;

            public Wrapper(IEqualityComparer<string> value, string name)
            {
                Value = value;
                _name = name;
            }

            public override string ToString() => _name;
        }

        // Wrapper is used to work around https://github.com/dotnet/BenchmarkDotNet/issues/1177 and to improve summary display
        public static IEnumerable<Wrapper> Comparers => new[]
        {
            new Wrapper(EqualityComparer<string>.Default, "Default"),
            new Wrapper(StringComparer.Ordinal, "Ordinal"),
            new Wrapper(StringComparer.OrdinalIgnoreCase, "OrdinalIgnoreCase")
        };

        [ParamsSource(nameof(Comparers))]
        public Wrapper Comparer;
    }

    [BenchmarkCategory(Categories.CoreFX, Categories.Collections, Categories.GenericCollections)]
    public class Perf_Dictionary_String1 : Perf_Dictionary_String_Base
    {
        [Benchmark]
        public object Construct()
        {
            return new Dictionary<string, string>(Comparer.Value);
        }
    }

    [BenchmarkCategory(Categories.CoreFX, Categories.Collections, Categories.GenericCollections)]
    public class Perf_Dictionary_String2 : Perf_Dictionary_String_Base
    {
        [Params(1, 10, 90, 110, 1_000, 10_000)]
        public int N;

        [ParamsAllValues]
        public bool Collisions;

        protected Dictionary<string, string> _dict;

        protected string[] _keys;

        [GlobalSetup]
        public void Initialize()
        {
            _dict = Enumerable.Range(0, N)
                .Select(i => string.Concat(Enumerable.Repeat(Collisions ? "\uA0A2\uA0A2" : "aa", i)))
                .ToDictionary(s => s, Comparer.Value);

            _keys = Enumerable.Range(0, N).Select(i => i.ToString()).ToArray();
        }

        [Benchmark]
        public int ContainsKey()
        {
            var d = _dict;
            int count = 0;

            foreach (var value in d.Keys)
            {
                if (d.ContainsKey(value))
                {
                    count++;
                }
            }

            return count;
        }

        [Benchmark]
        public int DoesntContainKey()
        {
            var d = _dict;
            int count = 0;

            foreach (var value in _keys)
            {
                if (d.ContainsKey(value))
                {
                    count++;
                }
            }

            return count;
        }

        [Benchmark]
        public object GetComparer()
        {
            return _dict.Comparer;
        }
    }

    [BenchmarkCategory(Categories.CoreFX, Categories.Collections, Categories.GenericCollections)]
    public class Perf_Dictionary_String3
    {
        public int N = 100;

        protected Dictionary<int, int> _dict;

        protected int[] _values;

        [GlobalSetup]
        public void Initialize()
        {
            _dict = Enumerable.Range(1, N)
                .ToDictionary(i => i);

            _values = Enumerable.Range(1, N)
                .Select(i => -i)
                .ToArray();
        }

        [Benchmark]
        public object Construct()
        {
            return new Dictionary<int, int>();
        }

        [Benchmark]
        public int ContainsKey()
        {
            var d = _dict;
            int count = 0;

            foreach (var value in d.Values)
            {
                if (d.ContainsKey(value))
                {
                    count++;
                }
            }

            return count;
        }

        [Benchmark]
        public int DoesntContainKey()
        {
            var d = _dict;
            int count = 0;

            foreach (var value in _values)
            {
                if (d.ContainsKey(value))
                {
                    count++;
                }
            }

            return count;
        }

        [Benchmark]
        public object GetComparer()
        {
            return _dict.Comparer;
        }
    }

    [BenchmarkCategory(Categories.CoreFX, Categories.Collections, Categories.GenericCollections)]
    public class Perf_Dictionary_String4
    {
        public int N = 100;

        protected Dictionary<object, object> _dict;

        protected object[] _values;

        [GlobalSetup]
        public void Initialize()
        {
            _dict = Enumerable.Range(1, N)
                .Select(i => (object)i)
                .ToDictionary(i => i);

            _values = Enumerable.Range(1, N)
                .Select(i => (object)-i)
                .ToArray();
        }

        [Benchmark]
        public object Construct()
        {
            return new Dictionary<object, object>();
        }

        [Benchmark]
        public int ContainsKey()
        {
            var d = _dict;
            int count = 0;

            foreach (var value in d.Values)
            {
                if (d.ContainsKey(value))
                {
                    count++;
                }
            }

            return count;
        }

        [Benchmark]
        public int DoesntContainKey()
        {
            var d = _dict;
            int count = 0;

            foreach (var value in _values)
            {
                if (d.ContainsKey(value))
                {
                    count++;
                }
            }

            return count;
        }

        [Benchmark]
        public object GetComparer()
        {
            return _dict.Comparer;
        }
    }
}
