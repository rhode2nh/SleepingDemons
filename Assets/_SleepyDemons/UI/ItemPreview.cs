using UnityEngine;

public class ItemPreview : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _offsetAngle;
    [SerializeField] private GameObject _defaultPreviewItem;

    private GameObject _currentItem;
    private ParentObject _spawn;

    private void Awake()
    {
        _spawn = GetComponentInChildren<ParentObject>();
    }

    private void Update()
    {
        _spawn.transform.Rotate(-Vector3.up * (_rotateSpeed * Time.deltaTime));
    }

    public void Open(GameObject item)
    {
        if (_currentItem != null)
        {
            Destroy(_currentItem);
        }

        _currentItem = Instantiate(item == null ? _defaultPreviewItem : item, _spawn.transform, false);
        _currentItem.transform.rotation = _spawn.transform.rotation;
        var eulerAngles = _currentItem.transform.eulerAngles;
        _currentItem.transform.eulerAngles = new Vector3(_offsetAngle, eulerAngles.y, eulerAngles.z);
    }

    public void Close()
    {
        Destroy(_currentItem);
    }
}