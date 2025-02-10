using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button SettingBtn;
    [SerializeField] private Button BackBtn;
    [SerializeField] private GameObject SettingPanel;

    private void Start() {
        SettingBtn.onClick.AddListener(OpenSettingPanel);
        BackBtn.onClick.AddListener(CloseSettingPanel);
    }

    void OpenSettingPanel() {
        SettingPanel.SetActive(true);
    }

    void CloseSettingPanel() {
        SettingPanel.SetActive(false);
    }
}
