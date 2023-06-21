using System.ComponentModel.DataAnnotations;

namespace DotNetBack
{
    public class UserGameResult
    {
        [Key]
        public string Nickname { get; set; } = "";
        public int Time { get; set; }
        public int MovesCount { get; set; }

        public void CopyNonKeyDataFrom(UserGameResult userGameResult)
        {
            Time = userGameResult.Time;
            MovesCount = userGameResult.MovesCount;
        }
    }
}