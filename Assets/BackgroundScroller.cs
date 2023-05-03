using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public GameObject player;
    public float scrollSpeed = 0.5f;

    private Renderer _rend;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    // Use this for initialization
    void Start () {
        _rend = GetComponent<Renderer>();
    }
	
    // Update is called once per frame
    void Update () {
        var offset = Time.time * scrollSpeed;
        _rend.material.SetTextureOffset(MainTex, new Vector2(offset, 0));

    }
}
