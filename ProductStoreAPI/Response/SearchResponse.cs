namespace ProductStoreAPI.Response
{
    public abstract class SearchResponse<T>
    {
        public virtual int TimeTaken { get; set; }
        public virtual bool TimedOut { get; set; }
    }
}
