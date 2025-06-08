using System;
using MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SampleGame.UI
{
    public sealed class ImageBinder : IBinder, IObserver<Sprite>
    {
        private readonly Image view;
        private readonly IReadOnlyReactiveProperty<Sprite> property;
        private IDisposable _handle;

        public ImageBinder(Image view, IReadOnlyReactiveProperty<Sprite> property)
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

        public void OnNext(Sprite value)
        {
            this.view.sprite = value;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}