using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nick : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator _pedestalAnimator;
    [SerializeField] private string _activeBoolName = "isHologramActive";

    [Header("Behaviours")]
    [SerializeField] private float _activationTime = 0.5f;

    [Header("Inputs")]
    [SerializeField] private KeyCode _activationKey = KeyCode.Space;

    [Header("Shader")]
    [SerializeField] private string _displacementFloatName = "_DisplacementAmount";

    private bool _isHologramActive = false, _isCoroutineActive = false;

    private Animator _animator;
    private Material[] _materials;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = 0.0f;
        _pedestalAnimator.SetBool(_activeBoolName, false);
        GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _materials = GetComponentInChildren<Renderer>().materials;
        foreach(Material material in _materials)
        {
            material.SetFloat(_displacementFloatName, 0.0f);
        }        
    }

    private void Update()
    {
        if(Input.GetKeyDown(_activationKey) && !_isCoroutineActive)
        {
            StartCoroutine(HologramActivation());
        }
    }

    private IEnumerator HologramActivation()
    {
        _isCoroutineActive = true;

        float t = 0.0f;

        if (!_isHologramActive)
        {
            _pedestalAnimator.SetBool(_activeBoolName, true);

            _animator.speed = 1.0f;

            GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }

        while(t < 1.0f)
        {
            t += Time.deltaTime / _activationTime;

            foreach(Material material in _materials)
            {
                if (_isHologramActive)
                {
                    material.SetFloat(_displacementFloatName, Mathf.Lerp(1.0f, 0.0f, t));
                }
                else
                {
                    material.SetFloat(_displacementFloatName, Mathf.Lerp(0.0f, 1.0f, t));
                }
            }

            yield return null;
        }

        if (_isHologramActive)
        {
            _pedestalAnimator.SetBool(_activeBoolName, false);

            _animator.speed = 0.0f;

            GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        _isHologramActive = !_isHologramActive;

        _isCoroutineActive = false;
    }
}
