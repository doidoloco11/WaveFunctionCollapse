using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Random = System.Random;

namespace ConsoleApp2;

public class Window : GameWindow
{
    public static Window window;
    public static int Tiles = 50;

    private Tile[] tiles;
    private Cell[,] cells;
    private Camera camera;
    
    public Window() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        
    }

    void GetTiles(string path)
    {
        Bitmap img = new Bitmap(path);

        tiles = new Tile[img.Width * img.Height];
        
        for (int x = 0; x < img.Width; x++)
        {
            for (int y = 0; y < img.Height; y++)
            {
                int index = x + y * img.Width;
                Tile tile = new Tile(index, img, ref tiles);
                tiles[index] = tile;
            }
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetNeighbors();
        }

        cells = new Cell[Tiles, Tiles];

        for (int x = 0; x < Tiles; x++)
        {
            for (int y = 0; y < Tiles; y++)
            {
                cells[x, y] = new Cell(tiles);
            }
        }
    }
    
    
    
    protected override void OnLoad()
    {
        base.OnLoad();

        Size = new Vector2i(600, 600);
        
        window = this;
        
        camera = new Camera();
        
        GetTiles("../../../Imagens/City.png");

        /*while (wfc())
        {
            
        }*/
    }

    bool wfc()
    {
        int i = Int32.MaxValue;
        bool AllColapsed = true;

        List<Vector2i> poss = new List<Vector2i>();
        
        for (int x = 0; x < Tiles; x++)
        {
            for (int y = 0; y < Tiles; y++)
            {
                int j = cells[x, y].tiles.Length;

                if (!cells[x, y].Colapsed)
                {
                    AllColapsed = false;

                    if (j < i)
                    {
                        i = j;
                        poss.Clear();
                    }

                    if (i == j)
                    {

                        poss.Add(new Vector2i(x, y));
                    }
                }
            }
        }

        if (AllColapsed)
        {
            return false;
        }
        Vector2i pos = poss[new Random().Next(0, poss.Count)];
        if (cells[pos.X, pos.Y].tiles.Length == 0)
        {
            for (int x = 0; x < Tiles; x++)
            {
                for (int y = 0; y < Tiles; y++)
                {
                    cells[x, y] = new Cell(tiles);
                }
            }
        }
        cells[pos.X, pos.Y].tiles = new []{cells[pos.X, pos.Y].tiles[new Random().Next(0, cells[pos.X, pos.Y].tiles.Length)]};
        cells[pos.X, pos.Y].Colapsed = true;

        int depth = 5;
        
        constraintValues(pos, 0, depth);
        constraintValues(pos, 1, depth);
        constraintValues(pos, 2, depth);
        constraintValues(pos, 3, depth);

        return true;
    }

    void constraintValues(Vector2i pos, int side, int depth)
    {
        Vector2i[] dir = new[] { new Vector2i(0, 1), new Vector2i(1, 0), new Vector2i(0, -1), new Vector2i(-1, 0) };
        Vector2i npos = pos + dir[side];

        if (depth == 0)
        {
            return;
        }
        
        if (npos.X < 0 || npos.X >= Tiles || npos.Y < 0 || npos.Y >= Tiles)
        {
            return;
        }

        if (cells[npos.X, npos.Y].Colapsed || cells[npos.X, npos.Y].Checked[side])
        {
            return;
        }

        List<Tile> newtiles = new List<Tile>();
        
        for (int i = 0; i < cells[npos.X, npos.Y].tiles.Length; i++)
        {
            Tile otile = cells[npos.X, npos.Y].tiles[i];
            if (cells[pos.X, pos.Y].contain(otile.index, side))
            {
                newtiles.Add(otile);
            }
        }

        cells[npos.X, npos.Y].tiles = newtiles.ToArray();

        cells[pos.X, pos.Y].Checked[side] = true;

        constraintValues(npos, 0, depth - 1);
        constraintValues(npos, 1, depth - 1);
        constraintValues(npos, 2, depth - 1);
        constraintValues(npos, 3, depth - 1);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        Title = (1f / UpdateTime).ToString();
        camera.update();
        wfc();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        
        GL.ClearColor(0, 0, 0, 1);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        for (int x = 0; x < Tiles; x++)
        {
            for (int y = 0; y < Tiles; y++)
            {
                int px = (800 / Tiles) * x;
                int py = (800 / Tiles) * y;
                
                cells[x, y].render(new Vector2(px, py), new Vector2(800/Tiles));
            }
        }
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        
        GL.Viewport(0, 0, Size.X, Size.Y);
    }
}
