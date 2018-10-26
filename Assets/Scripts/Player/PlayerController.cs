using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CharacterMotor;
using CharacterHand;
using IManager;

using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IMotorUser, IHandUser {

	[Tooltip("Local player instance")]
	public static GameObject LocalPlayerInstance;

	public PhotonView photonView;

	[Tooltip("PLayer height, used for ground check")]
	public float height = 0.6f;
	[Tooltip("Camera rotation speed")]
	public float cameraSpeed = 5f;
	[Tooltip("Do we want inverted camera Y axis?")]
	public bool invertY = true;
	[Tooltip("Which layers do we consider solid ground?")]
	public LayerMask Ground;
	[Tooltip("Reference to a name UI above player's head")]
	public GameObject playerNameUI;

	Charger Dasher;

	MenuState Menu;
	GameObject MenuScreen;

	float pitch;
	float yaw;

	public Motor CharacterPosition { get; set; }
	public Motor LastPosition { get; set; }

	MotorPrefs _motorprefs = new MotorPrefs {
		speed = 5f,
		jumpHeight = 3f
	};
	public MotorPrefs MPrefs { get { return _motorprefs; } }

	public Hand CharacterHolding { get; set; }
	public InputManager iManager { get; set; }

	void Awake () {
		photonView = GetComponent<PhotonView> ();
		if (photonView.IsMine)
			LocalPlayerInstance = this.gameObject;
		DontDestroyOnLoad (this.gameObject);
	}

	void Start () {
		SetCamera ();
		SetNameUI ();
		if (photonView.IsMine) {
			iManager = InputManager.Instance;
			SetInputCommands ();
			CharacterPosition = new Falling (this);
			CharacterHolding = new EmptyHand (this);
			MenuScreen = GameObject.FindGameObjectWithTag ("InGameMenu");
			Menu = new MenuState (MenuScreen);
			Dasher = gameObject.GetComponent<Charger> ();
			if (Dasher == null)
				Dasher = gameObject.AddComponent<Charger> ();
		}
	}

	void Update () {
		if (!photonView.IsMine && PhotonNetwork.IsConnected)
			return;
		
		iManager.HandleInput ();
		if (Menu.InGame) {
			CameraMovement ();
			Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")).normalized;
			CharacterPosition.Walk (input);
		}
		CharacterPosition.Fall ();
		CharacterPosition.Transition (IsGrounded);
	}

	void SetInputCommands () {
		iManager.Init ();
		iManager.AddCommand (KeyCode.Space, JumpCommand);
		iManager.AddCommand (KeyCode.LeftShift, BlinkCommand);
		iManager.AddCommand (KeyCode.M, MenuCommand);
		iManager.AddCommand (KeyCode.Mouse0, InteractCommand);
		iManager.AddCommand (KeyCode.Mouse1, AltInteractCommand);
	}

	public void JumpCommand () {
		if (Menu.InGame)
			CharacterPosition.Jump ();
	}

	public void BlinkCommand () {
		if (Menu.InGame && (Dasher.Charges > 0)) {
			CharacterPosition.Blink ();
			photonView.RPC("PlayDashAudio", RpcTarget.All);
			Dasher.Use();
		}
	}

	public void MenuCommand () {
		Menu.Toggle ();
	}

	public void InteractCommand () {
		if (Menu.InGame)
			CharacterHolding.Interact ();
	}

	public void AltInteractCommand () {
		if (Menu.InGame)
			CharacterHolding.AltInteract ();
	}

	void CameraMovement () {
		pitch += invertY ? (cameraSpeed * Input.GetAxis ("Mouse Y") * -1) : (cameraSpeed * Input.GetAxis ("Mouse Y"));
		pitch %= 360f;
		yaw += cameraSpeed * Input.GetAxis ("Mouse X");
		yaw %= 360f;
		pitch = Mathf.Clamp (pitch, -89.998f, 89.998f);
		transform.eulerAngles = new Vector3 (pitch, yaw, 0f);
	}

	bool IsGrounded () {
		Vector3 pos = transform.position;
		Vector3[] pivots = {
			new Vector3 (pos.x - 0.5f, pos.y, pos.z + 0.5f),
			new Vector3 (pos.x + 0.5f, pos.y, pos.z + 0.5f),
			new Vector3 (pos.x - 0.5f, pos.y, pos.z - 0.5f),
			new Vector3 (pos.x + 0.5f, pos.y, pos.z - 0.5f),
			new Vector3 (pos.x, pos.y, pos.z)
		};

		foreach (Vector3 p in pivots) {
			if (Physics.Raycast (p, Vector3.down, height, Ground))
				return true;
		}
		return false;
	}

	void SetCamera () {
		GameObject camera = transform.Find ("Main Camera").gameObject;
		if (photonView.IsMine)
			camera.tag = "MainCamera";
		else
			Destroy (camera);
	}

	void SetNameUI () {
		GameObject uiG = Instantiate (playerNameUI) as GameObject;
		uiG.GetComponent<PlayerNameDisplay> ().SetTarget (this);
	}

	[PunRPC]
	void PlayDashAudio () {
		AudioSource source = GetComponent<AudioSource> ();
		if (source == null)
			source = gameObject.AddComponent<AudioSource> ();
		string name = "teleports-behind-you";
		AudioClip sound = Resources.Load ("Sounds/" + name, typeof(AudioClip)) as AudioClip;
		System.Random random = new System.Random ();
		source.pitch = 1f + ((float)random.NextDouble() - 0.5f)/5;
		source.clip = sound;
		source.loop = false;
		source.Play ();
	}
}