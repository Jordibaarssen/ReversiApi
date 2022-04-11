using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ReversiRestApi.DAL
{
    public class SpelAccessLayer : ISpelRepository
    {
        private SpelContext _context;

        public SpelAccessLayer(SpelContext context) { _context = context; }

        public void AddSpel(Spel spel)
        {
            _context.Spellen.Add(spel);
            _context.SaveChanges();
        }

        public List<Spel> GetSpellen()
        {
            return _context.Spellen.ToList();
        }

        public Spel GetSpel(string spelToken)
        {
            return _context.Spellen.FirstOrDefault(spel => spel.Token == spelToken);
        }

        public Spel GetSpelFromPlayer(string id)
        {
            return _context.Spellen.FirstOrDefault(spel => spel.Speler1Token == id || spel.Speler2Token == id);
        }

        public void Delete(Spel spel)
        {
            _context.Spellen.Remove(spel);
        }

        public async Task<string> Save()
        {
            await _context.SaveChangesAsync();
            return "";
        }

    }
}