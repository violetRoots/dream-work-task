using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    private const float BorderOffset = 1.0f;

    private void Update()
    {
        var pos = transform.position;
        if (pos.z > ScreenController.Height / 2)
        {
            pos.z = -pos.z + BorderOffset;
        }
        if(pos.z < -ScreenController.Height / 2)
        {
            pos.z = -pos.z - BorderOffset;
        }
        if (pos.x > ScreenController.Width / 2)
        {
            pos.x = -pos.x + BorderOffset;
        }
        if(pos.x < -ScreenController.Width / 2)
        {
            pos.x = -pos.x - BorderOffset;
        }

        transform.position = pos;
    }
}
