using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum ZoneType
{
    Slow =0,
    Fast,
    PullToCenter,
    RepelFromCenter,
    Repel
}
public abstract class Zone : MonoBehaviour
{
    //public ZoneType ZoneType
    public float power;
    public float NeedDistanceToCenter;

    protected List<Rigidbody2D> allObjectsInZone;
    private GameState GameState;
    private UIInputController UIInputController;
    protected Transform Player;
    private Rigidbody2D ZoneBody;
    [Inject]
    public void Construct(GameState gameState, UIInputController uIInputController, Transform player)
    {
        GameState = gameState;
        UIInputController = uIInputController;
        Player = player;
    }
    public void Start()
    {
        allObjectsInZone = new List<Rigidbody2D>();
        ZoneBody = GetComponent<Rigidbody2D>();
        if (ZoneBody != null)
            Invoke(nameof(DestroyAfterTime), 60f);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D collisionBody = collision.transform.GetComponent<Rigidbody2D>();
        if (collisionBody != null)
            allObjectsInZone.Add(collisionBody);
        TryDestroyMovedZone(collision);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D collisionBody = collision.transform.GetComponent<Rigidbody2D>();
        if (collisionBody != null)
            allObjectsInZone.Remove(collision.transform.GetComponent<Rigidbody2D>());
    }
    public void DestroyAfterTime()
    {
        Destroy(gameObject);
    }
    private void TryDestroyMovedZone(Collider2D collision)
    {
        if (ZoneBody != null)
            if (collision.gameObject.layer == 0)
                if (!collision.gameObject.CompareTag("Zone") && !collision.gameObject.CompareTag("death") && collision.GetComponent<Teleport>() == null)
                    Destroy(gameObject);
    }

}
