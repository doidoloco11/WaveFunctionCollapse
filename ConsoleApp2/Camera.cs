using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ConsoleApp2;

public class Camera
{
    public Camera()
    {
        
    }

    public void update()
    {
        Shaders.TileShader.Use();

        Matrix4 m_proj = Matrix4.CreateOrthographicOffCenter(0, 800, 800, 0, 0, 1);
        GL.UniformMatrix4f(Shaders.TileShader.GetUniformID("m_proj"), 1, false, ref m_proj);
    }
}