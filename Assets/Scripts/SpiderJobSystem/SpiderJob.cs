
using Unity.Burst;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

[BurstCompile]
public struct SpiderJob : IJob
{
    private Vector2 _targetDir;                         // 목표 위치
    private float _changeDirCooldown;                   // 방향 변경 쿨타임
    private float _speed;                               // 움직이는 속도
    private float _rotSpeed;                            // 회전 속도

    private float _deltaTime;                           // 델타타임
    private uint _seed;                                 // 랜덤값 시드
    private Vector2 _screenPoint;                       // 화면 위치
    private int _screenWidth;                           // 화면 너비
    private int _screenHeight;                          // 화면 위치
    private Vector2 _position;                          // 객체 위치
    private Quaternion _rotation;                       // 객체 회전값
    
    private NativeArray<float> _changeDirCooldownRes;   // 방향 변경 쿨타임
    private NativeArray<Vector2> _targetDirRes;         // 목표 위치
    private NativeArray<Vector2> _posRes;               // 객체 위치
    private NativeArray<Quaternion> _rotationRes;       // 객체 회전값

    public SpiderJob(
        Vector2 targetDir,
        float changeDirCooldown,
        float speed,
        float rotSpeed,
        float deltaTime,
        uint seed,
        Vector2 screenPoint,
        int screenWidth,
        int screenHeight,
        Quaternion rotation,
        Vector2 position,
        NativeArray<float> changeDirCooldownRes,
        NativeArray<Vector2> targetDirRes,
        NativeArray<Vector2> posRes,
        NativeArray<Quaternion> rotationRes
    )
    {
        _targetDir = targetDir;
        _changeDirCooldown = changeDirCooldown;
        _speed = speed;
        _rotSpeed = rotSpeed;
        _deltaTime = deltaTime;
        _seed = seed;
        _screenPoint = screenPoint;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
        _rotation = rotation;
        _position = position;
        _changeDirCooldownRes = changeDirCooldownRes;
        _targetDirRes = targetDirRes;
        _posRes = posRes;
        _rotationRes = rotationRes;
    }
    
    public void Execute()
    {
        UpdateTargetDirection();
        RotateTowardstarget();
        SetPosition();
    }

    // 방향 변경 함수
    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandleOffScreen();

        _targetDirRes[0] = _targetDir;
    }

    // 갈 방향을 랜덤하게 변경하는 함수
    private void HandleRandomDirectionChange()
    {
        _changeDirCooldown -= _deltaTime;

        if (_changeDirCooldown <= 0)
        {
            var random = new Unity.Mathematics.Random(_seed);

            float angleChange = random.NextFloat(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, Vector3.forward);
            _targetDir = rotation * _targetDir;

            _changeDirCooldown = random.NextFloat(1f, 5f);
        }

        _changeDirCooldownRes[0] = _changeDirCooldown;
    }
    
    // 화면 경계 도달시 방향을 뒤로 바꾸는 함수
    private void HandleOffScreen()
    {
        if ((_screenPoint.x < 0 && _targetDir.x < 0) ||
            (_screenPoint.x > _screenWidth && _targetDir.x > 0))
        {
            _targetDir = new Vector2(-_targetDir.x, _targetDir.y);
        }
        
        if ((_screenPoint.y < 0 && _targetDir.x < 0) ||
            (_screenPoint.y > _screenHeight && _targetDir.x > 0))
        {
            _targetDir = new Vector2(_targetDir.x, -_targetDir.y);
        }
    }

    // 오브젝트의 현재 방향을 계산하는 함수
    private void RotateTowardstarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, _targetDir);
        _rotation = Quaternion.RotateTowards(_rotation, targetRotation, _rotSpeed * _deltaTime);

        _rotationRes[0] = _rotation;
    }
    
    // 방향과 전진거리를 바탕으로 현재 위치를 반환하는 함수
    private void SetPosition()
    {
        Vector2 posChange = _speed * _deltaTime * (_rotation * Vector2.up);

        _position = _position + posChange;

        _posRes[0] = _position;
    }
}
