namespace ShopBackend.Discoverabillity
//Based on https://code-maze.com/hateoas-aspnet-core-web-api/#comments
{
    public class LinkResourceBase
    {
        public LinkResourceBase()
        {
        }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
