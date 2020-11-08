extern vec2 resolution;

vec4 effect(vec4 color, Image texture, vec2 texture_coords, vec2 screen_coords)
{
  vec2 uv = vec2(screen_coords.x / resolution.x, screen_coords.y / resolution.y);
  uv -= .5;                             // make uv range -0.5 to 0.5
  uv.x *= resolution.x / resolution.y;  // multiply x by aspect ratio to compensate for the width vs height difference
  float d = length(uv);                 // distance from every pixel to the center
  float r = 0.3;                        // circle radius
  float c = smoothstep(r, r-0.005, d);  // smooth edge of the circle (reversed to go from high to low!)
  
  return vec4(vec3(c), 1);              // use the same c value for r, g and b
}