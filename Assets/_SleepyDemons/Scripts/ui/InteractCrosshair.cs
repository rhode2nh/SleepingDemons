using UnityEngine;
using UnityEngine.UI;

public class InteractCrosshair : MonoBehaviour
{
    private Image _image;
    private Sprite _defaultSprite;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultSprite = _image.sprite;
    }

    private void Start()
    {
        Close();
    }

    public void Open(Sprite sprite)
    {
        _image.sprite = sprite == null ? _defaultSprite : sprite;
        gameObject.SetActive(true);
    }
    
    public void Close()
    {
        _image.sprite = _defaultSprite;
        gameObject.SetActive(false);
    }
}
