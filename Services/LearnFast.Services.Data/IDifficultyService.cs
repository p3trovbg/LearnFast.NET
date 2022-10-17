using LearnFast.Data.Models.Enums;
using System.Collections.Generic;

namespace LearnFast.Services.Data
{
    public interface IDifficultyService
    {
        Dictionary<int, string> GetAll();
    }
}
