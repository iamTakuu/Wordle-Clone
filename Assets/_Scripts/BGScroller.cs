using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class BGScroller : MonoBehaviour
    {
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private float _x, _y;
        
        void Update()
        {
            _rawImage.uvRect = new Rect(_rawImage.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _rawImage.uvRect.size);
        }
        
        
    }
}
