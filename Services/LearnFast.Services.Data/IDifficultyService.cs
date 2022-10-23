namespace LearnFast.Services.Data
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IDifficultyService
    {
        Dictionary<int, string> GetAll();

        IEnumerable<SelectListItem> GetDifficultyList();
    }
}
