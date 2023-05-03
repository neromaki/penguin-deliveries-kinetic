using UnityEngine;

public class OffsetScroller : MonoBehaviour
{
    public GameObject player;
    public float scrollSpeed;
    private Vector2 _savedOffset;
    private string _materialName;
    private Rigidbody2D _rigidbody2D;
    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody2D = player.GetComponent<Rigidbody2D>();
        _materialName = "_MainTex";
        GetComponent<Renderer>().material.SetTextureOffset(_materialName, new Vector2(0, 0));
        _savedOffset = GetComponent<Renderer>().material.GetTextureOffset(_materialName);
    }

    void Update()
    {
        Vector2 velocity = _rigidbody2D.velocity;
        float speed = velocity.magnitude;        

        Vector2 offset = new Vector2(_savedOffset.x + ((velocity.x /2) * ((speed / 10000) * scrollSpeed)) * -1, _savedOffset.y + ((velocity.y * ((speed / 10000) * scrollSpeed))));
        _renderer.material.SetTextureOffset(_materialName, offset);
        _savedOffset = _renderer.material.GetTextureOffset(_materialName);
    }

    void OnDisable()
    {
        _renderer.material.SetTextureOffset(_materialName, _savedOffset);
    }
}
