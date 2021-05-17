using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProfanityFilter.Core;
using ProfanityFilter.Interface;
using ProfanityFilter.Models;
using ProfanityFilterTest.Mocks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfanityFilterTest
{
    [TestClass]
    public class ProfanityEngineTests
    {
        private ProfanityEngine profanityEngine;

        [TestInitialize]
        public void TestInitializer()
        {
            List<string> initialList = new List<string>() { "foo", "bar", "baz", "qux" };
            IProfanityWordRepository profanityWordRepository = new MockProfanityWordRepository(initialList);
            IProfanityLogger profanityLogger = new MockLogger();
            profanityEngine = new ProfanityEngine(profanityWordRepository, profanityLogger);
        }

        [TestMethod]
        public async Task ProfanityEngineTest_NoProfaneWord()
        {
            // assign
            string content = "This content doesn't have any profane word";

            // act
            ProfanityResult result = await profanityEngine.RunAsync(content).ConfigureAwait(false);

            // assert
            Assert.IsFalse(result.IsProfane, "Unexpected profanity flag");
            Assert.AreEqual(0, result.NumberOfProfaneWords, "Unexpected number of profane words");
            Assert.AreEqual(0, result.ProfaneWords.Count, "Unexpected length for profane word list");
        }

        [TestMethod]
        public async Task ProfanityEngineTest_OneProfaneWord()
        {
            // assign
            string content = "This content has a profane word, which is called bar. that is it.";
            List<string> expectedProfaneWords = new List<string>() { "bar" };

            // act
            ProfanityResult result = await profanityEngine.RunAsync(content).ConfigureAwait(false);

            // assert
            Assert.IsTrue(result.IsProfane, "Unexpected profanity flag");
            Assert.AreEqual(1, result.NumberOfProfaneWords, "Unexpected number of profane words");
            Assert.AreEqual(1, result.ProfaneWords.Count, "Unexpected length for profane word list");
            for (int i = 0; i < expectedProfaneWords.Count; i++)
            {
                Assert.AreEqual(expectedProfaneWords[i], result.ProfaneWords[i], "Unexpected profane word");
            }
        }

        [TestMethod]
        public async Task ProfanityEngineTest_AddWrod()
        {
            // assign
            string content = "This content has two profane words, which are called bar and stupid. that is it.";
            List<string> expectedProfaneWords = new List<string>() { "bar", "stupid" };

            // act
            await profanityEngine.AddWordAsync("stupid");
            ProfanityResult result = await profanityEngine.RunAsync(content).ConfigureAwait(false);

            // assert
            Assert.IsTrue(result.IsProfane, "Unexpected profanity flag");
            Assert.AreEqual(2, result.NumberOfProfaneWords, "Unexpected number of profane words");
            Assert.AreEqual(2, result.ProfaneWords.Count, "Unexpected length for profane word list");
            for (int i = 0; i < expectedProfaneWords.Count; i++)
            {
                Assert.AreEqual(expectedProfaneWords[i], result.ProfaneWords[i], "Unexpected profane word");
            }
        }

        [TestMethod]
        public async Task ProfanityEngineTest_RemoveWrod()
        {
            // assign
            string content = "This content has two profane words, which is called foo, and not bar. that is it.";
            List<string> expectedProfaneWords = new List<string>() { "foo" };

            // act
            await profanityEngine.RemoveWordAsync("bar");
            ProfanityResult result = await profanityEngine.RunAsync(content).ConfigureAwait(false);

            // assert
            Assert.IsTrue(result.IsProfane, "Unexpected profanity flag");
            Assert.AreEqual(1, result.NumberOfProfaneWords, "Unexpected number of profane words");
            Assert.AreEqual(1, result.ProfaneWords.Count, "Unexpected length for profane word list");
            for (int i = 0; i < expectedProfaneWords.Count; i++)
            {
                Assert.AreEqual(expectedProfaneWords[i], result.ProfaneWords[i], "Unexpected profane word");
            }
        }

        [TestMethod]
        public async Task ProfanityEngineTest_CapitalLetter()
        {
            // assign
            string content = "This content has a profane word, which is called Bar, and baZ. that is it.";
            List<string> expectedProfaneWords = new List<string>() { "bar", "baz" };

            // act
            ProfanityResult result = await profanityEngine.RunAsync(content).ConfigureAwait(false);

            // assert
            Assert.IsTrue(result.IsProfane, "Unexpected profanity flag");
            Assert.AreEqual(2, result.NumberOfProfaneWords, "Unexpected number of profane words");
            Assert.AreEqual(2, result.ProfaneWords.Count, "Unexpected length for profane word list");
            for (int i = 0; i < expectedProfaneWords.Count; i++)
            {
                Assert.AreEqual(expectedProfaneWords[i], result.ProfaneWords[i], "Unexpected profane word");
            }
        }

        [TestMethod]
        public async Task ProfanityEngineTest_AddWordWithDelimiter()
        {
            // assign
            string word = "Try!this";
            string expectedExceptionMessage = "Word cannot consist of delimiter (',', '.', ' ', '?', '!', '\n', '\r' ).";
            string actualExceptionMessage = string.Empty;

            // act
            try
            {
                await profanityEngine.AddWordAsync(word).ConfigureAwait(false);
            }
            catch (ProfanityException exception)
            {
                actualExceptionMessage = exception.Message;
            }

            // assert
            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage, "Unexpected exception message");
        }


        [TestMethod]
        public async Task ProfanityEngineTest_RemoveWordWithDelimiter()
        {
            // assign
            string word = "Try!this";
            string expectedExceptionMessage = "Word cannot consist of delimiter (',', '.', ' ', '?', '!', '\n', '\r' ).";
            string actualExceptionMessage = string.Empty;

            // act
            try
            {
                await profanityEngine.RemoveWordAsync(word).ConfigureAwait(false);
            }
            catch (ProfanityException exception)
            {
                actualExceptionMessage = exception.Message;
            }

            // assert
            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage, "Unexpected exception message");
        }
    }
}
