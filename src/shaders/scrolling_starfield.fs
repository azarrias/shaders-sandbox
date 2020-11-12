#define PI 3.14159265358979323846
extern vec2 resolution;
extern float time;

mat2 Rotate(float a) {
  float s = sin(a), c = cos(a);
  return mat2(c, -s, s, c);
}

float Star(vec2 uv, float flare) {
  float dc = length(uv);                      // distance to center
  float star = .05 / dc;                      // asymptote for star glow
    
  float sparkle = 1. - abs(uv.x * uv.y * 1000); // star sparkle
  sparkle = max(sparkle, 0);                  // avoid it to subtract color from the star
  star += sparkle * flare;
  
  uv *= Rotate(PI / 4.);                      // rotate uvs by 45 degrees
  sparkle = 1. - abs(uv.x * uv.y * 1000);     // rotated star sparkle
  sparkle = max(sparkle, 0);                  // avoid it to subtract color from the star
  star += sparkle * flare * .3;               // make it less bright
  star *= smoothstep(.5, .2, dc);             // fade out star, remove long rotated sparkle
  
  return star;
}

vec4 effect(vec4 color, Image texture, vec2 texture_coords, vec2 screen_coords) {
  vec2 uv = screen_coords / resolution - .5;  // normalize uv
  uv.x *= resolution.x / resolution.y;        // compensate for aspect ratio
  uv *= 3.;                                   // zoom out
  vec3 col = vec3(0.05, 0.05, 0.05);          // background color
  
  col += Star(uv, 1.) + time * 0.000000000001;
  
  return vec4(col, 1);              // use the same c value for r, g, b
}
