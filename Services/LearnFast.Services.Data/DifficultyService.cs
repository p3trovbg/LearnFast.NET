namespace LearnFast.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LearnFast.Data.Models.Enums;

    public class DifficultyService : IDifficultyService
    {
        public Dictionary<int, string> GetAll()
        {
            return Enum.GetValues(typeof(Difficulty))
                .Cast<Difficulty>()
                .ToDictionary(t => (int)t, t => t.ToString());
        }
    }
}
