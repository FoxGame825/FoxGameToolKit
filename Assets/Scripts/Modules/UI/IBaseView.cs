using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxGame.UI
{
    public interface IBaseView
    {
        void Init();

        void OnShowView();
        void OnShowViewComplate();

        void OnHideView();
        void OnHideViewComplate();

        void OnStartShowViewAnimation();
        void OnEndShowViewAnimation();

        void OnStartHideViewAnimation();
        void OnEndHideViewAnimation();

        void CanOperation();
        void CanNotOperation();
        
    }
}
