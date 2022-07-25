using Microsoft.Xrm.Sdk;

namespace Xrm.Application.Helpers
{
    public static class EntityExtensions
    {
        public static Entity Clone(this Entity entity)
        {
            Entity newEntity = new Entity(entity.LogicalName);

            foreach (string key in entity.Attributes.Keys)
            {
                newEntity[key] = entity[key];
            }

            return newEntity;
        }
    }
}
