using DotNetBack.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DotNetBack.Controllers
{
    [ApiController]
    [Route("api/fifteen-puzzle")]
    public class FifteenPuzzleController : ControllerBase
    {
        private readonly FifteenPuzzleDbContext _dbContext;

        public FifteenPuzzleController(FifteenPuzzleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("best")]
        public BestResults GetBestResults()
        {
            var bestTimeResult = _dbContext.UserGameResults.AsEnumerable().MinBy(ur => ur.Time);
            var bestMovesResult = _dbContext.UserGameResults.AsEnumerable().MinBy(ur => ur.MovesCount);

            return new BestResults()
            {
                BestTime = bestTimeResult?.Time,
                BestTimeUser = bestTimeResult?.Nickname,
                BestMovesCount = bestMovesResult?.MovesCount,
                BestMoveCountUser = bestMovesResult?.Nickname
            };
        }

        [HttpPost("new-record")]
        public void PutRecord([FromBody] UserGameResult newUserGameResult)
        {
            var userGameResult = _dbContext.UserGameResults.Find(newUserGameResult.Nickname);
            if (userGameResult != null)
            {
                userGameResult.CopyNonKeyDataFrom(newUserGameResult);
                _dbContext.UserGameResults.Update(userGameResult);
            }
            else
            {
                _dbContext.UserGameResults.Add(newUserGameResult);
            }
            _dbContext.SaveChanges();
        }
    }
}