 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] pointsToMove;
    public float _speed;
    public bool _Up;

    private Transform pointToMove;
    public void Update()
    {
        Move();
    }
    public void StartMove()
    {
        if (pointsToMove.Length < 2) return;
        pointToMove = pointsToMove[0];
    }
    private void Move()
    {
        Vector3 deltaVector = (transform.position - pointToMove.position) * _speed * Time.deltaTime;
        transform.position += deltaVector;
        if(Vector3.Distance(transform.position, pointToMove.position)< (deltaVector.magnitude / 2))
        {
            if (pointToMove == pointsToMove[0])
                pointToMove = pointsToMove[1];
            else
                pointToMove = pointsToMove[0];
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.parent = null;
    }
}
