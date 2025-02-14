using OpenTK.Mathematics;

namespace ConsoleApp2;

public class Cell
{
    public Tile[] tiles;

    public bool Colapsed = false;
    public bool[] Checked = new []{false, false, false, false};

    public Cell(Tile[] tiles)
    {
        this.tiles = tiles;
    }

    public bool contain(int id, int side)
    {
        bool f = false;
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].constraint[side].Contains(id))
            {
                f = true;
            }
        }

        return f;
    }

    public void render(Vector2 position, Vector2 size)
    {
        if (Colapsed)
        {
            tiles[0].Render(position, size);
        }

        for (int i = 0; i < 4; i++)
        {
            Checked[i] = false;
        }
    }
}