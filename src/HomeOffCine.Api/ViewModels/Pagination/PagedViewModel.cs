namespace HomeOffCine.Api.ViewModels.Pagination;

public class PagedViewModel<T> : IPagedList where T : class
{
    public string ReferenceAction { get; set; }
    public List<T> List { get; set; } = new List<T>();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string Query { get; set; }
    public int TotalResults { get; set; }
    public double TotalPages => Math.Ceiling((double)TotalResults / PageSize);
}
public interface IPagedList
{
    public string ReferenceAction { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string Query { get; set; }
    public int TotalResults { get; set; }
    public double TotalPages { get; }
}
