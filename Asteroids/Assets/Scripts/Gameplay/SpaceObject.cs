using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    private const float BorderOffset = 1.0f;

    public float PathLength { get; private set; }

    private Vector3 _prevoiusPos;
    private float _width, _height;

    private void Start()
    {
        _width = ScreenController.Width / 2;
        _height = ScreenController.Height / 2;
        _prevoiusPos = transform.position;
    }

    private void Update()
    {
        var difDistance = Vector3.Distance(_prevoiusPos, transform.position);

        var pos = transform.position;
        if (pos.z > _height)
        {
            pos.z = -pos.z + BorderOffset;
            difDistance = 0;
        }
        if(pos.z < -_height)
        {
            pos.z = -pos.z - BorderOffset;
            difDistance = 0;
        }
        if (pos.x > _width)
        {
            pos.x = -pos.x + BorderOffset;
            difDistance = 0;
        }
        if(pos.x < -_width)
        {
            pos.x = -pos.x - BorderOffset;
            difDistance = 0;
        }

        transform.position = pos;
        _prevoiusPos = pos;
        PathLength += difDistance;
    }

    public void ResetPath() { PathLength = 0; }
}
