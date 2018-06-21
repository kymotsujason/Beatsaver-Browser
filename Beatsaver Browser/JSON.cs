using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beatsaver_Browser
{
    public class DifficultyLevel
    {
        public string difficulty { get; set; }
        public int difficultyRank { get; set; }
        public string audioPath { get; set; }
        public string jsonPath { get; set; }
        public int offset { get; set; }
    }

    public class RootObject
    {
        public string id { get; set; }
        public string beatname { get; set; }
        public string ownerid { get; set; }
        public string downloads { get; set; }
        public string upvotes { get; set; }
        public string plays { get; set; }
        public string beattext { get; set; }
        public string uploadtime { get; set; }
        public string songName { get; set; }
        public string songSubName { get; set; }
        public string authorName { get; set; }
        public string beatsPerMinute { get; set; }
        public List<DifficultyLevel> difficultyLevels { get; set; }
        public string img { get; set; }
        public string uploader { get; set; }
    }
}
