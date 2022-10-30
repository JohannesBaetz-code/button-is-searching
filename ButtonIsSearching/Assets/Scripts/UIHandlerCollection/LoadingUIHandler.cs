using UnityEngine;

namespace UIHandlerCollection
{
    public class LoadingUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingUICanvas;

        public void CloseLoadingUI()
        {
            _loadingUICanvas.SetActive(false);
        }
    }
}