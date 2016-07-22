uniform float time;
uniform vec2 resolution;
uniform sampler2D texture;
uniform vec2 mouse; 
uniform float size;
uniform vec4 input_color;


void main( void ) {

	vec2 p =  gl_FragCoord.xy -vec2(mouse.x,resolution.y-mouse.y);

	p*=size*0.01;


	float xx = 0.0;
	
	float k = time*.07;
	

	xx+=(1.6)/length(vec2(p.x,p.y)); 



	vec4 color = vec4( xx*sin(cos(time)),xx*cos(time*xx),xx*sin(time), 1.0 );
	
	
	color =  input_color*color;

	vec2 coord = gl_TexCoord[0].st;
	
    vec4 backpixel = texture2D(texture,  vec2(coord.x,1.0-coord.y));
	
	color = max(color,backpixel);

	//color-=0.002;
	
	gl_FragColor = color;
}