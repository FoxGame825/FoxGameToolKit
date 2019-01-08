using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


namespace FoxGame.Utils {

    public class UIEvents : MonoBehaviour,
                                    IPointerClickHandler,
                                    IPointerDownHandler,
                                    IPointerEnterHandler,
                                    IPointerExitHandler,
                                    IPointerUpHandler,
                                    ISelectHandler,
                                    IUpdateSelectedHandler,
                                    IDeselectHandler,
                                    IBeginDragHandler,
                                    IDragHandler,
                                    IEndDragHandler,
                                    IDropHandler,
                                    IScrollHandler,
                                    IMoveHandler {
        public delegate void VoidDelegate(GameObject go);
        public delegate void DragDelegate(GameObject go, PointerEventData eventData);
        public VoidDelegate onClick;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;
        public VoidDelegate onDeSelect;
        public VoidDelegate onDrop;
        public VoidDelegate onScroll;
        public VoidDelegate onMove;

        public DragDelegate onDown;
        public DragDelegate onUp;
        public DragDelegate onDrag;
        public DragDelegate onDragBegin;
        public DragDelegate onDragEnd;


        public object parameter;

        public void OnPointerClick(PointerEventData eventData) { if (onClick != null) onClick(gameObject); }
        public void OnPointerDown(PointerEventData eventData) { if (onDown != null) onDown(gameObject, eventData); }
        public void OnPointerEnter(PointerEventData eventData) { if (onEnter != null) onEnter(gameObject); }
        public void OnPointerExit(PointerEventData eventData) { if (onExit != null) onExit(gameObject); }
        public void OnPointerUp(PointerEventData eventData) { if (onUp != null) onUp(gameObject, eventData); }
        public void OnSelect(BaseEventData eventData) { if (onSelect != null) onSelect(gameObject); }
        public void OnUpdateSelected(BaseEventData eventData) { if (onUpdateSelect != null) onUpdateSelect(gameObject); }
        public void OnDeselect(BaseEventData eventData) { if (onDeSelect != null) onDeSelect(gameObject); }
        public void OnDrag(PointerEventData eventData) { if (onDrag != null) onDrag(gameObject, eventData); }
        public void OnBeginDrag(PointerEventData eventData) { if (onDragBegin != null) onDragBegin(gameObject, eventData); }
        public void OnEndDrag(PointerEventData eventData) { if (onDragEnd != null) onDragEnd(gameObject, eventData); }
        public void OnDrop(PointerEventData eventData) { if (onDrop != null) onDrop(gameObject); }
        public void OnScroll(PointerEventData eventData) { if (onScroll != null) onScroll(gameObject); }
        public void OnMove(AxisEventData eventData) { if (onMove != null) onMove(gameObject); }

        static public UIEvents Listen(GameObject go) {
            UIEvents listener = go.GetComponent<UIEvents>();
            if (listener == null) listener = go.AddComponent<UIEvents>();
            return listener;
        }
        static public void RemoveListen(GameObject go) {
            UIEvents listener = go.GetComponent<UIEvents>();
            if (listener != null) Destroy(listener);
        }
    }
}