using ProfanityFilter.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProfanityFilterTest.Mocks
{
    public class MockProfanityWordRepository : IProfanityWordRepository
    {
        private List<string> profaneWords;

        public MockProfanityWordRepository(List<string> initialList)
        {
            profaneWords = initialList;
        }
        public async Task AddAsync(string word)
        {
            profaneWords.Add(word);
        }

        public async Task<List<string>> ListAsync()
        {
            return profaneWords;
        }

        public async Task RemoveAsync(string word)
        {
            profaneWords.Remove(word);
        }
    }
}
