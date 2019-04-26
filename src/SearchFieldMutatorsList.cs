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
    public class SearchFieldMutatorsList<TQuery, TSearch> : IEnumerable<SearchFieldMutatorList<TQuery, TSearch>>
    {
        private readonly IList<SearchFieldMutatorList<TQuery, TSearch>> _inner = new List<SearchFieldMutatorList<TQuery, TSearch>>();

        /// <summary>
        /// Adds a Search Field Mutator item
        /// </summary>
        /// <param name="condition">Condition to check</param>
        /// <param name="mutator">Expression to run if true</param>
        public void Add(Predicate<TSearch> condition, QueryMutatorList<TQuery, TSearch> mutator)
        {
            _inner.Add(new SearchFieldMutatorList<TQuery, TSearch>(condition, mutator));
        }

        public void AddRange(SearchFieldMutatorsList<TQuery, TSearch> collections)
        {
            foreach (var searchFieldMutator in collections)
            {
                _inner.Add(searchFieldMutator);
            }
        }

        public IEnumerator<SearchFieldMutatorList<TQuery, TSearch>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
    }
}
