using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;  
    private Vector3 _offset;
    private float _initialSize;
    private Vector3 _initialPosition;

    public float cameraMoveSpeed = 8.0f;
    public float cameraZoomSpeed = 1.25f;

    private List<Coroutine> _coroutines;
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _initialSize = GetComponent<Camera>().orthographicSize;
        _initialPosition = transform.position;
        
        if (target != null)
        {
            _offset = transform.position - target.transform.position;
        }

        _coroutines = new List<Coroutine>();
    }


    void LateUpdate()
    {
        if (target == null) return;
        var position = target.transform.position;
        transform.position = new Vector3(position.x + _offset.x, position.y + _offset.y, -10);
    }

    public void SetTarget(GameObject targetEntity)
    {
        foreach (Coroutine cr in _coroutines)
        {
            StopCoroutine(cr);
        }
        
        if (targetEntity == null)
        {
            _coroutines.Add(StartCoroutine(LerpPosition(null, _initialPosition, 0.35f)));
            target = null;
        }
        else
        {
            _coroutines.Add(StartCoroutine(CamFollow(targetEntity, cameraMoveSpeed)));
            _coroutines.Add(StartCoroutine(SetCameraTargetSnap(targetEntity)));

        }
        //target = targetEntity;
    }

    IEnumerator SetCameraTargetSnap(GameObject targetEntity)
    {
        yield return new WaitForSeconds(0.5f);
        target = targetEntity;
    }
    
    IEnumerator CamFollow(GameObject targetEntity, float speed)
    {
        while (true)
        {
            var lerp = Vector3.Lerp(transform.position, targetEntity.transform.position, speed * Time.deltaTime);
            transform.position = new Vector3(lerp.x, lerp.y, -10);
            yield return null;
        }
    }

    private IEnumerator LerpPosition(GameObject targetEntity, Vector3 targetPosition, float duration)
    {
        float time = 0;
        var startPosition = transform.position;
        Vector3 lerpTarget;
        
        if (targetEntity != null)
        {
            var position = targetEntity.transform.position;
            lerpTarget = new Vector3(position.x, position.y, -10);
        }
        else
        {
            lerpTarget = new Vector3(targetPosition.x, targetPosition.y, -10);
        }
        
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, lerpTarget, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void SetSize(float size)
    {
        if (size <= 0.0f)
        {
            _coroutines.Add(StartCoroutine(SetCameraSize(_initialSize)));
            return;
        }

        _coroutines.Add(StartCoroutine(SetCameraSize(size)));
    }

    private IEnumerator SetCameraSize(float targetSize)
    {
        float time = 0;
        var startValue = _camera.orthographicSize;
        var duration = cameraZoomSpeed;
        
        while (time < duration)
        {
            _camera.orthographicSize = Mathf.Lerp(startValue, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _camera.orthographicSize = targetSize;
    }

    public void ResetCamera()
    {
        transform.position = _initialPosition;
        SetSize(0.0f);
    }
}
