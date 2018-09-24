using EcsRx.Components;
using EcsRx.Entities;

namespace EcsRx.Views.Components
{
    public class ViewComponent : IComponent
    {
        public bool DestroyWithView { get; set; }
        public object View { get; set; }
        public IEntity Entity { get; set ; }
    }
}