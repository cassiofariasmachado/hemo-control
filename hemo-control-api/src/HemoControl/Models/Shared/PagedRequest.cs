namespace HemoControl.Models.Shared
{
    public abstract class PagedRequest
    {
        public int Limit { get; set; } = 20;
        public int Offset { get; set; } = 0;
    }
}