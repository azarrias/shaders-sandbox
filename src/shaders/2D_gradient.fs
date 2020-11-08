extern vec2 resolution;

vec4 effect(vec4 color, Image texture, vec2 texture_coords, vec2 screen_coords)
{
  vec2 uv = vec2(screen_coords.x / resolution.x, screen_coords.y / resolution.y);
  return vec4(uv.x, 1 - uv.y, 0, 1);
}