using Space.Model.Enums;

namespace Space.View.Services.Interfaces
{
    public interface IWindowService
    {
        void ShowWindow(WindowType type);
        void CloseWindow(WindowType type);
        void LockWindow(WindowType type);
    }
}
