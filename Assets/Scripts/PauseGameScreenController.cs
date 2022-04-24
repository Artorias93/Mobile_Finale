using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    
    public void showPausePanel() {
        panel.SetActive(true);
    }

    public void hidePausePanel() {
        panel.SetActive(false);
    }
}
