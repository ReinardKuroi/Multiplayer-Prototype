using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterHealth;

public class PlayerNameDisplay : MonoBehaviour {

	public float offsetY = 50f;
	public Text PlayerName;
	public Image PlayerHealth;

	float _characterHeight = 0.6f;
	Transform _targetTransform;
	PlayerController _target;
	Vector3 offset;
	Vector3 _targetPosition;

	IHealthUser _targetHealth;

	public void SetTarget (PlayerController target) {
		if (target == null)
			return;

		_target = target;
		if (PlayerName != null)
			PlayerName.text = _target.photonView.Owner.NickName;

		_targetTransform = _target.transform;

		_targetHealth = _target.gameObject.GetComponent<IHealthUser> ();
	}

	void Awake () {
		this.GetComponent<Transform> ().SetParent (GameObject.Find ("UI").GetComponent<Transform> ());
		offset = new Vector3 (0f, offsetY, 0f);
	}

	void Update () {
		if (_target == null || _target.photonView.IsMine) {
			Destroy (this.gameObject);
			return;
		}

		PlayerHealth.fillAmount = 0.5f * (float)_targetHealth.CharacterHP.HP / _targetHealth.CharacterHP.MaxHP;
	}

	void LateUpdate () {
		if (_targetTransform != null) {
			_targetPosition = _targetTransform.position;
			_targetPosition.y += _characterHeight;
			Vector3 ui = Vector3.zero;
			if (Camera.main != null) {
				ui = Camera.main.WorldToScreenPoint (_targetPosition);
			} 
			if (ui.z < 0) {
				PlayerName.enabled = false;
				PlayerHealth.enabled = false;
			} else {
				PlayerName.enabled = true;
				PlayerHealth.enabled = true;

				this.transform.position = ui + offset;
			}
		}
	}
}