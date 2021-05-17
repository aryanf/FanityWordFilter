using ProfanityFilter.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProfanityFilter.Core
{
    public class TextFileProfanityWordRepository : IProfanityWordRepository
    {
        private readonly string path;
        public TextFileProfanityWordRepository()
        {
            path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\profane_words.txt"));
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        public async Task<List<string>> ListAsync()
        {
            string line;
            List<string> words = new List<string>();

            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    words.Add(line);
                }
            }
            return words;
        }

        public async Task AddAsync(string word)
        {
            await File.AppendAllTextAsync(path, word + Environment.NewLine).ConfigureAwait(false);
        }

        public async Task RemoveAsync(string word)
        {
            string tempFile = Path.GetRandomFileName();

            using (var sr = new StreamReader(path))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;

                while ((line = await sr.ReadLineAsync()) != null)
                {
                    if (line != word)
                    {
                        await sw.WriteLineAsync(line).ConfigureAwait(false);
                    }
                }
            }

            File.Delete(path);
            File.Move(tempFile, path);
        }
    }
}
