using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyRing : MonoBehaviour
{
    [SerializeField] private float Scale;
    public float _needRange { get; private set; }
    private float ScaleToUp;
    private bool _enemyActive;
    private ParticleSystem EnemyParticleSystem;
    private CircleCollider2D CurrCollider;
    public Rigidbody2D EnemyBody;
    private Transform Player;
    [Inject]
    public void Construct(Transform player)
    {
        Player = player;
    }
    private void Start()
    {
        EnemyBody = GetComponent<Rigidbody2D>();
        CurrCollider = GetComponent<CircleCollider2D>();
        EnemyParticleSystem = GetComponent<ParticleSystem>();
        ScaleToUp = Scale * 1.1f;
    }
    private void Update()
    {
        if (!_enemyActive)
        {
            if(((Vector2)(transform.position - Player.position)).magnitude < _needRange){
                _enemyActive = true;
                EnemyParticleSystem.Play();
                StartCoroutine(ScaleTo1(transform.GetChild(0)));
                StartCoroutine(JumpAfterTime());
            }
        }
    }
    public void SetNeedRange(float needRange)
    {
        _needRange = needRange;
    }

    private IEnumerator JumpAfterTime()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            float y = Random.Range(0f, 1f);
            if (Player.position.y < transform.position.y)
                y = -y;
            Vector2 v = new Vector2(Random.Range(-1f, 1f), y);
            EnemyBody.velocity = Vector2.zero;
            float kp = Random.Range(0.5f, 1.2f);
            EnemyBody.AddForce(v.normalized * kp);
            if(Random.Range(0,5)==1)
                yield return new WaitForSeconds(0.35f);
            else
                yield return new WaitForSeconds(Random.Range(0.45f, 3f));
        }
    }
    private IEnumerator ScaleTo1(Transform image)
    {
        while (image.localScale.x < Scale)
        {
            float value = Mathf.Lerp(image.localScale.x, ScaleToUp, Time.deltaTime *3f);
            float value2 = Scale + value * 0.7f * Scale;
            CurrCollider.radius = value2;
            image.localScale = new Vector3(value, value, 1);
            yield return null;
        }
        image.localScale = new Vector3(Scale, Scale, 1);
        CurrCollider.radius = 1.7f;
    }
}
