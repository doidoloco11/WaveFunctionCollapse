#version 330

out vec4 frag_color;

uniform sampler2D main_tex;

in vec2 uv;

void main() {
    
    frag_color = texture(main_tex, uv/3 + 1/3);
    
    /*vec2 vu = fract(uv.yx * 3);

    if (vu.x < 0.01 || vu.x > 0.99 || vu.y < 0.01 || vu.y > 0.99){
        frag_color = vec4(0, 0, 1, 1);
    }
    
    if (uv.x < 0.01 || uv.x > 0.99 || uv.y < 0.01 || uv.y > 0.99){
        frag_color = vec4(0, 0, 0, 1);
    }*/
    
    //frag_color = vec4(uv, 0, 1);
}