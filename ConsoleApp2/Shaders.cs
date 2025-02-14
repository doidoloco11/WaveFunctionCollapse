using OpenTK.Graphics.OpenGL;

namespace ConsoleApp2;

public class Shaders
{
    public static Shader TileShader = new Shader("../../../Shaders/Tile");
}

public struct Shader
{
    private int id;
    private Dictionary<string, int> uniformIDs;

    public Shader(string path)
    {
        uniformIDs = new Dictionary<string, int>();
        CreateProgram(path);
    }
    
    private int CreateShader(string path, ShaderType type)
    {
        int id = GL.CreateShader(type);
        GL.ShaderSource(id, File.ReadAllText(path));
        GL.CompileShader(id);

        GL.GetShaderInfoLog(id, out string info);
        if (!String.IsNullOrEmpty(info))
        {
            throw new Exception(info);
        }

        return id;
    }

    private void CreateProgram(string path)
    {
        id = GL.CreateProgram();
        int vert = CreateShader(path + ".vert", ShaderType.VertexShader);
        int frag = CreateShader(path + ".frag", ShaderType.FragmentShader);
        
        GL.AttachShader(id, vert);
        GL.AttachShader(id, frag);
        
        GL.LinkProgram(id);
        
        GL.DetachShader(id, vert);
        GL.DetachShader(id, frag);

        GL.GetProgramInfoLog(id, out string info);
        if (!String.IsNullOrEmpty(info))
        {
            throw new Exception(info);
        }
    }

    public int GetUniformID(string UniformName)
    {
        if (!uniformIDs.ContainsKey(UniformName))
        {
            uniformIDs.Add(UniformName, GL.GetUniformLocation(id, UniformName));
        }

        return uniformIDs[UniformName];
    }

    public void Use()
    {
        GL.UseProgram(id);
    }
}