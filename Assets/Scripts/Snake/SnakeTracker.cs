using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTracker : MonoBehaviour
{
    [SerializeField] private SnakeHead _snakeHead;
    [SerializeField] private float _speed;
    [SerializeField] private float _offsetY;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), _speed * Time.fixedDeltaTime);
    }

    private Vector3 GetTargetPosition()
    {
        return new Vector3(transform.position.x, _snakeHead.transform.position.y + _offsetY, transform.position.z);
    }
}
