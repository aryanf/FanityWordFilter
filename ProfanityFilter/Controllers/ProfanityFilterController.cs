using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProfanityFilter.Core;
using ProfanityFilter.Interface;
using ProfanityFilter.Models;

namespace ProfanityFilter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfanityFilterController : ControllerBase
    {
        private readonly ProfanityEngine engine;
        private readonly IProfanityLogger profanityLogger;

        public ProfanityFilterController()
        {
            profanityLogger = new ConsoleProfanityLogger();
            IProfanityWordRepository profanityWordRepository = new TextFileProfanityWordRepository();

            engine = new ProfanityEngine(profanityWordRepository, profanityLogger);
        }

        [HttpPost]
        public async Task<IActionResult> Run()
        {
            string body = string.Empty;

            try
            {
                using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
                {
                    body = await stream.ReadToEndAsync().ConfigureAwait(false);
                }
                ProfanityResult result = await engine.RunAsync(body).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception exception)
            {
                profanityLogger.Log($"{nameof(Run)} | {body} | {exception.Message}");
                return BadRequest(new { Error = exception.Message });
            }
        }

        [HttpGet("listwords")]
        public async Task<IActionResult> ListWords()
        {
            try
            {
                List<string> words = await engine.ListWordsAsync().ConfigureAwait(false);
                return Ok(words);
            }
            catch (Exception exception)
            {
                profanityLogger.Log($"{nameof(ListWords)} | {exception.Message}");
                return BadRequest(new { Error = exception.Message });
            }
        }

        [HttpPost("addword")]
        public async Task<IActionResult> AddWord()
        {
            string body = string.Empty;

            try
            {
                using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
                {
                    body = await stream.ReadToEndAsync().ConfigureAwait(false);
                }
                await engine.AddWordAsync(body).ConfigureAwait(false);
                List<string> words = await engine.ListWordsAsync().ConfigureAwait(false);
                return Ok(words);
            }
            catch (Exception exception)
            {
                profanityLogger.Log($"{nameof(AddWord)} | {body} | {exception.Message}");
                return BadRequest(new { Error = exception.Message });
            }
        }

        [HttpPost("removeword")]
        public async Task<IActionResult> RemoveWord()
        {
            string body = string.Empty;

            try
            {
                using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
                {
                    body = await stream.ReadToEndAsync().ConfigureAwait(false);
                }
                await engine.RemoveWordAsync(body).ConfigureAwait(false);
                List<string> words = await engine.ListWordsAsync().ConfigureAwait(false);
                return Ok(words);
            }
            catch (Exception exception)
            {
                profanityLogger.Log($"{nameof(RemoveWord)} | {body} | {exception.Message}");
                return BadRequest(new { Error = exception.Message });
            }
        }
    }
}
