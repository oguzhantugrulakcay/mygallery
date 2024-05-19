namespace mygallery.Data
{
    public class PageableData<T>
{
    public int count { get; set; }

    public List<T> data { get; set; }
}
}
