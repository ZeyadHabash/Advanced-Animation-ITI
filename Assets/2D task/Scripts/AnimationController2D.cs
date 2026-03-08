using UnityEngine;

public class AnimationController2D : MonoBehaviour
{
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var inputx = Input.GetAxisRaw("Horizontal");
        var inputy = Input.GetAxisRaw("Vertical");
        _animator.SetFloat("x_velocity", inputx);
        _animator.SetFloat("y_velocity", inputy);

    }
}
