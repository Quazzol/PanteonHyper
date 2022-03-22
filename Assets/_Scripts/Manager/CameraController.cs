using System.Collections;
using UnityEngine;

namespace Manager
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera _mainCamera = null;

        private Transform _objectToFollow = null;
        private Vector3 _defaultPosition;
        private Quaternion _defaultRotation;
        private float _cameraSpeed = 30f;
        private Coroutine _follow;
        private Coroutine _rotate;

        private void Awake()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;

            _defaultPosition = _mainCamera.transform.position;
            _defaultRotation = _mainCamera.transform.rotation;
        }

        private void Reset()
        {
            _mainCamera.transform.position = _defaultPosition;
            _mainCamera.transform.rotation = _defaultRotation;
        }

        public void SetObjectToFollow(Transform obj)
        {
            _objectToFollow = obj;
        }

        public void StartFollowing()
        {
            StopRotating();
            _follow = StartCoroutine(Follow());
        }

        private void StopFollowing()
        {
            if (_follow == null)
                return;

            StopCoroutine(_follow);
        }

        public void StartRotating()
        {
            StopFollowing();
            _rotate = StartCoroutine(Rotate());
        }

        public void StopRotating()
        {
            Reset();

            if (_rotate == null)
                return;

            StopCoroutine(_rotate);
        }

        private IEnumerator Follow()
        {
            while (true)
            {
                if (_objectToFollow == null)
                    break;

                Vector3 targetPosition = new Vector3(_defaultPosition.x + _objectToFollow.position.x,
                                                    _defaultPosition.y + _objectToFollow.position.y,
                                                    _defaultPosition.z + _objectToFollow.position.z);
                _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, targetPosition, Time.deltaTime * _cameraSpeed);

                yield return null;
            }
        }

        private IEnumerator Rotate()
        {
            while (true)
            {
                if (_objectToFollow == null)
                    break;

                _mainCamera.transform.LookAt(_objectToFollow);
                _mainCamera.transform.RotateAround(_objectToFollow.position, Vector3.up, _cameraSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
}