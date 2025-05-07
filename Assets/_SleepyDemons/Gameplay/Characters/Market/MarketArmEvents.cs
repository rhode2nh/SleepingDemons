using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketArmEvents : MonoBehaviour
{
    [SerializeField] private Animator _marketArmAnimator;
    [SerializeField] private GameObject _handPos;
    [SerializeField] private GameObject _currentWorldItem;
    [SerializeField] private GameObject _offerItem;
    [SerializeField] private Transform _offerItemPos;
    [SerializeField] private float _zOffset;
    [SerializeField] private float _xOffset;
    [SerializeField] private Vector3 _itemOffset;
    [SerializeField] private float _nudgeStrength;
    [SerializeField] private float _spawnFrequency;
    [SerializeField] private GameObject _currency;
    [SerializeField] private float _force;
    [SerializeField] private float _xRandomCurrencyOffset;
    [SerializeField] private Transform _currencySpawnPos;
    [SerializeField] private SpringJoint _sprintJoint;
    [SerializeField] private Rigidbody _doorRb;
    [SerializeField] private Door physicsDoor;
    private Vector3 _armPosition;

    public void SetWorldItem(GameObject worldItem)
    {
        _currentWorldItem = worldItem;
        _armPosition = _currentWorldItem.transform.position;
        _armPosition.z -= _zOffset;
        _armPosition.x -= _xOffset;
        transform.position = _armPosition;
    }

    public void SetLeftArm()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetRightArm()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }

    public void SetArmPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void OnOpenDoor()
    {
        _marketArmAnimator.SetBool("IsOpening", false);
    }

    public void OnOpenDoorComplete()
    {
        _marketArmAnimator.SetBool("IsOpening", false);
        // physicsDoor.Nudge(_nudgeStrength);
    }

    public void GrabItem()
    {
        _currentWorldItem.GetComponent<Rigidbody>().isKinematic = true;
        _currentWorldItem.GetComponent<Collider>().enabled = false;
        _currentWorldItem.GetComponent<Rigidbody>().MovePosition(_handPos.transform.position);
        _currentWorldItem.transform.parent = _handPos.transform;
        _currentWorldItem.transform.localPosition = _itemOffset;
    }

    public void OnGrabComplete()
    {
        _marketArmAnimator.SetBool("IsGrabbing", false);
        if (_currentWorldItem.GetComponent<ICurrency>() != null)
        {
            _marketArmAnimator.SetBool("IsOffering", true);
            SetArmPosition(Vector3.zero + new Vector3(0.0f, 0.0f, -0.2f));
        }
        else
        {
            int numEyesToSpawn = _currentWorldItem.GetComponent<WorldItem>().ItemSO.Cost;
            StartCoroutine(ShootEyes(numEyesToSpawn));
        }
        Destroy(_currentWorldItem);
    }

    public void OnCloseDoor()
    {
        SetArmPosition(Vector3.zero + new Vector3(-0.1f, -0.5f, -0.1f));
    }

    public void OnGrabDoor()
    {
        _sprintJoint.connectedBody = _doorRb;
    }

    public void OnCloseDoorComplete()
    {
        _marketArmAnimator.SetBool("IsClosing", false);
        _sprintJoint.connectedBody = null;
        SetLeftArm();
    }

    public void OnOfferComplete()
    {
        var item = DatabaseManager.instance.GetRandomItem();
        while (item == DatabaseManager.instance.GetEmptyItem())
        {
            item = DatabaseManager.instance.GetRandomItem();
        }
        // var instantiatedItem = Instantiate(item.WorldItem, _handPos.transform.position, new Quaternion());
        // instantiatedItem.GetComponent<Rigidbody>().isKinematic = true;
        var instantiatedOfferItem = Instantiate(_offerItem, _offerItemPos.position, new Quaternion());
        instantiatedOfferItem.GetComponent<MarketOffer>().marketArmEvents = this;
        instantiatedOfferItem.GetComponent<MarketOffer>().ItemSO = item;
    }

    public void StopOffering()
    {
        _marketArmAnimator.SetBool("IsOffering", false);
        SetRightArm();
        _marketArmAnimator.SetBool("IsClosing", true);
    }

    IEnumerator ShootEyes(int numEyesToSpawn)
    {
        for (int i = 0; i < numEyesToSpawn; i++)
        {
            yield return new WaitForSeconds(_spawnFrequency);
            var instantiatedCurrency = Instantiate(_currency, _currencySpawnPos.position + new Vector3(Random.Range(-_xRandomCurrencyOffset, _xRandomCurrencyOffset), 0.0f, 0.0f), transform.rotation);
            instantiatedCurrency.GetComponent<Rigidbody>().AddForce(transform.forward * _force, ForceMode.Impulse);
            instantiatedCurrency.GetComponent<Rigidbody>().AddTorque(transform.right * _force, ForceMode.Impulse);
        }
    }
}
