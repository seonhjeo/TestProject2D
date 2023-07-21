
using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderJobMove : MonoBehaviour
{
    private Camera _camera;
    private Vector2 _targetDir;
    private float _changeDirCooldown;
    private float _speed;
    private float _rotationSpeed;
    
    private NativeArray<float> _changeDirCooldownRes;
    private NativeArray<Vector2> _targetDirRes;
    private NativeArray<Vector2> _posRes;
    private NativeArray<Quaternion> _rotationRes;

    private JobHandle _jobHandle;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _camera = Camera.main;
        _targetDir = Vector2.up;
        _speed = Random.Range(2f, 5f);
        _rotationSpeed = Random.Range(90f, 180f);

        _changeDirCooldownRes = new NativeArray<float>(1, Allocator.Persistent);
        _targetDirRes = new NativeArray<Vector2>(1, Allocator.Persistent);
        _posRes = new NativeArray<Vector2>(1, Allocator.Persistent);
        _rotationRes = new NativeArray<Quaternion>(1, Allocator.Persistent);
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 screenPoint = _camera.WorldToScreenPoint(transform.position);

        SpiderJob job = new SpiderJob(
            _targetDir,
            _changeDirCooldown,
            _speed,
            _rotationSpeed,
            Time.deltaTime,
            (uint)Random.Range(1, 1000),
            screenPoint,
            _camera.pixelWidth,
            _camera.pixelHeight,
            transform.rotation,
            transform.position,
            _changeDirCooldownRes,
            _targetDirRes,
            _posRes,
            _rotationRes
        );

        _jobHandle = job.Schedule();
    }

    private void LateUpdate()
    {
        _jobHandle.Complete();

        _targetDir = _targetDirRes[0];
        _changeDirCooldown = _changeDirCooldownRes[0];
        transform.rotation = _rotationRes[0];
        transform.position = _posRes[0];
    }

    private void OnDestroy()
    {
        _changeDirCooldownRes.Dispose();
        _targetDirRes.Dispose();
        _posRes.Dispose();
        _rotationRes.Dispose();
    }
}
