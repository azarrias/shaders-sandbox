extern vec2 resolution;
extern float time;
#define NUM_LAYERS 6

mat2 Rot(float a)
{
  float s = sin(a), c = cos(a);
  return mat2(c, -s, s, c);       // 2D rotation matrix
}

float Star(vec2 uv, float flare)
{
  float d = length(uv);
  float star = .05 / d;                      // shining light effect
  
  float rays = max(0., 1. - abs(uv.x * uv.y * 3000.));   // star horizontal vertical rays
  star += rays * flare;
  
  uv *= Rot(3.1415 / 4.);           // 45 degree rotation
  rays = max(0., 1. - abs(uv.x * uv.y * 3000.));   // star rotated rays
  star += rays * .3 * flare;
  star *= smoothstep(1, .2, d);
  
  return star;
}

float Hash21(vec2 p)
{
  p = fract(p * vec2(123.34, 456.21));
  p += dot(p, p + 45.32);
  return fract(p.x * p.y);
}

vec3 StarLayer(vec2 uv) 
{
  vec3 col = vec3(0);
  vec2 gv = fract(vec2(uv.x, uv.y)) - .5;         // grid uv, put origin of each box in the middle
  vec2 id = floor(uv);
  
  for (int y = -1;y <= 1; y++)
  {
    for (int x = -1; x <= 1; x++)
    {
      vec2 offset = vec2(x, y);
      float n = Hash21(id + offset);
      float size = fract(n * 345.32);
      float star = Star(gv - offset - vec2(n, fract(n * 34.)) + .5, smoothstep(.9, 1., size) * .6);
      vec3 c = sin(vec3(.2, .3, .9) * fract(n * 2345.2) * 123.2) * .5 + .5;
      c = c * vec3(1., .25, 1. + size);
      
      star *= sin(time * 3. + n * 6.2831) * .5 + 1.;  // sparkle
      col += star * size * c;
    }
  }
  
  return col;
}

vec4 effect(vec4 color, Image texture, vec2 texture_coords, vec2 screen_coords)
{
  vec2 uv = screen_coords / resolution - .5;  // normalize, put uv origin at center
  uv.x *= resolution.x / resolution.y;        // compensate for aspect ratio
  float t = time * .05;

  uv *= Rot(t);
  vec3 col = vec3(0);
  
  for (float i = 0.; i < 1.; i += 1. / NUM_LAYERS) {
    float depth = fract(i + t);
    float scale = mix(20., .5, depth);
    float fade = depth * smoothstep(1., .9, depth);
    col += StarLayer(uv * scale + i * 453.2) * fade;
  }
  
  return vec4(col, 1);              // use the same c value for r, g, b
}