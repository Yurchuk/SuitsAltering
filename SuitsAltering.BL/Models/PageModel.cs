using SuitsAltering.BL.Abstractions;

namespace SuitsAltering.BL.Models;

public class PageModel<T>
{
    public int TotalItems { get; set; }

    public IEnumerable<T> Items { get; set; }
}