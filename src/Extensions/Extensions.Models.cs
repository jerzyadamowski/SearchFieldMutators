using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SFM.Model;

namespace SFM.Extensions
{
    /// <summary>
    /// https://gist.github.com/Grinderofl/2767155
    /// </summary>
    /// <example>
    /// <![CDATA[
    ///        IRepository<User> userRepository = unitOfWork.GetRepository<User>();
    ///        SearchFieldVM model = new SearchFieldVM()
    ///        {
    ///            AgencyId = 3,
    ///            DepartmentId = 2
    ///        };
    ///
    ///        var searchRules = new SearchFieldMutators<SearchFieldMutatorsTest.SearchFieldVM, User>();
    ///        searchRules.Add(
    ///            x => x.AgencyId.HasValue,
    ///            (query, search) =>
    ///                query.Where(
    ///                    p =>
    ///                        p.Accesses.Any(s => s.AgencyId == search.AgencyId) ||
    ///                        p.Accesses.All(s => s.AgencyId == null)));
    ///        searchRules.Add(
    ///            x => x.DepartmentId.HasValue,
    ///            (query, search) =>
    ///                query.Where(p =>
    ///                    p.Accesses.Any(x => x.DepartmentId == search.DepartmentId) ||
    ///                    p.Accesses.All(x => x.DepartmentId == null)));
    ///
    ///        SearchFieldVM modelOther = new SearchFieldVM()
    ///        {
    ///            AgencyId = 1,
    ///            DepartmentId = 2
    ///        };
    ///    
    ///        var collection = userRepository.GetQuery();
    ///        collection = collection.FilterMutator<SearchFieldVM, User>(modelOther, searchRules);
    /// ]]>
    /// </example>
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
