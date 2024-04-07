namespace Codebase.Library.SAD
{
    public class DefaultEntityComponents : EntityComponents
    {
        public override EntityComponents Declare(Entity abstractEntity) => this;
    }
}