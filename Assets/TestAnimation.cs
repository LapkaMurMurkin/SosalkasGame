using TMPro;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    private float _writerProgress;
    private TMP_Text _textMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _textMesh = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float writerSpeed = 2f;

        if (_writerProgress >= _textMesh.text.Length)
            return;

        _writerProgress += Time.deltaTime * writerSpeed;
        _textMesh.maxVisibleCharacters = (int)_writerProgress;
    }
}
