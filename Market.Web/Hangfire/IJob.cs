namespace Market.Web.Hangfire
{
    interface IJob
    {
        
        void Do(int itemId);
    }
}
