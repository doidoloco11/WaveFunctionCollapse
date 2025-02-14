using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Mathematics;

namespace ConsoleApp2;

public class Tile
{
    public static int TileSize = 1;

    public static int UP = 0;
    public static int RIGHT = 1;
    public static int DOWN = 2;
    private static int LEFT = 3;
    
    private Tile[] tiles;
    public Bitmap image;
    public int index;
    private int size;
    public Dictionary<int, List<int>> constraint;

    private Mesh mesh;

    public Tile(int index, Bitmap img, ref Tile[] tiles)
    {
        this.index = index;
        this.tiles = tiles;
        size = TileSize * 2 + 1;
        image = new Bitmap(size, size, img.PixelFormat);
        constraint = new Dictionary<int, List<int>>();
        
        constraint.Add(UP, new List<int>());
        constraint.Add(DOWN, new List<int>());
        constraint.Add(RIGHT, new List<int>());
        constraint.Add(LEFT, new List<int>());

        mesh = new Mesh();
        
        int xx = index % img.Width;
        int yy = (int)(index / img.Width);

        for (int dx = -TileSize; dx < TileSize + 1; dx++)
        {
            for (int dy = -TileSize; dy < TileSize + 1; dy++)
            {
                int x = (xx + dx) % img.Width;
                int y = (yy + dy) % img.Height;

                if (x < 0)
                {
                    x += img.Width;
                }

                if (y < 0)
                {
                    y += img.Height;
                }

                int i = (dx+1 + (dy+1) * 3) * 4;
                
                Color color = img.GetPixel(x, y);
                image.SetPixel(dx + TileSize, dy + TileSize, color);
                
            }
        }

        BitmapData data = image.LockBits(new Rectangle(0, 0, 3, 3), ImageLockMode.ReadOnly, image.PixelFormat);
        mesh.SetTexture(data, size);
        
        image.UnlockBits(data);
    }

    public void GetNeighbors()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            bool combine = true;
            for (int x = 1; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Tile othertile = tiles[i];

                    Color othercolor = othertile.image.GetPixel(x - 1, y);
                    Color thiscolor = image.GetPixel(x, y);
                    if (othercolor != thiscolor)
                    {
                        combine = false;
                        break;
                    }
                }

                if (!combine) break;
            }
            if (combine) constraint[RIGHT].Add(i);
        }
        
        for (int i = 0; i < tiles.Length; i++)
        {
            bool combine = true;
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Tile othertile = tiles[i];

                    Color othercolor = othertile.image.GetPixel(x + 1, y);
                    Color thiscolor = image.GetPixel(x, y);
                    if (othercolor != thiscolor)
                    {
                        combine = false;
                        break;
                    }
                }

                if (!combine) break;
            }
            if (combine) constraint[LEFT].Add(i);
        }
        
        for (int i = 0; i < tiles.Length; i++)
        {
            bool combine = true;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 1; y < 3; y++)
                {
                    Tile othertile = tiles[i];

                    Color othercolor = othertile.image.GetPixel(x, y - 1);
                    Color thiscolor = image.GetPixel(x, y);
                    if (othercolor != thiscolor)
                    {
                        combine = false;
                        break;
                    }
                }

                if (!combine) break;
            }
            if (combine) constraint[UP].Add(i);
        }
        
        for (int i = 0; i < tiles.Length; i++)
        {
            bool combine = true;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    Tile othertile = tiles[i];

                    Color othercolor = othertile.image.GetPixel(x, y + 1);
                    Color thiscolor = image.GetPixel(x, y);
                    if (othercolor != thiscolor)
                    {
                        combine = false;
                        break;
                    }
                }

                if (!combine) break;
            }
            if (combine) constraint[DOWN].Add(i);
        }
    }

    public void Render(Vector2 position, Vector2 size)
    {
        mesh.Render(position, size);
    }
}