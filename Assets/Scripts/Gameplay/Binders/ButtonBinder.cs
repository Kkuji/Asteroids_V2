using System;
using MVVM;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SampleGame.UI
{
    public sealed class ButtonBinder : IBinder
    {
        private readonly Button view;
        private readonly UnityAction modelAction;

        public ButtonBinder(Button view, Action model)
        {
            this.view = view;
            this.modelAction = new UnityAction(model);
        }

        void IBinder.Bind()
        {
            this.view.onClick.AddListener(this.modelAction);
        }

        void IBinder.Unbind()
        {
            this.view.onClick.RemoveListener(this.modelAction);
        }
    }
}