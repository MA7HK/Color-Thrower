using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

public class LoadBar : MonoBehaviour
{
	[SerializeField] private GameObject loadingBarPrefabs;
	[SerializeField] private Transform loadingBarPosition;

	private Image _fillImage;
	private BaseMiner _miner;
	private GameObject _barCanvas;

	void Start() {
		_miner = GetComponent<BaseMiner>();
		CreateLoadBar();
	}

	private void CreateLoadBar() {
		_barCanvas = Instantiate(loadingBarPrefabs, loadingBarPosition.position, Quaternion.identity);
		_barCanvas.transform.SetParent(transform);
		_fillImage = _barCanvas.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();

	}

    private void LoadingBar(BaseMiner minerSender, float duration) {
		if (_miner == minerSender) {
			_barCanvas.SetActive(true);
			_fillImage.fillAmount = 0.0f;
			_fillImage.DOFillAmount(1.0f, duration).OnComplete(
				() => _barCanvas.SetActive(false));
		}
    }

	private void OnEnable() {
		BaseMiner.OnLoading += LoadingBar;
	}

    private void OnDisable() {
		BaseMiner.OnLoading -= LoadingBar;
	}

}