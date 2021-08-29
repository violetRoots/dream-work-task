using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingArea : MonoBehaviour
{
    public Enemy Enemy { get; private set; }

    private void Start()
    {
        Enemy = GetComponentInParent<Enemy>();
    }
}
