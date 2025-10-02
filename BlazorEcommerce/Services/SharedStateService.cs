namespace BlazorEcommerce.Services
{
    public class SharedStateService
    {
        // This service is used to share state between components
        // It is registered as a singleton in Program.cs
        // It is used to notify components when the shopping cart is updated
        public event Action? OnChange;
        private int _cartItemsCount;

        public int CartItemsCount
        {
            get => _cartItemsCount;
            set
            {
                _cartItemsCount = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
