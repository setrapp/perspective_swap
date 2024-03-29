using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject playerCharacter = null;
	private CharacterInfo characterInfo = null;
	public TileMap tileMap;
	private float tileDestEpsilon = 0.00005f;

	public int lifeCount = 3;
	
	public int currentTileX, currentTileY;
	public int targetTileX, targetTileY;
	public int tileMoveX, tileMoveY;
	
	void Start () {
		characterInfo = playerCharacter.GetComponent<CharacterInfo>();
		if (characterInfo ==  null) {
			enabled = false;
		}
		if(tileMap != null)
			tileMap.getTileCoordinateAt(playerCharacter.transform.position, ref currentTileX, ref currentTileY);
		targetTileX = currentTileX;
		targetTileY = currentTileY;
		tileMap.showLayerInGroupWithTag(characterInfo.tag, 1, true);
	}

	void Update () {
		CheckInput();
	}

	void CheckObjectInteraction() {
		bool adjacentSpecial;
		Tile frontTile = tileMap.getTile(currentTileX + tileMoveX, currentTileY + tileMoveY, characterInfo.tileLayer);
		if(characterInfo.interactionBubble != null)
			characterInfo.interactionBubble.SetActive(false);
		var distance1 = new Vector2( currentTileX - frontTile.x, currentTileY - frontTile.y);

		switch (characterInfo.occupation) {
		case CharacterInfo.Occupation.BREAK:
			if(frontTile.isBreakable) {
				characterInfo.interactionBubble.SetActive(true);
				if (Input.GetAxis("Touch") != 0) {
					TileHolder.Instance.SwapInNewTile(0, currentTileX + tileMoveX, currentTileY + tileMoveY);
					Tile fixedTile = tileMap.getTile(currentTileX + tileMoveX, currentTileY + tileMoveY, characterInfo.tileLayer);
					fixedTile.isBlock = false;
					fixedTile.isBreakable = false;
					fixedTile.isClimbable = false;
					fixedTile.isTunnelable = false;
					fixedTile.isMoveable = false;
				}
			}
			break;
		case CharacterInfo.Occupation.CLIMB:
			if(frontTile.isClimbable) {
				
			}
			break;
		case CharacterInfo.Occupation.FLOAT:
			if(frontTile.isFloatable) {
				playerCharacter.transform.Translate(distance1 * Time.deltaTime);
			}
			break;
		case CharacterInfo.Occupation.MOVE:
			if(frontTile.isMoveable) {
				
			}
			break;
		case CharacterInfo.Occupation.TUNNEL:
			if(frontTile.isTunnelable) {
				
			}
			break;
		}

		string isEnd = tileMap.getAttributeAt(currentTileX, currentTileY, "isend");
		if(isEnd.Length > 0) {
			if(!GameStatus.ended) {
				iTween.CameraFadeAdd();
				iTween.CameraFadeTo(iTween.Hash("amount", 1.0f, "time", 20f/*, "oncomplete", "OnCamFadeComplete", "oncompletetarget", this.gameObject*/));
				GameStatus.ended = true;

				foreach(CharacterInfo character in CharacterManager.Instance.characters) {
					AIController npc = character.gameObject.GetComponent<AIController>();
					if(npc != null) {
						npc.characterInfo.moveSpeed = 2;
						npc.nagivateTo(currentTileX, currentTileY, () => {
							GameStatus.PeopleSaved += 1;
						});
						npc.wanderRadius = 0;
						npc.enableWandering = false;
					}
				}
			}
		}
	}

	void OnCamFadeComplete() {
		iTween.CameraFadeTo(iTween.Hash("amount", -1.0f, "time", 0.5f));
	}

	bool isClose(Vector3 p1, Vector3 p2) {
		return Mathf.Abs (p2.x - p1.x) < 0.00005f &&
			Mathf.Abs (p2.y - p1.y) < 0.00005f;
	}

	private void CheckInput() {
		if(tileMap != null)  {
			int tx = targetTileX, ty = targetTileY;
			bool closeToDest = isClose (this.playerCharacter.transform.position, tileMap.getTile(targetTileX, targetTileY, characterInfo.tileLayer).gameObject.transform.position);
			if(targetTileY == currentTileY && closeToDest) {
				if(Input.GetAxis("Horizontal") < 0) {
					tx = (int)Mathf.Clamp(currentTileX - 1, 0, 9999);
					tileMoveX = -1;
					tileMoveY = 0;
				} else if(Input.GetAxis("Horizontal") > 0) {
					tx = (int)Mathf.Clamp(currentTileX + 1, 0, tileMap.width - 1);
					tileMoveX = 1;
					tileMoveY = 0;
				}
			}
			if (targetTileX == currentTileX && closeToDest) {
				if(Input.GetAxis("Vertical") > 0) {
					ty = (int)Mathf.Clamp(currentTileY + 1, 0, 9999);
					tileMoveY = 1;
					tileMoveX = 0;
				} else if(Input.GetAxis("Vertical") < 0) {
					ty = (int)Mathf.Clamp(currentTileY - 1, 0, tileMap.height - 1);
					tileMoveY = -1;
					tileMoveX = 0;
				}
			}
			Tile t = tileMap.getTile(tx, ty, 0);
			if(!tileMap.isBlockAt(tx, ty)) {
				targetTileX = tx;
				targetTileY = ty;

				AIController.MoveTo(t.gameObject.transform.position, characterInfo.moveSpeed, playerCharacter);
				tileMap.getTileCoordinateAt(playerCharacter.transform.position, ref currentTileX, ref currentTileY);
			}

			CheckObjectInteraction();

			///	Debug.Log (tileMap.getTileAt(transform.position, 0));
		} else {
			playerCharacter.transform.Translate(characterInfo.moveSpeed * Input.GetAxis("Horizontal"), 0, 0);
			playerCharacter.transform.Translate(0, characterInfo.moveSpeed * Input.GetAxis("Vertical"), 0);
		}
	}
}
