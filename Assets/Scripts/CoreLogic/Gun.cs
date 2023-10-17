using System.Collections;
using UnityEngine;
using Zenject;

public class Gun : MonoBehaviour
{
    public float _time;
    public bool _toPlayer;
    public float _stayTime;
    private Animator anim;

    private Transform Player;
    [Inject]
    public void Construct(Transform player)
    {
        Player = player;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        if (_time < 0.2f)
            _time = 0.2f;
        StartCoroutine(Attack());
    }
    public void Update()
    {
        LookToPlayer();
    }

    private void LookToPlayer()
    {
        if (!_toPlayer) return;
        Vector2 resultVector = Player.position - transform.position;
        float a = Mathf.Atan(resultVector.y / resultVector.x) * 57.3248f;
        if (resultVector.x < 0)
            a += 180;
        transform.eulerAngles = new Vector3(0, 0, a);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(_stayTime);
        while (true)
        {
            anim.Rebind();
            yield return new WaitForSeconds(0.2f);
           //Attack
            yield return new WaitForSeconds(_time-0.2f);
        }
    }
}
