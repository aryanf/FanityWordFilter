using ProfanityFilter.Interface;
using ProfanityFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfanityFilter.Core
{
    public class ProfanityEngine
    {
        private readonly char[] delimiters = new char[] { ',', '.', ' ', '?', '!', '\n', '\r' };
        private string exceptionMessage1 = "Word cannot consist of delimiter (',', '.', ' ', '?', '!', '\n', '\r' ).";
        private readonly IProfanityWordRepository profanityWordRepository;
        private readonly IProfanityLogger profanityLogger;
        public ProfanityEngine(IProfanityWordRepository profanityWordRepository, IProfanityLogger profanityLogger)
        {
            this.profanityWordRepository = profanityWordRepository;
            this.profanityLogger = profanityLogger;
        }

        public async Task<ProfanityResult> RunAsync(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ProfanityException("Empty input!", ProfanityErrorOperation.ProfanityRun);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ProfanityResult profanityResult = new ProfanityResult()
            {
                Text = content,
                IsProfane = false,
                NumberOfProfaneWords = 0,
                ProcessTimeInMilliseconds = 0,
                ProfaneWords = new List<string>()
            };
            List<string> profaneWords = await profanityWordRepository.ListAsync().ConfigureAwait(false);
            List<string> splittedContent = content.ToLower().Split(delimiters).Select(p => p.Trim()).ToList();
            foreach (string profaneWord in profaneWords)
            {
                if (splittedContent.Any(p => p == profaneWord))
                {
                    profanityResult.IsProfane = true;
                    profanityResult.NumberOfProfaneWords++;
                    profanityResult.ProfaneWords.Add(profaneWord);
                }
            }
            watch.Stop();
            profanityResult.ProcessTimeInMilliseconds = watch.ElapsedMilliseconds;
            return profanityResult;
        }

        public async Task<List<string>> ListWordsAsync()
        {
            return await profanityWordRepository.ListAsync().ConfigureAwait(false);
        }

        public async Task AddWordAsync(string profanityWord)
        {
            if (delimiters.Any(d => profanityWord.Contains(d)))
            {
                throw new ProfanityException(exceptionMessage1, ProfanityErrorOperation.ProfanityWordList);
            }
            await profanityWordRepository.AddAsync(profanityWord.ToLower()).ConfigureAwait(false);
        }

        public async Task RemoveWordAsync(string profanityWord)
        {
            if (delimiters.Any(d => profanityWord.Contains(d)))
            {
                throw new ProfanityException(exceptionMessage1, ProfanityErrorOperation.ProfanityWordList);
            }
            await profanityWordRepository.RemoveAsync(profanityWord.ToLower()).ConfigureAwait(false);
        }
    }
}
