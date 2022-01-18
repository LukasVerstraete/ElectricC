#version 330 core
out vec4 color;

in vec2 texCoordOut;
uniform sampler2D texture0;

void main()
{
	color = texture(texture0, texCoordOut);
}