#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texCoord;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 transform;

out vec2 texCoordOut;

void main()
{
	//mat4 projectedPosition = projection * transform;
	//gl_Position = projectedPosition * vec4(position, 1.0);
	texCoordOut = texCoord;
	gl_Position = vec4(position, 1.0) * transform * view * projection;
}