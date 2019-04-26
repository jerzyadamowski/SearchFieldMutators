using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFM
{
    /// <summary>
    /// List of SearchFieldMutators
    /// </summary>
    /// <typeparam name="TSearch">Object to perform condition checking on</typeparam>
    /// <typeparam name="TQuery">Object to perform expression on</typeparam>
    public class SearchFieldMutators<TQuery, TSearch> : IEnumerable<SearchFieldMutator<TQuery, TSearch>>
    {
        private readonly IList<SearchFieldMutator<TQuery, TSearch>> _inner = new List<SearchFieldMutator<TQuery, TSearch>>();

        /// <summary>
        /// Adds a Search Field Mutator item
        /// </summary>
        /// <param name="condition">Condition to check</param>
        /// <param name="mutator">Expression to run if true</param>
        public void Add(Predicate<TSearch> condition, QueryMutator<TQuery, TSearch> mutator)
        {
            _inner.Add(new SearchFieldMutator<TQuery, TSearch>(condition, mutator));
        }

        public void AddRange(SearchFieldMutators<TQuery, TSearch> collections)
        {
            foreach (var searchFieldMutator in collections)
            {
                _inner.Add(searchFieldMutator);
            }
        }

        public IEnumerator<SearchFieldMutator<TQuery, TSearch>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
    }
}
