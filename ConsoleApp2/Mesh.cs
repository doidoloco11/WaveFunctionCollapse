using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography.X509Certificates;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace ConsoleApp2;

public class Mesh
{
    private int Vao;
    private int VerticesBuffer;
    private int UVBuffer;
    private int Texture;

    public Mesh()
    {
        Vao = GL.GenVertexArray();
        VerticesBuffer = GL.GenBuffer();
        UVBuffer = GL.GenBuffer();

        Texture = GL.GenTexture();
        
        GL.BindVertexArray(Vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, 32, new Vector2[]
        {
            new Vector2(-0.5f, -0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(-0.5f, 0.5f),
            new Vector2(0.5f, 0.5f)
        }, BufferUsage.StaticDraw);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, UVBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, 32, new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        }, BufferUsage.StaticDraw);
    }

    public void SetTexture(BitmapData data, int size)
    {
        GL.BindTexture(TextureTarget.Texture2d, Texture);
        
        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba8, size, size, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
        
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
    }

    public void Render(Vector2 position, Vector2 size)
    {
        Matrix4 m_model = Matrix4.CreateScale(new Vector3(size, 1));
        m_model *= Matrix4.CreateTranslation(new Vector3(position + new Vector2(size.X / 2, size.Y), 0));
        
        Shaders.TileShader.Use();
        
        GL.BindVertexArray(Vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VerticesBuffer);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, UVBuffer);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
        
        GL.UniformMatrix4f(Shaders.TileShader.GetUniformID("m_model"), 1, false, ref m_model);
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d, Texture);
        
        GL.Uniform1i(Shaders.TileShader.GetUniformID("main_tex"), 0);
        
        GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
    }
}