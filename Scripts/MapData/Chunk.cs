using System;
using Godot;

public class Chunk
{
    
	private short[] tiles;

	private bool isSaved = true;
	
	private bool render = false;

	private Vector2 position;
	
	public Chunk(short[] tiles, int x, int y) {
		this.tiles = tiles;
		position = new Vector2(x, y);
	}

	public short getTile(int x, int y) {
		
		if(x <= -1) {
			x += MapCore.CHUNK_WIDTH;
		}
		
		if(y <= -1) {
			y += MapCore.CHUNK_WIDTH;
		}
		
		GD.Print("Tile in [" + x + "," + y + "]");
		
		int index = x + (y * MapCore.CHUNK_WIDTH);
		
		return tiles[index];
	}
	
	public void setTile(int x, int y, short value) {
		
		if(x <= -1) {
			x += MapCore.CHUNK_WIDTH;
		}
		
		if(y <= -1) {
			y += MapCore.CHUNK_WIDTH;
		}
		
		GD.Print("Tile in [" + x + "," + y + "]");
		
		int index = x + (y * MapCore.CHUNK_WIDTH);
		
		tiles[index] = value;

		isSaved = false;

		UpdateGrapichs();

	}
	
	public void setSaved() {
		isSaved = true;
	}
	
	public short[] getTiles() {
		return tiles;
	}

	public bool getSaved() {
		return isSaved;
	}
	
	public bool getRender() {
		return render;
	}
	
	public Vector2 getPosition() {
		return position;
	}

	public void setPosition(Vector2 position) {
		this.position = position;
	}
	
	public void createModel() {

		UpdateGrapichs();
		
		render = true;
		
	}
	
    public void UpdateGrapichs() {

		MeshingManager.getToDoMapModelsList().Add(this);

	}

	public void destroyModel() {
        MeshingManager.getToDeleteModelsList().Add(this);
		render = false;
	}
	
}
