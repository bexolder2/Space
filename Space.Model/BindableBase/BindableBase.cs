using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Space.Model.BindableBase
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public virtual bool Set<T>(ref T field, T value, [CallerMemberName]string propName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propName);
            return true;
        }
    }
}
