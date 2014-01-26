﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TileMap: MonoBehaviour {

	public TileLayer[] layers { 
		get {
			return this.gameObject.GetComponentsInChildren<TileLayer>();
		}
	}

	[SerializeField]
	public List<Tileset> tilesets = new List<Tileset>();

	public int sortingOrder = 1;
	public string sortingLayer = "";

	public int mapWidth = 256;
	public int mapHeight = 256;
	public int tileWidth = 32;
	public int tileHeight = 32;
	public int pixelPerUnit = 100;

	public void Awake() {
		foreach(Tileset ts in this.tilesets) {
			ts.CreateTileSprites();
		}
	}

	public TileLayer addLayer(LayerType t, string name) {
		GameObject obj = new GameObject();
		obj.transform.parent = this.gameObject.transform;
		obj.name = name;

		TileLayer l = obj.AddComponent<TileLayer>();
		l.parentMap = this;
		l.layerType = t;
		l.name = name;
		l.init(this.width, this.height);

		return l;
	}

	public void RemoveLayer(TileLayer t) {
		DestroyImmediate(t.gameObject);
	}

	public Vector3 startPoint {
		get {
			return this.gameObject.transform.position - new Vector3((float)this.mapWidth / this.pixelPerUnit / 2,
			                                                        (float)this.mapHeight / this.pixelPerUnit / 2);
		}
	}

	public float xStep {
		get {
			return (float)this.tileWidth / this.pixelPerUnit; 
		}
	}

	public float yStep {
		get {
			return (float)this.tileHeight / this.pixelPerUnit;
		}
	}

	public float worldWidth {
		get {
			return (float)this.mapWidth / this.pixelPerUnit;
		}
	}

	public float worldHeight {
		get {
			return (float)this.mapHeight / this.pixelPerUnit;
		}
	}

	public Rect worldMapRect {
		get {
			Vector3 sp = this.startPoint;
			return new Rect(sp.x, sp.y, this.worldWidth, this.worldHeight);
		}
	}

	public int width {
		get {
			return this.mapWidth / this.tileWidth;
		}
	}

	public int height {
		get {
			return this.mapHeight / this.tileHeight;
		}
	}

	public SpriteRenderer previewTile {
		get {
			TilePreview tp = this.gameObject.GetComponentInChildren<TilePreview>();
			if(tp == null) {
				GameObject previewObj = new GameObject();
				previewObj.transform.parent = this.gameObject.transform;
				previewObj.name = "PreviewTile";
				tp = previewObj.AddComponent<TilePreview>();
				SpriteRenderer sr = previewObj.AddComponent<SpriteRenderer>();
				sr.sortingOrder = 20;
			}
			return tp.gameObject.GetComponent<SpriteRenderer>();
		}
	}

}