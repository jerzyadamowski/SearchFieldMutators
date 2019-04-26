using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFM
{
    public delegate IList<TQuery> QueryMutatorList<TQuery, TSearch>(IList<TQuery> item, TSearch model);
    
    /// <summary>
    /// Mutator class
    /// </summary>
    /// <typeparam name="TSearch">Predicate expression</typeparam>
    /// <typeparam name="TQuery">Query expression if TSearch == true</typeparam>
    public class SearchFieldMutatorList<TQuery, TSearch>
    {
        /// <summary>
        /// Search Field Mutator construct
        /// </summary>
        /// <param name="condition">Condition to check</param>
        /// <param name="mutator">Expression to run if true</param>
        public SearchFieldMutatorList(Predicate<TSearch> condition, QueryMutatorList<TQuery, TSearch> mutator)
        {
            Condition = condition;
            Mutator = mutator;
        }

        public Predicate<TSearch> Condition { get; set; }

        public QueryMutatorList<TQuery, TSearch> Mutator { get; set; }

        public TSearch Model { get; set; }

        /// <summary>
        /// Applies the mutator to an IQueryable object if Search == true
        /// </summary>
        /// <param name="search">Object to check for conditions</param>
        /// <param name="query">Object to run the expression on</param>
        /// <returns>Result set</returns>
        public IList<TQuery> Apply(IList<TQuery> query, TSearch search)
        {
            return Condition(search) ? Mutator(query, search) : query;
        }
    }
}
