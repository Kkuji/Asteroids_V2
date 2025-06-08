using System;
using MVVM;
using TMPro;
using UniRx;
using UnityEngine;

namespace Binders
{
    public class TextBinder : IBinder, IObserver<string>
    {
        private readonly TMP_Text view;
        private readonly IReadOnlyReactiveProperty<string> property;
        private IDisposable _handle;

        public TextBinder(TMP_Text view, IReadOnlyReactiveProperty<string> property)
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

        public void OnNext(string value)
        {
            this.view.text = value;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}