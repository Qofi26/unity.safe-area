using UnityEngine;
using UnityEngine.UI;

namespace Erem.SafeArea
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaController : MonoBehaviour
    {
        private bool _isNestedController;
        private RectTransform _rectTransform;
        private Rect _lastSafeArea = Rect.zero;

        private void Awake()
        {
            _rectTransform = (RectTransform) transform;
        }

        private void OnEnable()
        {
            var parent = transform.parent;
            if (parent == null)
            {
                _isNestedController = false;
                return;
            }

            var parentController = parent.GetComponentInParent<SafeAreaController>();
            _isNestedController = parentController != null && parentController.enabled;
        }

        private void LateUpdate()
        {
            if (_isNestedController)
            {
                return;
            }

            var safeArea = Screen.safeArea;
            if (_lastSafeArea == safeArea)
            {
                return;
            }

            Refresh(safeArea);
        }

        private void Refresh(Rect safeArea)
        {
            _lastSafeArea = safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }
    }
}
