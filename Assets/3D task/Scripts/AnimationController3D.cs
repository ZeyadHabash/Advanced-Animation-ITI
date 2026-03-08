using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationController3D : MonoBehaviour
{
    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private TwoBoneIKConstraint _leftHandIKConstraint;
    [SerializeField] private Transform _objectToHold;
    [SerializeField] private float _animationDuration = 1.0f;

    private Vector3 _originalPosition;
    private bool _isPlaying;

    void Start()
    {
        _originalPosition = _leftHandTarget.localPosition;
        _leftHandIKConstraint.weight = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_isPlaying)
        {
            StartCoroutine(HoldHandle());
        }
    }

    private IEnumerator HoldHandle()
    {
        _isPlaying = true;

        float elapsed = 0;
        float startWeight = _leftHandIKConstraint.weight;
        Vector3 startPos = _leftHandTarget.position;

        while (elapsed < _animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _animationDuration; 

            _leftHandIKConstraint.weight = Mathf.Lerp(startWeight, 1.0f, t);
            _leftHandTarget.position = Vector3.Lerp(startPos, _objectToHold.position, t);

            yield return null; 
        }

        _leftHandIKConstraint.weight = 1.0f;
        _leftHandTarget.position = _objectToHold.position;

        yield return new WaitForSeconds(2f);

        elapsed = 0;
        startWeight = _leftHandIKConstraint.weight;
        startPos = _leftHandTarget.position;

        Vector3 targetReturnPos = transform.TransformPoint(_originalPosition);

        while (elapsed < _animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _animationDuration;

            _leftHandIKConstraint.weight = Mathf.Lerp(startWeight, 0f, t);
            _leftHandTarget.position = Vector3.Lerp(startPos, targetReturnPos, t);

            yield return null; 
        }

        _leftHandIKConstraint.weight = 0f;
        _isPlaying = false;
    }
}