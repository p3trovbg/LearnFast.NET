namespace LearnFast.Services.Data.DifficultyService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LearnFast.Data.Models.Enums;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class DifficultyService : IDifficultyService
    {
        public Dictionary<int, string> GetAll()
        {
            return Enum.GetValues(typeof(Difficulty))
                .Cast<Difficulty>()
                .ToDictionary(t => (int)t, t => t.ToString());
        }

        public IEnumerable<SelectListItem> GetDifficultyList()
        {
            return this.GetAll()
                .Select(keyValuePair => new SelectListItem()
                {
                    Text = keyValuePair.Value.ToString(),
                    Value = keyValuePair.Key.ToString(),
                });
        }
    }
}
