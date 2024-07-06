using System.Linq.Expressions;

namespace Core.Specification
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Creterial { get; }
        List<Expression<Func<T, object>>> Includes { get; }
    }
}