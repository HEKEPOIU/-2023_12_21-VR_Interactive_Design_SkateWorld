using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController _controller;
        [SerializeField] private Camera _vRHeadCamera;

        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _gravityMultiplier = 1.0f;
        // Start is called before the first frame update
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        private void Update()
        {
            Vector3 moveDir = _vRHeadCamera.transform.forward;
            moveDir.y = 0;
            _controller.Move(moveDir * (Time.deltaTime * _speed));
            _controller.Move(Physics.gravity * (Time.deltaTime * _gravityMultiplier));
        
        }
    }
}
