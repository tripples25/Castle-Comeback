using System;
using UnityEngine;

public class Target : Entity
{
    public bool isLocked;
    private static readonly int IsTargetReached = Animator.StringToHash("IsTargetReached");

    private void Start()
    {
        GameManager.Instance.OnAllPathsCompleted += StartAnimation;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnAllPathsCompleted -= StartAnimation;
    }

    private void StartAnimation()
    {
        GetComponent<Animator>().SetBool(IsTargetReached, true);
    }
}
