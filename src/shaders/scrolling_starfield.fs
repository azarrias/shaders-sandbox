#define PI 3.14159265358979323846
extern vec2 resolution;
extern float time;

mat2 Rotate(float a) {
  float s = sin(a), c = cos(a);
  return mat2(c, -s, s, c);
}

// pseudo-random number generator, 
// takes 2 inputs, returns 1 value (0 to 1)
float Hash(vec2 p) {
  p = fract(p * vec2(123.34, 456.21));
  p += dot(p, p + 45.32);
  return fract(p.x * p.y);                    
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
  star *= smoothstep(1., .2, dc);             // fade out star, remove long rotated sparkle
  
  return star;
}

vec4 effect(vec4 color, Image texture, vec2 texture_coords, vec2 screen_coords) {
  vec2 uv = screen_coords / resolution - .5;  // normalize uv
  uv.x *= resolution.x / resolution.y;        // compensate for aspect ratio
  uv *= 3.;                                   // zoom out
  
  vec3 col = vec3(0.05, 0.05, 0.05);          // background color

  vec2 gv = fract(uv);                        // grid uv with fractional component of uv
  gv -= .5;                                   // remap origin to center for each cell
  vec2 id = floor(uv);                        // cell id, integer components of uv
  
  for (int y = -1; y <= 1; y++) {             // iterate through cell neighbors to get
    for (int x = -1; x <= 1; x++) {           // light contribution from one cell to the other
      vec2 cell_offset = vec2(x, y);          // cell offset to the neighbor
      float rand_x = Hash(id + cell_offset);  // random num from 0 to 1
      float rand_y = fract(rand_x * 34.);
      rand_x -= .5;                           // range random num from -.5 to .5
      rand_y -= .5;      
      
      col += Star(gv - cell_offset - vec2(rand_x, rand_y), 1.) + time * 0.000000000001;
    }
  }
 
  return vec4(col, 1);              // use the same c value for r, g, b
}
