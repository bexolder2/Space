using System.Collections.Generic;

namespace Space.Helpers.Interfaces
{
    public interface IGenerator<T>
    {
        T Generate(List<T> args = null);
    }
}
