using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using VideoGameApi.Data;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController(VideoGameDbContext context) : ControllerBase
    {

        private readonly VideoGameDbContext _context = context;



        [HttpGet]
        public async Task<ActionResult<List<VideoGame>>> GetVideoGames()
        {
            return StatusCode(StatusCodes.Status200OK, await _context.VideoGames.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoGame>> GetVideoGameByID(int id)
        {
            var game = await _context.VideoGames.FindAsync(id);

            if (game is null)
                return StatusCode(StatusCodes.Status404NotFound);

            return StatusCode(StatusCodes.Status200OK, game);
        
        }

        [HttpPost]
        public async Task<ActionResult<VideoGame>> AddVideoGame(VideoGame newVideoGame)
        {
            if(newVideoGame is null)
                return StatusCode(StatusCodes.Status400BadRequest);

            _context.VideoGames.Add(newVideoGame);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, CreatedAtAction(nameof(GetVideoGameByID),new { id = newVideoGame.Id },newVideoGame));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideoGame(int id, VideoGame updatedVideoGame)
        {
            var game = await _context.VideoGames.FindAsync(id);

            if (game is null)
                return StatusCode(StatusCodes.Status404NotFound);

            game.Title = updatedVideoGame.Title;
            game.Platform = updatedVideoGame.Platform;
            game.Developer = updatedVideoGame.Developer;
            game.Publisher = updatedVideoGame.Publisher;

            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoGame(int id) {

            var game = await _context.VideoGames.FindAsync(id);

            if (game is null)
                return StatusCode(StatusCodes.Status404NotFound);

            _context.VideoGames.Remove(game);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);


        
        }
    }
}
