namespace LearnFast.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISorterCourse
    {
        Task<IEnumerable<T>> GetAllOrderByPriceAsync<T>();

        Task<IEnumerable<T>> GetAllOrderByDescendingPriceAsync<T>();

        Task<IEnumerable<T>> GetAllBySellsAsync<T>();
    }
}
