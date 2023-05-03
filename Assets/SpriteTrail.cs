using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpriteTrail : MonoBehaviour
{
    public int cloneCount = 10;
    public float velocityThreshold = 0.1f;
    public float trailDuration = 0.5f;
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private Transform _tf;
    private List<SpriteRenderer> _clones;
    public Vector3 scalePerSecond = new Vector3(1f, 1f, 1f);
    public Color colorPerSecond = new Color(255, 255, 255, 1f);
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tf = GetComponent<Transform>();
        _sr = GetComponent<SpriteRenderer>();
        _clones = new List<SpriteRenderer>();
        StartCoroutine(Trail());
    }

    void Update()
    {
        for (int i = 0; i < _clones.Count; i++)
        {
            _clones[i].color -= colorPerSecond * Time.deltaTime;
            _clones[i].transform.localScale -= scalePerSecond * Time.deltaTime;
            if (_clones[i].color.a <= 0f || _clones[i].transform.localScale.z <= 0.0f)
            {
                Destroy(_clones[i].gameObject);
                _clones.RemoveAt(i);
                i--;
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator Trail()
    {
        while(true)
        {
            if (_rb.velocity.magnitude > velocityThreshold)
            {
                var clone = new GameObject("trailClone") {
                    transform = {
                        position = _tf.position,
                        rotation = _tf.rotation,
                        localScale = _tf.localScale
                    }
                };
                var cloneRend = clone.AddComponent<SpriteRenderer>();
                cloneRend.sprite = _sr.sprite;
                cloneRend.sortingOrder = _sr.sortingOrder - 1;
                _clones.Add(cloneRend);
            }
            yield return new WaitForSeconds(trailDuration / cloneCount);
        }
    }
}
