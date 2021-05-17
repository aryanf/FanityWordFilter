using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfanityFilter.Interface
{
    public interface IProfanityWordRepository
    {
        Task<List<string>> ListAsync();

        Task AddAsync(string word);

        Task RemoveAsync(string word);
    }
}
