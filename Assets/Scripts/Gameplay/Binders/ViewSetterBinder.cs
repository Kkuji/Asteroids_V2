using System;
using MVVM;
using UniRx;

namespace SampleGame.UI
{
    public sealed class ViewSetterBinder<T> : IBinder, IObserver<T>
    {
        private readonly Action<T> view;
        private readonly IReadOnlyReactiveProperty<T> property;
        private IDisposable _handle;

        public ViewSetterBinder(Action<T> view, IReadOnlyReactiveProperty<T> property)
        {
            this.view = view;
            this.property = property;
        }

        public void Bind()
        {
            this.OnNext(this.property.Value);
            _handle = this.property.Subscribe(this);
        }

        public void Unbind()
        {
            _handle?.Dispose();
            _handle = null;
        }

        public void OnNext(T value)
        {
            this.view.Invoke(value);
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}