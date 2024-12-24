using UnityEngine;

public class Figure : MonoBehaviour
{
    public PlayerType PlayerType {get; private set;}
    public int Power { get { return power; } }
    [SerializeField] int power;
    Vector3 startPos;
    Collider figureCollider;

    void Start()
    {
        startPos = transform.position;
        figureCollider = gameObject.GetComponent<Collider>();
    }

    public void Init(PlayerType playerType)
    {
        PlayerType = playerType;
    }

    public void OnFigureClick(bool canReplace)
    {
        if(canReplace)
        {
            transform.position = startPos + Vector3.up;
        }
        else
        {
            transform.position = startPos;
        }
    }

    public void DestroyColl()
    {
        Destroy(figureCollider);
    }
}
