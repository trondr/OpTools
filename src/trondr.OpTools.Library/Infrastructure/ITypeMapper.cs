namespace trondr.OpTools.Library.Infrastructure
{
    public interface ITypeMapper
    {
        T Map<T>(object source);
    }
}
