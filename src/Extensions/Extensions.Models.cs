using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SFM.Model;

namespace SFM.Extensions
{
    public static partial class Extensions
    {
        public static IQueryable<TQuery> FilterMutator<TQuery, TSearch>(
            this IQueryable<TQuery> collection,
            SearchFieldMutators<TQuery, TSearch> searchRules,
            TSearch modelSearch) 
        {
            return searchRules.Aggregate(collection, (current, mutator) => mutator.Apply(current, modelSearch));
        }

        public static IQueryable<TQuery> FilterMutatorPager<TQuery, TSearch>(
            this IQueryable<TQuery> collection,
            SearchFieldMutators<TQuery, TSearch> searchRules,
            TSearch modelSearch) where TSearch : IFilterMutatorPager
        {
            return searchRules.Aggregate(collection, (current, mutator) => mutator.Apply(current, modelSearch)).Skip(modelSearch.ItemsToSkip).Take(modelSearch.ItemsPerPage);
        }
    }
}
