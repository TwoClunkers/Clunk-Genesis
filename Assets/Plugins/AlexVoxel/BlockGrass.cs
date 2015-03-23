using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockGrass : Block
{

    public BlockGrass()
        : base()
    {
		material = 7;
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 2;
                tile.y = 7;
                return tile;
            case Direction.down:
                tile.x = 1;
                tile.y = 7;
                return tile;
        }

        tile.x = 0;
        tile.y = 7;

        return tile;
    }
}