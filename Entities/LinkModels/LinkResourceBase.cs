namespace Entities.LinkModels;

public abstract class LinkResourceBase
{
    public LinkResourceBase()
    {

    }

    public List<Link> Links { get; set; } = new List<Link>();
}