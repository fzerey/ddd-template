using Fzerey.DDDStarter.Domain.Model;

namespace Fzerey.DDDStarter.Tests.Fakes
{
    internal static class EntityExtensions
    {
        public static T WithId<T>(this T entity, int id) where T : Entity
        {
            typeof(Entity).GetProperty(nameof(Entity.Id))!.SetValue(entity, id);
            return entity;
        }
    }
}
