using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFM
{
    public delegate IQueryable<TQuery> QueryMutator<TQuery, TSearch>(IQueryable<TQuery> item, TSearch model);

    /// <summary>
    /// Mutator class
    /// </summary>
    /// <typeparam name="TSearch">Predicate expression</typeparam>
    /// <typeparam name="TQuery">Query expression if TSearch == true</typeparam>
    public class SearchFieldMutator<TQuery, TSearch>
    {
        /// <summary>
        /// Search Field Mutator construct
        /// </summary>
        /// <param name="condition">Condition to check</param>
        /// <param name="mutator">Expression to run if true</param>
        public SearchFieldMutator(Predicate<TSearch> condition, QueryMutator<TQuery, TSearch> mutator)
        {
            Condition = condition;
            Mutator = mutator;
        }

        public Predicate<TSearch> Condition { get; set; }

        public QueryMutator<TQuery, TSearch> Mutator { get; set; }

        public TSearch Model { get; set; }

        /// <summary>
        /// Applies the mutator to an IQueryable object if Search == true
        /// </summary>
        /// <param name="search">Object to check for conditions</param>
        /// <param name="query">Object to run the expression on</param>
        /// <returns>Result set</returns>
        public IQueryable<TQuery> Apply(IQueryable<TQuery> query, TSearch search)
        {
            return Condition(search) ? Mutator(query, search) : query;
        }
    }
}
