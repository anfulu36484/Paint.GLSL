uniform sampler2D texture;


void main( void ) {
	vec2 coord = gl_TexCoord[0].st;
	
    vec4 backpixel = texture2D(texture,  vec2(coord.x,coord.y));
	
	
	gl_FragColor = backpixel;
}