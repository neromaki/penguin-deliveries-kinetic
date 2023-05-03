using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;  
    private Vector3 offset;
    private float initialSize;
    private Vector3 initialPosition;

    public float cameraMoveSpeed = 8.0f;
    public float cameraZoomSpeed = 1.25f;

    private List<Coroutine> coroutines;

    void Start()
    {
        initialSize = GetComponent<Camera>().orthographicSize;
        initialPosition = transform.position;
        
        if (target != null)
        {
            offset = transform.position - target.transform.position;
        }

        coroutines = new List<Coroutine>();
    }


    void LateUpdate()
    {
        if (target == null) return;
        transform.position = new Vector3(target.transform.position.x + offset.x, target.transform.position.y + offset.y, -10);
        //transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f * Time.deltaTime);
        
    }

    public void SetTarget(GameObject targetEntity)
    {
        foreach (Coroutine cr in coroutines)
        {
            StopCoroutine(cr);
        }
        
        if (targetEntity == null)
        {
            coroutines.Add(StartCoroutine(LerpPosition(null, initialPosition, 0.35f)));
            target = null;
        }
        else
        {
            coroutines.Add(StartCoroutine(CamFollow(targetEntity, cameraMoveSpeed)));
            coroutines.Add(StartCoroutine(SetCameraTargetSnap(targetEntity)));

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
        float time = 0;
        //Vector3 startPosition = transform.position;
        
        while (true)
        {
            Vector3 lerped = Vector3.Lerp(transform.position, targetEntity.transform.position, speed * Time.deltaTime);
            transform.position = new Vector3(lerped.x, lerped.y, -10);
            yield return null;
        }
    }
    
    IEnumerator LerpPosition(GameObject targetEntity, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        Vector3 target;
        if (targetEntity != null)
        {
            target = new Vector3(targetEntity.transform.position.x, targetEntity.transform.position.y, -10);
        }
        else
        {
            target = new Vector3(targetPosition.x, targetPosition.y, -10);
        }
        
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    public void SetSize(float size)
    {
        if (size <= 0.0f)
        {
            //GetComponent<Camera>().orthographicSize = initialSize;
            coroutines.Add(StartCoroutine(SetCameraSize(initialSize)));
            return;
        }

        coroutines.Add(StartCoroutine(SetCameraSize(size)));
    }

    private IEnumerator SetCameraSize(float targetSize)
    {
        float time = 0;
        float startValue = GetComponent<Camera>().orthographicSize;
        float duration = cameraZoomSpeed;
        
        while (time < duration)
        {
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(startValue, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        GetComponent<Camera>().orthographicSize = targetSize;
    }

    public void ResetCamera()
    {
        transform.position = initialPosition;
        SetSize(0.0f);
    }
}
