using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class ActivatingBaseObject : MonoBehaviour, IActivatedObject
{
    public ActivationPlateActionType ActionType;
    //public ActivationPlateTypeObject TypeObject;
    public Transform FirstUsedObject;
    public Transform SecondUsedObject;
    public Transform ThirdUsingObject;
    public float speed;
    [SerializeField] protected Transform ButtonScalablePart;
    [SerializeField] private bool _movingToStartPoint;

    private Animation PlaybleAnimation;
    protected Collider2D ActivatedCollider;
    private SpriteRenderer FirstUsedObjectSprite;
    protected bool _active;
    protected float _timeToAction = 1;
    protected GameState GameState;
    [Inject]
    public void Construct(GameState gameState)
    {
        GameState = gameState;
    }

    public virtual void Start()
    {
        BindFirstUsedObjectReferences();
    }

    public virtual void BindFirstUsedObjectReferences()
    {
        if (FirstUsedObject != null)
        {
            PlaybleAnimation = FirstUsedObject.GetComponent<Animation>();
            ActivatedCollider = FirstUsedObject.GetComponent<Collider2D>();
            FirstUsedObjectSprite = FirstUsedObject.GetComponent<SpriteRenderer>();
            //Gun = FirstUsedObject.GetComponent<Gun>();
        }
    }

    public virtual void Action()
    {
        if (FirstUsedObject == null) return;
        switch (ActionType)
        {
            case ActivationPlateActionType.Rotate:
                Rotate();
                break;
            case ActivationPlateActionType.RotateAroundObject:
                RotateAroundObject();
                break;
            case ActivationPlateActionType.Animation:
                TryAnimateObject();
                break;
            case ActivationPlateActionType.ActiveInFalse:
                DisableActivatedCollider();
                VisualActiveButton(0.4f, 0.4f);
                break;
            case ActivationPlateActionType.Move:
                MoveObject();
                break;
        }
    }

    private void MoveObject()
    {
        if (!_movingToStartPoint)
            MoveFirstUsedObject(ThirdUsingObject.position);
        else
            MoveFirstUsedObject(SecondUsedObject.position);
    }

    private void Rotate()
    {
        FirstUsedObject.Rotate(0, 0, speed);
    }

    private void RotateAroundObject()
    {
        FirstUsedObject.RotateAround(SecondUsedObject.position, new Vector3(0, 0, 1), speed * Time.unscaledDeltaTime);
    }

    private void TryAnimateObject()
    {
        if (PlaybleAnimation != null)
            if (!PlaybleAnimation.isPlaying)
                PlaybleAnimation.Play();
    }

    private void MoveFirstUsedObject(Vector3 PointToMove)
    {
        Vector3 MovedObjectPosition = FirstUsedObject.position;
        float deltaPosition = Time.deltaTime * speed;
        MoveMovedObject();
        CheckPositionAfterMove();
        void CheckPositionAfterMove()
        {
            if ((MovedObjectPosition - PointToMove).magnitude < (SecondUsedObject.position - ThirdUsingObject.position).magnitude * deltaPosition)
                _movingToStartPoint = !_movingToStartPoint;
        }
        void MoveMovedObject()
        {
            FirstUsedObject.position = MovedObjectPosition + (PointToMove - MovedObjectPosition).normalized * deltaPosition;
        }
    }

    private void DisableActivatedCollider()
    {
        ActivatedCollider.enabled = false;
    }

    protected void ChangeActionButtonReActive()
    {
        ActivatedCollider.enabled = !ActivatedCollider.enabled;
        VisualActiveButtonWithCollider();
    }

    protected void VisualActiveButtonWithCollider()
    {
        if (ActivatedCollider.enabled)
            VisualActiveButton(0.4f, 1f);
        else
            VisualActiveButton(1f, 0.4f);
    }

    private void VisualActiveButton(float spriteUsedObjectAlpha, float scaleX)
    {
        if(FirstUsedObjectSprite != null)
            FirstUsedObjectSprite.color = new Color(FirstUsedObjectSprite.color.r, FirstUsedObjectSprite.color.g, FirstUsedObjectSprite.color.b, spriteUsedObjectAlpha);
        if(ButtonScalablePart != null) 
            ButtonScalablePart.localScale = new Vector3(scaleX, 1, 1);
    }

    protected void Deactive()
    {
        _active = false;
    }

    private void Update()
    {
        if (GameState.ActiveCreateMode) return;    
        if (_active)
            Action();

    }

    protected void Active()
    {
        _active = true;
    }
}
