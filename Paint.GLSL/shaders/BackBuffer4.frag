uniform float time;
uniform vec2 resolution;
uniform sampler2D texture;
uniform vec2 mouse; 
uniform float size;


void main( void ) {

	vec2 p =  gl_FragCoord.xy -vec2(mouse.x,resolution.y-mouse.y);

	p*=size;


	float xx = 0.0;
	
	float k = time*.07;
	
	for(float i=0.0;i<100.0;i+=1.0)
		xx+=(1.6*i)/length(vec2(p.x-i*100*cos(i*time)*cos(i),p.y-i*100*sin(i*time)*cos(i))); 



	vec4 color = vec4( xx*sin(cos(time)),xx*sin(xx*time),xx*sin(time), 1.0 );
	
	
	vec2 coord = gl_TexCoord[0].st;
	
    vec4 backpixel = texture2D(texture,  vec2(coord.x,1.0-coord.y));
	
	color = max(color,backpixel);

	//color-=0.002;
	
	gl_FragColor = color;
}