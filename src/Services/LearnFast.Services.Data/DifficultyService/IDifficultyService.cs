namespace LearnFast.Services.Data.DifficultyService
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IDifficultyService
    {
        Dictionary<int, string> GetAll();

        IEnumerable<SelectListItem> GetDifficultyList();
    }
}
