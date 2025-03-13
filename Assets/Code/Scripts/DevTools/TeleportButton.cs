using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    private FoxMovement _player;
    private ITeleportPoint _target;
    internal ITeleportPoint Target { get => _target; set { _target = value; _buttonText.text = value.Name; } }

    private void Awake()
    {
        _button.onClick.AddListener(Teleport);
    }

    public void Teleport()
    {
        _player = FindFirstObjectByType<FoxMovement>();
        _player.gameObject.SetActive(false);
        _player.transform.position = _target.Position;
        _player.transform.rotation = _target.Rotation;
        _player.gameObject.SetActive(true);
    }
}
