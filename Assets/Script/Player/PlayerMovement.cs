using System;
using System.Linq;
using UnityEngine;

namespace Player
{
    internal enum Direction
    {
        Center,
        Left,
        Right
    }
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController _controller;
        private float[] _sensorData;
        private Direction _rotationDirection = Direction.Center;
        private bool _isRuning = false;
        private float _rotation = 0;
        private float _speed;
        private float _boostPercent = 0;
        
        private bool _isMoveAble = false;
        public event Action Ontend;

        [SerializeField] private float _rotationThreshold = 20f;
        [SerializeField] private float _gravityMultiplier = 1.0f;
        [SerializeField] private float _rotationDeadZone = 50f;
        [SerializeField] private float _sprintDeadZone = 1f;
        [SerializeField] private float _baseMaxSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _sprintMaxSpeed = 10f;
        // Start is called before the first frame update
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            SerialReader.Instance.OnDataReceived += ReadSensorData;
        }


        // Update is called once per frame
        private void Update()
        {
            if (_isMoveAble == false) return;

            HandleSprint();
            RotatePlayer();
            
            Vector3 moveDir = transform.forward;
            moveDir += Physics.gravity * (Time.deltaTime * _gravityMultiplier);
            _controller.Move(moveDir * (Time.deltaTime * _speed));
        
        }

        private void HandleSprint()
        {
            _boostPercent += Time.deltaTime;
            _boostPercent = Mathf.Clamp01(_boostPercent);

            if (_isRuning)
            {
                _speed = Mathf.Lerp(0, _sprintMaxSpeed, _boostPercent);
            }
            else
            {
                _speed = Mathf.Lerp(0, _baseMaxSpeed, _boostPercent);
            }
        }

        private void ReadSensorData(string data)
        {
            _sensorData = data.Split(',').Select(float.Parse).ToArray();
            
            
            float tendency = _sensorData[0];
            
            _isRuning = tendency > -_sprintDeadZone;
            if (_isRuning)
            {
                Ontend?.Invoke();
            }
            if (_isMoveAble == false) return;

            if (_rotation > _rotationDeadZone)
            {
                if (_sensorData[2] < -_rotationThreshold)
                {
                    _rotation += _sensorData[2];
                }
                _rotationDirection = Direction.Right;
            }
            else if (_rotation < -_rotationDeadZone)
            {
                if (_sensorData[2] > _rotationThreshold)
                {
                    _rotation += _sensorData[2];
                }
                _rotationDirection = Direction.Left;
            }
            else
            {
                if (_sensorData[2] > _rotationThreshold || _sensorData[2] < -_rotationThreshold)
                {
                    _rotation += _sensorData[2];
                }
                _rotationDirection = Direction.Center;
            }
        }
        private void RotatePlayer()
        {
            switch (_rotationDirection)
            {
                case Direction.Left:
                    transform.Rotate(Vector3.up, -Time.deltaTime * _rotationSpeed);
                    break;
                case Direction.Right:
                    transform.Rotate(Vector3.up, Time.deltaTime * _rotationSpeed);
                    break;
                case Direction.Center:
                default:
                    break;
            }

            if (transform.rotation.eulerAngles.y > 45 && transform.rotation.eulerAngles.y < 180)
            {
                transform.rotation = Quaternion.Euler(0, 45, 0);
            }
            else if (transform.rotation.eulerAngles.y > 180 && transform.rotation.eulerAngles.y < 315)
            {
                transform.rotation = Quaternion.Euler(0, 315, 0);
            }
        }

        private void OnDestroy()
        {
            SerialReader.Instance.OnDataReceived -= ReadSensorData;
        }
        
        public void SetMoveAble(bool isMoveAble)
        {
            _isMoveAble = isMoveAble;
        }
    }
    
    
}
