using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenController
{
    public static float Width
    {
        get
        {
            var camera = Camera.main;
            var leftDownCorner = new Vector3(0, 0, camera.transform.position.y);
            return -camera.ScreenToWorldPoint(leftDownCorner).x * 2;
        }
    }
    public static float Height
    {
        get
        {
            var camera = Camera.main;
            var leftDownCorner = new Vector3(0, 0, camera.transform.position.y);
            return -camera.ScreenToWorldPoint(leftDownCorner).z * 2;
        }
    }
}
