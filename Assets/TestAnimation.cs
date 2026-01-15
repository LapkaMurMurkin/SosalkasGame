using Extensions;
using SosalkasGame.Extensions;
using TMPro;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    private float _writerProgress;
    private TextMeshProUGUI _textMesh;
    private TMPTagAnimator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        _animator = new TMPTagAnimator(_textMesh);
    }

/*     private void OnDestroy()
    {
        _animator.Dispose();
    } */
}
