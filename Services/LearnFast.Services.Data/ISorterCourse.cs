namespace LearnFast.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISorterCourse
    {
        Task<IEnumerable<T>> GetAllOrderByPrice<T>();

        Task<IEnumerable<T>> GetAllOrderByDescendingPrice<T>();

        Task<IEnumerable<T>> GetAllBySells<T>();
    }
}
