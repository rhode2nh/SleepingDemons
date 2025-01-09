using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _marketArmAnimator;
    [SerializeField] private MarketArmEvents _marketArmEvents;
    [SerializeField] private float _grabWaitDuration;
    [SerializeField] private float _closeWaitDuration;
    [SerializeField] private bool _closeMarket = false;
    [SerializeField] private bool _openMarket = false;

    void Update()
    {
        if (_openMarket)
        {
            _openMarket = false;
            StartCoroutine(OpenMarket());
        }
        if (_closeMarket)
        {
            _closeMarket = false;
            StartCoroutine(CloseMarket());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WorldItem>() != null || other.gameObject.GetComponent<ICurrency>() != null)
        {
            if (other.gameObject.GetComponent<ICurrency>() != null)
            {
                if (other.gameObject.GetComponent<ICurrency>().IsBag())
                {
                    StartCoroutine(Grab(other.gameObject));
                }
            }
            else
            {
                StartCoroutine(Grab(other.gameObject));
            }
        }
    }

    public IEnumerator Grab(GameObject other)
    {
        yield return new WaitForSeconds(_grabWaitDuration);
        _marketArmAnimator.SetBool("IsGrabbing", true);
        _marketArmEvents.SetWorldItem(other.gameObject);
    }

    public IEnumerator OpenMarket()
    {
        yield return new WaitForSeconds(_closeWaitDuration);
        _marketArmAnimator.SetBool("IsClosing", false);
        _marketArmAnimator.SetBool("IsOpening", true);
        yield return new WaitForSeconds(1.0f);
        _marketArmEvents.OnOpenDoorComplete();
        // _marketArmAnimator.SetBool("IsOpening", false);
        // _marketArmEvents.OpenDoor();
    }

    public IEnumerator CloseMarket()
    {
        yield return new WaitForSeconds(_closeWaitDuration);
        _marketArmEvents.SetRightArm();
        _marketArmAnimator.SetBool("IsClosing", true);
        // _marketArmEvents.SetArmPosition(Vector3.zero + new Vector3(-0.1f, -0.5f, -0.1f));
        ClearAllActions();
        // _marketArmEvents.CloseDoor();
    }

    public void ClearAllActions()
    {
        _marketArmAnimator.SetBool("IsOffering", false);
        // Let grab animation finish and return to idle before closing
        // _marketArmAnimator.SetBool("IsGrabbing", false);
    }
}
