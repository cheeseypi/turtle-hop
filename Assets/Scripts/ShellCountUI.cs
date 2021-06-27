using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShellCountUI : MonoBehaviour
{
    public GameObject targetPlayerObject;

    private TextMeshProUGUI _shellCountText;
    private PlayerController _targetPlayerController;
    private int _numShells;
    // Start is called before the first frame update
    void Start()
    {
        _targetPlayerController = targetPlayerObject.GetComponent<PlayerController>();
        _shellCountText = this.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetPlayerController.CanLeaveLevel)
        {
            _shellCountText.text = "";
        }
        else
        {
            _numShells = _targetPlayerController.numPlatforms;
            _shellCountText.text = "Shells: " + _numShells;
        }
    }
}
