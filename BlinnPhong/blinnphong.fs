#version 330 core
out vec4 FragColor;

in VS_OUT {
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
} fs_in;

uniform sampler2D floorTexture;
uniform vec3 lightPos;
uniform vec3 viewPos;
uniform bool blinn;

void main(){
	vec3 color = texture(floorTexture,fs_in.TexCoords).rgb;

	vec3 lightDir = normalize(lightPos - fs_in.FragPos);
	vec3 viewDir = normalize(viewPos - fs_in.FragPos);
	vec3 normal = normalize(fs_in.Normal);
	float diff = max(dot(lightDir,normal),0);

	float spec = 0.0f;
	if(blinn){
		vec3 halfwayDir = normalize(lightDir+viewDir);
		spec = pow(max(dot(halfwayDir,normal),0.0f),32);
	}else{
		vec3 reflectDir = reflect(-lightDir,normal);
		spec = pow(max(dot(viewDir,reflectDir),0.0f),8);
	}
	vec3 ambient = vec3(0.05) * color;
	vec3 diffuse = vec3(1.0) * diff * color;
	vec3 specular = vec3(0.3) * spec;
	vec3 result = ambient + diffuse + specular;
	FragColor = vec4(result , 1.0);

}