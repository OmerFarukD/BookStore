using Entities.Models;

namespace Entities.LinkModels;

public class LinkResponse
{
    public bool Haslinks { get; set; }
    public List<Entity>  ShappedEntities { get; set; }
    public LinkCollectionWrapper<Entity> LinkedEntities { get; set; }

    public LinkResponse()
    {
        ShappedEntities = new List<Entity>();
        LinkedEntities = new LinkCollectionWrapper<Entity>();
    }
}