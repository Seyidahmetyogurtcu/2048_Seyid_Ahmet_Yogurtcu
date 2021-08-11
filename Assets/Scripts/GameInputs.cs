using UnityEngine;

namespace SAY2048.Inputs
{
    public class GameInputs : MonoBehaviour //,IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        bool isDraging, isShifted;
        Vector2 hexTouchStart, hexTouchEnd;
        public float inputX;
        public float inputY;
        public static GameInputs singleton;
        void Awake()
        {
            singleton = this;
        }

        void Update()
        {
            inputX = Input.GetAxisRaw("Horizontal");
            inputY = Input.GetAxisRaw("Vertical");
        }

        //not ready yet
        #region Android Controller
        //#if UNITY_ANDROID
        //public void OnBeginDrag(PointerEventData pointerEventData)
        //{
        //    isDraging = true;
        //    hexTouchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //}

        //public void OnDrag(PointerEventData eventData)
        //{
        //    isDraging = true;
        //}

        //public void OnEndDrag(PointerEventData eventData)
        //{
        //    isDraging = false;
        //    hexTouchEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    isShifted = false;
        //}
        //private void FixedUpdate()
        //{
        //    Vector3 inputPosition = Input.mousePosition;
        //    Vector3 touchPositionWorld = Camera.main.ScreenToWorldPoint(inputPosition);
        //}
        //#endif
        #endregion
    }

}
