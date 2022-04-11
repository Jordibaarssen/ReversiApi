using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Controllers
{
    [Route("api/Spel")]
    [ApiController]
    public class SpelController : ControllerBase
    {
        private readonly ISpelRepository iRepository;

        public SpelController(ISpelRepository repository)
        {
            iRepository = repository;
        }


        // GET api/spel
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetSpellen()
        {
            return Ok(iRepository.GetSpellen().Where(spel => spel.Speler2Token == null));
        }

        [HttpPost]
        public ActionResult<Spel> CreateSpel([FromForm] string spelerToken, [FromForm] string omschrijving)
        {
            Spel spel = new Spel();
            spel.Token = Guid.NewGuid().ToString();
            spel.Speler1Token = spelerToken;
            spel.Omschrijving = omschrijving;
            iRepository.AddSpel(spel);
            return Ok(spel);
        }

        [HttpGet("/api/Spel/{id}")]
        public ActionResult<Spel> GetSpel(string id)
        {
            Spel spel = iRepository.GetSpel(id);
            if (spel == null) return NotFound();
            return Ok(spel);
        }

        [HttpGet("/api/SpelSpeler/{id}")]
        public ActionResult<Spel> GetSpelVanSpeler(string id)
        {
            Spel spel = iRepository.GetSpelFromPlayer(id);
            if (spel == null) return NotFound();

            return Ok(spel);
        }

        // GET: api/Reversi/Beurt/{token}
        [HttpGet("/api/Spel/Beurt/{token}")]
        public ActionResult<Kleur> GetBeurt(string token)
        {
            return Ok(iRepository.GetSpel(token).AandeBeurt);  
        }

        [HttpPost("/api/Spel/{id}/join")]
        public async Task<ActionResult<Spel>> JoinSpel(string id, [FromForm] string spelerToken)
        {
            if (spelerToken == null) return Unauthorized();

            Spel spel = iRepository.GetSpel(id);
            if (spel == null) return NotFound();

            if (spel.Speler2Token != null) return BadRequest("Geen ruimte in dit spel");
            spel.Speler2Token = spelerToken;

            await iRepository.Save();

            return Ok(spel);
        }

        [HttpPost("/api/Spel/{id}/zet")]
        public async Task<ActionResult<Spel>> DoeZet(string id, [FromForm] string token, [FromForm] int x, [FromForm] int y)
        {
            if (token == null) return Unauthorized();

            Spel spel = iRepository.GetSpel(id);
            if (spel == null) return NotFound();

            if ((spel.AandeBeurt == Kleur.Wit ? spel.Speler1Token : spel.Speler2Token) != token) return Unauthorized("Niet je beurt");
            if (!spel.DoeZet(x, y)) return BadRequest("Ongeldige zet");

            await iRepository.Save();

            return Ok(spel);
        }

        [HttpDelete("/api/spel/{id}/remove")]
        public ActionResult<Spel> DeleteSpel(string id)
        {
            Spel spel = iRepository.GetSpel(id);
            if (spel == null) return NotFound();

            iRepository.Delete(spel);
            iRepository.Save();

            return Ok(spel);
        }

    }
}
