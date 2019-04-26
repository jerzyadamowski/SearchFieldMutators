using System;
using System.Linq;
using Xunit;
using SFM;
using SFM.Extensions;
using SFM.Model;
using System.Collections.Generic;

namespace SFM.tests
{
    public class SearchFieldMutatorsTest
    {
        public SearchFieldMutatorsTest()
        {
            Init();
        }

        [Fact]
        public void Test1()
        {
            var searchRules = new SearchFieldMutators<User, SearchFieldVM>();
            searchRules.Add(
                test => !string.IsNullOrWhiteSpace(test.InputText),
                (query, search) => query.Where(x => x.Name.Contains(search.InputText)));

            searchRules.Add(
                test => test.MinAge > 0,
                (query, search) => query.Where(x => x.Age >= search.MinAge));

            searchRules.Add(
                test => test.MaxAge > 0,
                (query, search) => query.Where(x => x.Age <= search.MaxAge));

            SearchFieldVM modelSearch = new SearchFieldVM()
            {
                InputText = "J",
                MinAge = 17,
                MaxAge = 19
            };

            var collection = userRepository.AsQueryable();
            collection = collection.FilterMutator<User, SearchFieldVM>(searchRules, modelSearch);
            Assert.Equal(1, collection.Count());
        }

        [Fact]
        public void Test2()
        {
            var searchRules = new SearchFieldMutators<User, SearchFieldVM>();
            searchRules.Add(
                test => !string.IsNullOrWhiteSpace(test.InputText),
                (query, search) => query.Where(x => x.Name.Contains(search.InputText)));

            searchRules.Add(
                test => test.MinAge > 0,
                (query, search) => query.Where(x => x.Age >= search.MinAge));

            searchRules.Add(
                test => test.MaxAge > 0,
                (query, search) => query.Where(x => x.Age <= search.MaxAge));

            SearchFieldVM modelSearch = new SearchFieldVM()
            {
                InputText = null,
                MinAge = 17,
                MaxAge = 19
            };

            var collection = userRepository.AsQueryable();
            collection = collection.FilterMutator<User, SearchFieldVM>(searchRules, modelSearch);
            Assert.Equal(2, collection.Count());
        }

        private void Init()
        {

        }

        private List<User> userRepository = new List<User>()
        {
            new User()
            {
                Name = "Joe",
                Age = 18,
                Department = "HR"
            },
            new User()
            {
                Name = "Jay",
                Age = 21,
                Department = "Administration"
            },
            new User()
            {
                Name = "Nicole",
                Age = 18,
                Department = "Finance"
            },
        };
    }

    public class User
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public int Age { get; set; }
    }

    public class SearchFieldVM
    {
        public string InputText { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
    }
}