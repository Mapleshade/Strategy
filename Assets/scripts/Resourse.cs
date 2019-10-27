using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resourse {

	int type;
	bool isActive = false;
	Vector2 coordinate;
	Grid grid;

	public Resourse(int type, bool isActive, int x, int y, Grid grid) {
		this.type = type;
		this.isActive = isActive;
		coordinate = new Vector2 (x, y);
		this.grid = grid;
	}

	public int GetTypeRes() {
		return type;
	}

	public bool GetActive() {
		return isActive;
	}

	public void Activate() {
		isActive = true;
		switch(type) {
		case -1:
		case -4:
			type = -7;
			MapInfo.current.prefMap [(int)coordinate.x, (int)coordinate.y] = -7;
			break;
		case -2:
		case -5:
			type = -8;
			MapInfo.current.prefMap [(int)coordinate.x, (int)coordinate.y] = -8;
			break;
		case -3:
		case -6:
			type = -9;
			MapInfo.current.prefMap [(int)coordinate.x, (int)coordinate.y] = -9;
			break;
		}
			
		grid.ActivateResource ((int)coordinate.x, (int)coordinate.y, type);
	}
}
